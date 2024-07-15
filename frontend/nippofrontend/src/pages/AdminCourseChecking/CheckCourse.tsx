import {
  Alert,
  Box,
  Button,
  Container,
  Snackbar,
  Typography,
} from "@mui/material";
import axios from "axios";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { QuizCheckModal } from "./QuizModal";

interface CourseDataProps {
  id: number;
  title: string;
  description: string;
  price: number;
  imgPath: string;
  authorId: string;
}

interface ModuleProps {
  id: number;
  title: string;
  type: string;
  order: number;
}

export const CheckCourse = () => {
  const navigate = useNavigate();
  const { courseId } = useParams();
  const [acceptCourse, setAcceptCourse] = useState<boolean>(false);
  const [cancelCourse, setCancelCourse] = useState<boolean>(false);
  const [isQuizModalOpen, setQuizModalOpen] = useState<boolean>(false);
  const [chosenQuizId, setChosenQuizId] = useState<number>(1);
  const [chosenLessonId, setChosenLessonId] = useState<number>(1);
  const [isLessonModalOpen, setLessonModalOpen] = useState<boolean>(false);
  const [courseData, setCourseData] = useState<CourseDataProps>({
    id: 0,
    title: "",
    description: "",
    price: 0,
    imgPath: "",
    authorId: "",
  });
  const [modules, setModules] = useState<ModuleProps[]>();

  const handleAlertClose = (
    event: React.SyntheticEvent | Event,
    reason?: string
  ) => {
    if (reason === "clickaway") {
      return;
    }

    setAcceptCourse(false);
    setCancelCourse(false);
  };

  const handleCloseModal = () => {
    setQuizModalOpen(false);
    setLessonModalOpen(false);
  };

  const handleAcceptCourse = async () => {
    try {
      const response = await axios.post("/admin/accept-course", {
        courseId: courseId,
      });

      if (response.status === 200) {
        setAcceptCourse(true);
        navigate(`/admin/courses-to-check`);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const handleCancelCourse = async () => {
    try {
      const response = await axios.post("/admin/cancel-course", {
        courseId: courseId,
      });

      if (response.status === 200) {
        setCancelCourse(true);
        navigate(`/admin/courses-to-check`);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const handleCheckModule = (module: ModuleProps) => {
    if (module.type === "lesson") {
      setChosenLessonId(module.id);
      setLessonModalOpen(true);
    } else {
      setChosenQuizId(module.id);
      setQuizModalOpen(true);
    }
  };

  useEffect(() => {
    getCourseData();
  }, []);
  async function getCourseData() {
    try {
      const response = await axios.get(`course/get-course?id=${courseId}`);
      if (response.status === 200) {
        courseData.title = response.data.title;
        courseData.description = response.data.description;
        courseData.price = response.data.price;
        courseData.imgPath = response.data.imgPath;
        await getLessons();
        getQuizzes();
      } else {
        console.log("Another response status");
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  }
  async function getLessons() {
    try {
      const response = await axios.get(
        `/lesson/get-by-course?courseId=${courseId}`
      );
      if (response.status === 200) {
        const lessonsWithType = response.data.map((lesson: any) => ({
          ...lesson,
          type: "lesson",
        }));
        setModules(lessonsWithType);
      } else {
        console.log("Another response status");
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  }
  async function getQuizzes() {
    try {
      const response = await axios.get(
        `/quiz/get-by-course?courseId=${courseId}`
      );
      if (response.status === 200) {
        const quizzesWithType = response.data.map((quiz: any) => ({
          ...quiz,
          type: "quiz",
        }));
        setModules((prevModules) => {
          if (prevModules) {
            return [...prevModules, ...quizzesWithType];
          } else {
            return quizzesWithType;
          }
        });
      } else {
        console.log("Another response status");
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  }

  return (
    <Container>
      <Box
        sx={{
          margin: "1rem",
          padding: "0",
          bgcolor: "#a3a3a3",
          textAlign: "center",
          borderRadius: "16px",
        }}>
        <Typography variant="h4" sx={{ fontWeight: "500" }}>
          Проверка курса
        </Typography>
      </Box>
      <Container
        sx={{
          bgcolor: "#707070",
          display: "flex",
          justifyContent: "space-between",
          borderRadius: "10px",
        }}>
        <Box sx={{ width: "30%" }}>
          <Box
            textAlign="center"
            sx={{
              flex: 4,
              backgroundColor: "#dedede",
              margin: 1,
              borderRadius: "6px",
            }}>
            <Typography>Название: {courseData.title}</Typography>
          </Box>
          <Box
            padding={1}
            sx={{
              flex: 4,
              backgroundColor: "#dedede",
              margin: 1,
              borderRadius: "6px",
            }}>
            <Typography>Описание: {courseData.description}</Typography>
          </Box>
          <Box
            textAlign="center"
            sx={{
              flex: 4,
              backgroundColor: "#dedede",
              margin: 1,
              borderRadius: "6px",
            }}>
            <Typography>Цена: {courseData.price}</Typography>
          </Box>
          <Box textAlign="center">
            <Typography color="white">Логотип курса:</Typography>
            <img src={courseData.imgPath} alt="course logo" width="90%" />
          </Box>
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
            }}>
            <Button
              variant="contained"
              color="success"
              sx={{ margin: 1, width: "90%" }}
              onClick={handleAcceptCourse}>
              Одобрить
            </Button>
            <Button
              variant="contained"
              color="error"
              sx={{ margin: 1, width: "90%" }}
              onClick={handleCancelCourse}>
              Отправить на доработку
            </Button>
          </Box>
        </Box>
        <Box>
          {modules?.map((module: ModuleProps) => (
            <Box
              sx={{
                backgroundColor: "#bfbfbf",
                marginY: 1,
                borderRadius: "4px",
                display: "flex",
                justifyContent: "space-between",
                flexWrap: "wrap",
              }}>
              <Typography
                key={module.id}
                textAlign={"start"}
                sx={{ width: "50%", padding: 1 }}>
                {module.order}. {module.title}
              </Typography>
              <Typography textAlign={"end"} sx={{ width: "50%", padding: 1 }}>
                тип: {module.type}
              </Typography>
              <Button
                variant="contained"
                size="small"
                sx={{ margin: 1 }}
                onClick={() => handleCheckModule(module)}>
                Проверить
              </Button>
            </Box>
          ))}
        </Box>
        <QuizCheckModal
          isOpen={isQuizModalOpen}
          handleClose={handleCloseModal}
          quizId={chosenQuizId}
          courseId={Number(courseId)}
        />
        <Snackbar
          open={cancelCourse}
          autoHideDuration={5000}
          onClose={handleAlertClose}>
          <Alert
            onClose={handleAlertClose}
            severity="info"
            variant="filled"
            sx={{ width: "100%" }}>
            Курс отправлен автору на доработку
          </Alert>
        </Snackbar>
        <Snackbar
          open={acceptCourse}
          autoHideDuration={5000}
          onClose={handleAlertClose}>
          <Alert
            onClose={handleAlertClose}
            severity="success"
            variant="filled"
            sx={{ width: "100%" }}>
            Курс размещен в общем доступе
          </Alert>
        </Snackbar>
      </Container>
    </Container>
  );
};
