import Container from "@mui/material/Container";
import Box from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import { useEffect, useState } from "react";
import Input from "@mui/material/Input";
import Button from "@mui/material/Button";
import { useNavigate, useParams } from "react-router-dom";
import { QuizModal } from "./QuizModal";
import { LessonModal } from "./LessonModal";
import axios from "axios";

interface Module {
  id: number;
  title: string;
  type: string;
  order: number;
}

interface CourseDataProps {
  Title: string;
  Description: string;
  Price: string;
  ImgPath: string;
}

function createCourse() {
  const { courseId } = useParams<{ courseId?: string }>();

  const [courseIdState, setCourseIdState] = useState<number>(-1);
  const [isCourseSaved, setCourseSaved] = useState<boolean>(false);
  const [modules, setModules] = useState<Module[]>([]);
  const [isQuizModalOpen, setQuizModalOpen] = useState<boolean>(false);
  const [isLessonModalOpen, setLessonModalOpen] = useState<boolean>(false);
  const [courseData, setCourseData] = useState<CourseDataProps>({
    Title: "",
    Description: "",
    Price: "",
    ImgPath: "",
  });
  const [logo, setLogo] = useState<File | null>(null);

  const navigate = useNavigate();
  const haldleCloseModal = () => {
    setQuizModalOpen(false);
    setLessonModalOpen(false);
  };

  const handleLogoButton = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      setLogo(e.target.files[0]);
    }
  };

  const handleSaveCourse = async () => {
    try {
      const formData = new FormData();
      formData.append("Title", courseData.Title);
      formData.append("Description", courseData.Description);
      formData.append("Price", courseData.Price);
      if (logo) {
        formData.append("ImgPath", logo);
      }

      const response = await axios.post("/course/create-course", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      if (response.status === 200) {
        setCourseData((prev) => ({
          ...prev!,
          Title: response.data.title,
          Description: response.data.description,
          Price: response.data.price,
          Logo: response.data.imgPath,
        }));
        setCourseSaved(true);
        setCourseIdState(response.data.id);
        navigate(`/course/${response.data.id}/create`);
      } else {
        console.log(response.status);
        console.log(response);
      }
    } catch (error) {
      console.error("Can't create course", error);
    }
  };
  const handleSaveCourseChanges = async () => {
    try {
      const formData = new FormData();
      if (courseId) {
        formData.append("Id", courseId);
      }
      formData.append("Title", courseData.Title);
      formData.append("Description", courseData.Description);
      formData.append("Price", courseData.Price);
      if (logo) {
        formData.append("ImgPath", logo);
      }

      const response = await axios.post("/course/edit-course", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      if (response.status === 200) {
        setCourseData((prev) => ({
          ...prev!,
          Title: response.data.title,
          Description: response.data.description,
          Price: response.data.price,
          Logo: response.data.imgPath,
        }));
        setCourseSaved(false);
        setCourseSaved(true);
        setCourseIdState(response.data.id);
        getCourseData();
      } else {
        console.log(response.status);
        console.log(response);
      }
    } catch (error) {
      console.error("Can't update course", error);
    }
  };

  const handleAddModules = (type: string) => {
    if (type === "lesson") {
      setLessonModalOpen(true);
    } else {
      setQuizModalOpen(true);
    }
  };

  const handleDeleteModule = (courseModule: any) => {
    const clearModules = modules.filter(
      (module) => module.id !== courseModule.id
    );
    const moduleType = courseModule.type === "lesson" ? "lesson" : "quiz";
    axios.delete(`${moduleType}/delete?${moduleType}Id=${courseModule.id}`);
    setModules(clearModules);
  };

  const handleEditModule = (module: any) => {
    const moduleType = module.type === "lesson" ? "lesson" : "quiz";
    navigate(
      "/course/" + courseId + "/" + moduleType + "/" + module.id + "/edit"
    );
  };

  const handleCourseFinished = async () => {
    try {
      const response = await axios.post(`/course/submit-for-review`, {
        courseId: Number(courseId),
      });
      if (response.status === 200) {
        console.log("Курс отправлен на проверку");
      }
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    if (courseId) {
      getCourseData();
      setCourseIdState(Number(courseId));
    }
  }, [courseId]);

  useEffect(() => {
    async function getData() {
      await getLessons();
      getQuizzes();
    }
    getData();
  }, []);

  async function getCourseData() {
    try {
      const response = await axios.get(`course/get-course?id=${courseId}`);
      if (response.status === 200) {
        courseData.Title = response.data.title;
        courseData.Description = response.data.description;
        courseData.Price = response.data.price;
        courseData.ImgPath = response.data.imgPath;
        setCourseSaved(true);
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
          const filteredModules = prevModules.filter(
            (module) => module.type !== "quiz"
          );
          return [...filteredModules, ...quizzesWithType];
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
          Создание курса
        </Typography>
      </Box>
      <Container
        sx={{
          bgcolor: "#707070",
          display: "flex",
          justifyContent: "space-between",
          borderRadius: "10px",
        }}>
        <Box
          sx={{
            flex: 4,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
          }}>
          <Typography>Название:</Typography>
          <TextField
            id="filled-basic"
            label="Название курса"
            variant="filled"
            sx={{ width: "100%" }}
            value={courseData.Title}
            onChange={(e) =>
              setCourseData({ ...courseData, Title: e.target.value })
            }
          />
          <Typography>Описание:</Typography>
          <TextField
            id="filled-basic"
            label="Описание курса"
            variant="filled"
            sx={{ width: "100%" }}
            value={courseData.Description}
            onChange={(e) =>
              setCourseData({ ...courseData, Description: e.target.value })
            }
            multiline
          />
          <Typography>Цена курса:</Typography>
          <TextField
            id="filled-basic"
            label="Цена курса должна быть числом"
            variant="filled"
            sx={{ width: "100%" }}
            type="number"
            inputProps={{
              pattern: "^[0-9]+(.[0-9]+)?$",
              title: "Введите положительное число",
              min: 0,
            }}
            value={courseData.Price}
            onChange={(e) =>
              setCourseData({ ...courseData, Price: e.target.value })
            }
            multiline
          />
          <Typography>Логотип курса:</Typography>
          <Input
            type="file"
            name="image"
            onChange={handleLogoButton}
            sx={{ width: "100%" }}
          />
          {courseData.ImgPath && (
            <Box
              sx={{
                marginTop: 2,
                paddingX: 1,
                marginBottom: 1,
                width: "100%",
                display: "flex",
                justifyContent: "center",
                backgroundColor: "#858585",
              }}>
              <img
                src={courseData.ImgPath}
                alt="Preview"
                style={{ maxWidth: "100%", maxHeight: "200px" }}
              />
            </Box>
          )}
          {!isCourseSaved && (
            <Button onClick={handleSaveCourse}>Сохранить</Button>
          )}
          {isCourseSaved && (
            <Button onClick={handleSaveCourseChanges}>
              Сохранить изменения
            </Button>
          )}
        </Box>
        <Box
          sx={{
            flex: 6,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
          }}>
          {modules.map((module: any) => (
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
                sx={{ width: "50%" }}>
                {module.order}. {module.title}
              </Typography>
              <Typography textAlign={"end"} sx={{ width: "50%" }}>
                тип: {module.type}
              </Typography>
              <Button
                variant="contained"
                size="small"
                sx={{ marginY: 1 }}
                onClick={() => handleDeleteModule(module)}>
                Удалить
              </Button>
              <Button
                variant="contained"
                size="small"
                sx={{ marginY: 1 }}
                onClick={() => handleEditModule(module)}>
                Редактировать
              </Button>
            </Box>
          ))}
        </Box>
        <Box
          sx={{
            flex: 2,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
            display: "flex",
            flexDirection: "column",
            justifyContent: "start",
          }}>
          <Button
            variant="contained"
            size="large"
            sx={{ margin: 2 }}
            onClick={() => handleAddModules("lesson")}
            disabled={!isCourseSaved}>
            Создать урок
          </Button>
          <Button
            variant="contained"
            size="large"
            sx={{ margin: 2 }}
            onClick={() => handleAddModules("test")}
            disabled={!isCourseSaved}>
            Создать тест
          </Button>
          {!isCourseSaved && (
            <Typography sx={{ fontFamily: "cursive", fontStyle: "italic" }}>
              Сохраните курс, чтобы создать тесты и уроки
            </Typography>
          )}
          {isCourseSaved && (
            <Button
              variant="contained"
              size="large"
              sx={{ margin: 2 }}
              onClick={handleCourseFinished}
              disabled={!isCourseSaved}>
              Курс готов
            </Button>
          )}
          <QuizModal
            courseId={courseIdState}
            isOpen={isQuizModalOpen}
            handleClose={haldleCloseModal}></QuizModal>
          <LessonModal
            courseId={courseIdState}
            isOpen={isLessonModalOpen}
            handleClose={haldleCloseModal}></LessonModal>
        </Box>
      </Container>
    </Container>
  );
}

export default createCourse;
