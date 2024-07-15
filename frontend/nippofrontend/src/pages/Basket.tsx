import {
  Box,
  Typography,
  Container,
  Paper,
  Button,
  Snackbar,
  Alert,
} from "@mui/material";
import RequireAuth from "../components/RequireAuth";
import DeleteIcon from "@mui/icons-material/Delete";
import { useEffect, useState } from "react";
import axios from "axios";
import AddToQueueIcon from "@mui/icons-material/AddToQueue";
import { useNavigate } from "react-router-dom";
import { BuyModal } from "./BuyModal";
import courses from "../components/FakeData/fakeCourses";

interface CourseCardProps {
  id: number;
  title: string;
  description: string;
  price: number;
  imgPath: string;
  authorId: string;
}

interface BasketCourseProps {
  id: number;
  courseId: number;
  userId: string;
}

interface BuyModalProps {
  courseId: number;
  title: string;
  price: number;
}

export const Basket = () => {
  const navigate = useNavigate();
  const [basketCoursesIds, setBasketCoursesIds] =
    useState<BasketCourseProps[]>();
  const [basketCourses, setBasketCourses] = useState<CourseCardProps[]>();
  const [alreadyBought, setAlreadyBought] = useState<boolean>(false);
  const [lackOfPoints, setLackOfPoints] = useState<boolean>(false);
  const [isBuyModalOpen, setBuyModalOpen] = useState<boolean>(false);
  const [buyingCourse, setBuyingCourse] = useState<BuyModalProps>({
    courseId: 0,
    title: "",
    price: -1,
  });

  const handleCloseBuyModal = () => {
    setBuyModalOpen(false);
  };

  const handleClose = (
    event: React.SyntheticEvent | Event,
    reason?: string
  ) => {
    if (reason === "clickaway") {
      return;
    }

    setAlreadyBought(false);
    setLackOfPoints(false);
  };

  async function getBasketCourses() {
    try {
      const response = await axios.get(`/course/get-basket-courses`);

      if (response.status === 200) {
        setBasketCoursesIds(response.data);
      }
    } catch (error) {
      console.error(error);
    }
  }
  async function fetchCoursesData() {
    if (basketCoursesIds === undefined || basketCoursesIds.length === 0) return;
    console.log(basketCoursesIds);
    try {
      const coursePromises = basketCoursesIds.map((bc) => {
        console.log(bc.courseId);
        return axios.get(`/course/get-course?id=${bc.courseId}`);
      });

      const responses = await Promise.all(coursePromises);
      const courseData = responses.map((response) => response.data);
      setBasketCourses(courseData);
    } catch (error) {
      console.error(error);
    }
  }

  const handleBuyCourseClick = ({ courseId, title, price }: BuyModalProps) => {
    const modalCourse = {
      courseId: courseId,
      title: title,
      price: price,
    };
    setBuyingCourse(modalCourse);
    setBuyModalOpen(true);
  };

  const handleDeleteClick = async (courseId: number) => {
    try {
      const response = await axios.post(
        `/course/remove-from-basket?courseId=${courseId}`
      );
      if (response.status === 200) {
        setBasketCoursesIds((prevBasketCoursesIds) => {
          if (!prevBasketCoursesIds) return prevBasketCoursesIds;
          return prevBasketCoursesIds.filter(
            (item) => item.courseId !== courseId
          );
        });
      } else {
        console.log("Another response status");
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    getBasketCourses();
  }, []);

  useEffect(() => {
    fetchCoursesData();
  }, [basketCoursesIds]);

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
            Корзина
          </Typography>
        </Box>
        <Container
          sx={{ bgcolor: "#707070", padding: 1, borderRadius: "16px" }}>
          {(basketCourses === undefined || basketCourses.length === 0) && (
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
          {basketCourses != undefined &&
            basketCourses.map((course: CourseCardProps) => (
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
                        onClick={() =>
                          handleBuyCourseClick({
                            courseId: course.id,
                            title: course.title,
                            price: course.price,
                          })
                        }
                        variant="contained"
                        color="success"
                        sx={{ width: "150px" }}>
                        Купить {course.price}
                      </Button>
                    </Box>
                    <Box
                      sx={{
                        width: "100%",
                        height: 30,
                        padding: 1,
                        fontWeight: 700,
                      }}
                      margin={2}>
                      <Button
                        onClick={() => handleDeleteClick(course.id)}
                        variant="contained"
                        color="error"
                        startIcon={<DeleteIcon />}
                        sx={{ width: "150px" }}>
                        Удалить
                      </Button>
                    </Box>
                  </Box>
                </Container>
              </Paper>
            ))}
        </Container>
      </Container>
      <BuyModal
        courseId={buyingCourse.courseId}
        courseTitle={buyingCourse.title}
        price={buyingCourse.price}
        handleClose={handleCloseBuyModal}
        isOpen={isBuyModalOpen}
      />
    </RequireAuth>
  );
};
