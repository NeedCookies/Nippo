import axios from "axios";
import { useEffect, useState } from "react";
import Container from "@mui/material/Container";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { Button, Paper } from "@mui/material";
import RequireAuth from "../../components/RequireAuth";
import { useNavigate } from "react-router-dom";
import AddToQueueIcon from "@mui/icons-material/AddToQueue";

interface CourseCardProps {
  id: number;
  title: string;
  description: string;
  price: number;
  imgPath: string;
  authorId: string;
}

export const CoursesChecking = () => {
  const navigate = useNavigate();
  const [toCkeckCourses, setToCkeckCourses] = useState<CourseCardProps[]>();

  const handleGoToCourse = (courseId: number) => {
    navigate(`/check-course/${courseId}`);
  };

  async function getToCkeckCourses() {
    try {
      const response = await axios.get(`/admin/get-courses-to-check`);

      if (response.status === 200) {
        setToCkeckCourses(response.data);
      }
    } catch (error) {
      console.error(error);
    }
  }

  useEffect(() => {
    getToCkeckCourses();
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
            Курсы для проверки
          </Typography>
        </Box>
        <Container
          sx={{ bgcolor: "#707070", padding: 1, borderRadius: "16px" }}>
          {(toCkeckCourses === undefined || toCkeckCourses.length === 0) && (
            <>
              <Typography
                sx={{
                  textAlign: "center",
                  marginY: 5,
                  fontWeight: "800",
                  fontSize: "36px",
                  color: "white",
                }}>
                Нет курсов для проверки
                <Button onClick={() => navigate("/courses")}>
                  <AddToQueueIcon
                    sx={{ color: "white", fontSize: "36px" }}
                    fontSize="large"
                  />
                </Button>
              </Typography>
            </>
          )}
          {toCkeckCourses != undefined &&
            toCkeckCourses.map((course: CourseCardProps) => (
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
                        Проверить
                      </Button>
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
