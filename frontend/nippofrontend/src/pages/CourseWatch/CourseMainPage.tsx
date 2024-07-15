import { Box, Container, Typography } from "@mui/material";
import axios from "axios";
import { useEffect, useState } from "react";
import { Button } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";

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

interface QuizResult {
  id: number;
  quizId: number;
  score: number;
  attempt: number;
}

export const CourseMainPage = () => {
  const navigate = useNavigate();
  const { courseId } = useParams();
  const [courseData, setCourseData] = useState<CourseDataProps>();
  const [modules, setModules] = useState<ModuleProps[]>();
  const [quizResults, setQuizResults] = useState<QuizResult[]>();

  const handleGoToQuiz = async (quizId: number) => {
    navigate(`/my-course/${courseId}/quiz/${quizId}`);
  };

  const handleGoToLesson = async (lessonId: number) => {
    navigate(`/my-course/lesson/${lessonId}`);
  };

  async function getCourseData(courseId: number) {
    try {
      const response = await axios.get(`/course/get-course?id=${courseId}`);
      if (response.status === 200) {
        setCourseData((prev) => ({
          ...prev!,
          title: response.data.title,
          description: response.data.description,
          price: response.data.price,
          imgPath: response.data.imgPath,
        }));
      } else {
        console.log("Another response status");
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  }

  async function getQuizzes(courseId: number) {
    try {
      const response = await axios.get(
        `quiz/get-by-course?courseId=${courseId}`
      );
      if (response.status === 200) {
        const quizModules = response.data.map((module: any) => ({
          id: module.id,
          title: module.title,
          type: "quiz",
          order: module.order,
        }));
        quizModules.map(async (q: any) => {
          try {
            const response = await axios.get(
              `/quizresult/get-by-quiz?quizId=${q.id}`
            );

            if (response.status === 200) {
              setQuizResults((prev) => {
                if (prev) {
                  return [...prev, response.data];
                } else {
                  return [response.data];
                }
              });
            }
          } catch (error) {
            console.error(error);
          }
        });
        setModules(() => {
          const newModules = quizModules;
          return newModules;
        });
      } else {
        console.log("Another response status");
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  }

  async function getLessons(courseId: number) {
    try {
      const response = await axios.get(
        `lesson/get-by-course?courseId=${courseId}`
      );
      if (response.status === 200) {
        const quizModules = response.data.map((module: any) => ({
          id: module.id,
          title: module.title,
          type: "lesson",
          order: module.order,
        }));
        setModules((prevModules) => {
          const newModules = prevModules
            ? [...prevModules, ...quizModules]
            : quizModules;
          return newModules;
        });
      } else {
        console.log("Another response status");
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  }

  async function sortModules() {
    if (modules && modules.length > 0) {
      const sortedModules = [...modules].sort((a, b) => a.order - b.order);
      setModules(sortedModules);
    }
  }

  useEffect(() => {
    getCourseData(Number(courseId));
    getQuizzes(Number(courseId));
    getLessons(Number(courseId));
    sortModules();
  }, []);

  return (
    <Container>
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
          <Box
            sx={{
              marginTop: 2,
              paddingX: 1,
              marginBottom: 1,
              display: "flex",
              backgroundColor: "#858585",
            }}>
            <img src={courseData?.imgPath} alt="course-logo" width="30%" />
            <Box textAlign="center" width="70%">
              <Typography marginY={2} variant="h3" sx={{ fontWeight: "500" }}>
                Курс
              </Typography>
              <Typography variant="h3" sx={{ fontWeight: "500" }}>
                {courseData?.title}
              </Typography>
            </Box>
          </Box>
          <Box>
            <Typography margin={2} variant="h4" sx={{ fontWeight: "500" }}>
              Описание курса
            </Typography>
            <Typography margin={1} fontSize="18px">
              {courseData?.description}
            </Typography>
          </Box>
        </Box>
      </Container>
      <Box>
        <Typography variant="h4" sx={{ fontWeight: "500" }}>
          Содержание курса
        </Typography>
        {modules?.map((module: ModuleProps) => (
          <Box
            sx={{
              backgroundColor: "#bfbfbf",
              marginY: 1,
              borderRadius: "4px",
            }}>
            <Box
              marginX={1}
              padding={1}
              sx={{
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
            </Box>
            <Box
              marginX={1}
              padding={1}
              sx={{
                display: "flex",
                justifyContent: "space-between",
                flexWrap: "wrap",
              }}>
              <Button
                onClick={() => {
                  module.type === "quiz"
                    ? handleGoToQuiz(module.id)
                    : handleGoToLesson(module.id);
                }}>
                Пройти
              </Button>
              {quizResults && (
                <Typography>
                  Оценка:{" "}
                  {quizResults.find((quizRes) => quizRes.quizId === module.id)
                    ?.score + "/10" ?? "Не пройден"}
                </Typography>
              )}
            </Box>
          </Box>
        ))}
      </Box>
    </Container>
  );
};
