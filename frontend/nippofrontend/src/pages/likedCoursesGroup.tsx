import Container from "@mui/material/Container";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import RequireAuth from "../components/RequireAuth";
import { useEffect, useState } from "react";
import axios from "axios";
import { Button, Paper } from "@mui/material";
import AddToQueueIcon from "@mui/icons-material/AddToQueue";
import { useNavigate } from "react-router-dom";

interface CourseCardProps {
  id: number;
  title: string;
  description: string;
  price: number;
  imgPath: string;
  authorId: string;
}

function likedCoursesList() {
  const [userCourses, setUserCourses] = useState<CourseCardProps[]>();
  const navigate = useNavigate();

  async function getUserCourses() {
    try {
      const response = await axios.get(`/user/get-courses`);

      if (response.status === 200) {
        setUserCourses(response.data);
      }
    } catch (error) {
      console.error(error);
    }
  }

  useEffect(() => {
    getUserCourses();
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
            Курсы, которые я прохожу
          </Typography>
        </Box>
        <Container
          sx={{ bgcolor: "#707070", padding: 1, borderRadius: "16px" }}>
          {(userCourses === undefined || userCourses.length === 0) && (
            <>
              <Typography
                sx={{
                  textAlign: "center",
                  marginY: 5,
                  fontWeight: "800",
                  fontSize: "36px",
                  color: "white",
                }}>
                Корзина пуста
                <Button onClick={() => navigate("/courses")}>
                  <AddToQueueIcon
                    sx={{ color: "white", fontSize: "36px" }}
                    fontSize="large"
                  />
                </Button>
              </Typography>
            </>
          )}
          {userCourses != undefined &&
            userCourses.map((course: CourseCardProps) => (
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
                        sx={{ width: "120px" }}>
                        Продолжить учиться
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
}

export default likedCoursesList;
