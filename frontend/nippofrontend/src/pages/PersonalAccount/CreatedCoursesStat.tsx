import axios from "axios";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import RequireAuth from "../../components/RequireAuth";
import { Box, Container, Typography, Button, Paper } from "@mui/material";
import AddToQueueIcon from "@mui/icons-material/AddToQueue";

interface CourseCardProps {
  id: number;
  title: string;
  description: string;
  price: number;
  imgPath: string;
  authorId: string;
}

export const CreatedCoursesStat = () => {
  const [createdCourses, setCreatedCourses] = useState<CourseCardProps[]>();
  const [courseMembers, setCourseMembers] = useState<{ [key: number]: number }>(
    {}
  );
  const navigate = useNavigate();

  const handleGoToCourse = (courseId: number) => {
    navigate(`/course/${courseId}/create`);
  };

  async function getCreatedCourses() {
    try {
      const response = await axios.get(`/user/get-created-courses`);

      if (response.status === 200) {
        setCreatedCourses(response.data);
        const membersCount: { [key: number]: number } = {};
        for (const course of response.data) {
          try {
            const response = await axios.get(
              `/user/get-users-by-course?courseId=${course.id}`
            );

            if (response.status === 200) {
              membersCount[course.id] = response.data;
            }
          } catch (error) {
            console.error(error);
          }
        }

        setCourseMembers(membersCount);
      }
    } catch (error) {
      console.error(error);
    }
  }

  useEffect(() => {
    getCreatedCourses();
  }, []);

  return (
    <RequireAuth>
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
            Мои курсы
          </Typography>
        </Box>
        <Container
          sx={{ bgcolor: "#707070", padding: 1, borderRadius: "16px" }}>
          {(createdCourses === undefined || createdCourses.length === 0) && (
            <>
              <Typography
                sx={{
                  textAlign: "center",
                  marginY: 5,
                  fontWeight: "800",
                  fontSize: "36px",
                  color: "white",
                }}>
                У вас нет созданных курсов
                <Button onClick={() => navigate("/course/create")}>
                  <AddToQueueIcon
                    sx={{ color: "white", fontSize: "36px" }}
                    fontSize="large"
                  />
                </Button>
              </Typography>
            </>
          )}
          {createdCourses != undefined &&
            createdCourses.map((course: CourseCardProps) => (
              <Paper
                elevation={3}
                sx={{
                  height: "100%",
                  display: "flex",
                  flexDirection: "row",
                  justifyContent: "space-around",
                  margin: 1,
                }}>
                <Box
                  sx={{
                    marginTop: 2,
                    paddingX: 1,
                    marginBottom: 1,
                    display: "flex",
                    justifyContent: "center",
                    backgroundColor: "#858585",
                  }}>
                  <img
                    src={course.imgPath}
                    alt="course logo"
                    style={{ width: "200px", height: "200px" }}
                  />
                </Box>
                <Container
                  sx={{
                    height: "90%",
                    display: "flex",
                    flexDirection: "row",
                    justifyContent: "space-between",
                  }}>
                  <Box>
                    <Typography variant="h6" fontSize={18}>
                      {course.title}
                    </Typography>
                    <Typography
                      sx={{
                        overflow: "hidden",
                        WebkitLineClamp: 3,
                        WebkitBoxOrient: "vertical",
                        display: "-webkit-box",
                        textOverflow: "ellipsis",
                      }}>
                      {course.description}
                    </Typography>
                  </Box>
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "column",
                      justifyContent: "space-around",
                    }}>
                    <Box
                      sx={{
                        width: "100%",
                        height: 30,
                        padding: 1,
                        fontWeight: 700,
                      }}
                      margin={2}>
                      <Button
                        variant="contained"
                        color="success"
                        sx={{ width: "120px" }}
                        onClick={() => handleGoToCourse(course.id)}>
                        Редактировать
                      </Button>
                      <Typography>
                        Количество учеников: {courseMembers[course.id] || 0}
                      </Typography>
                    </Box>
                  </Box>
                </Container>
              </Paper>
            ))}
        </Container>
      </Container>
    </RequireAuth>
  );
};
