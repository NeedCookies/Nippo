import Container from "@mui/material/Container";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import { Paper, Typography, Button, Snackbar, Alert } from "@mui/material";
import RequireAuth from "../components/RequireAuth";
import { useEffect, useState } from "react";
import axios from "axios";
import AddShoppingCartIcon from "@mui/icons-material/AddShoppingCart";

interface CourseCardProps {
  id: number;
  title: string;
  description: string;
  price: number;
  imgPath: string;
  authorId: string;
}

function Courses() {
  const [alreadyInBasket, setAlreadyInBasket] = useState<boolean>(false);
  const [addedToBasket, setAddedToBasket] = useState<boolean>(false);
  const [courses, setCourses] = useState<CourseCardProps[]>();

  const handleClose = (
    _: React.SyntheticEvent | Event,
    reason?: string
  ) => {
    if (reason === "clickaway") {
      return;
    }

    setAlreadyInBasket(false);
    setAddedToBasket(false);
  };

  const handleAddToBasket = (courseId: number) => {
    async function addToBasket(courseId: number) {
      try {
        const response = await axios.post(
          `/course/add-to-basket?courseId=${courseId}`
        );

        if (response.status === 200) {
          setAddedToBasket(true);
        } else {
          console.log(response.status);
        }
      } catch (error) {
        setAlreadyInBasket(true);
        console.error(error);
      }
    }
    addToBasket(courseId);
  };

  useEffect(() => {
    async function getCourses() {
      try {
        const response = await axios.get(`course/get-all-courses`);
        if (response.status === 200) {
          setCourses(response.data);
        } else {
          console.log("Another response status");
          console.log(response.status);
        }
      } catch (error) {
        console.error(error);
      }
    }
    getCourses();
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
            Выбор курсов
          </Typography>
        </Box>
        <Container
          sx={{ bgcolor: "#707070", padding: 1, borderRadius: "16px" }}>
          <Grid
            container
            rowSpacing={1}
            columnSpacing={{ xs: 1, sm: 2, md: 3 }}>
            {courses?.map((course) => (
              <Grid item xs={6} md={4} lg={3}>
                <Paper
                  elevation={3}
                  sx={{
                    height: "100%",
                    display: "flex",
                    flexDirection: "column",
                  }}>
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
                      src={course.imgPath}
                      alt="Course logo"
                      style={{ maxWidth: "100%", height: "200px" }}
                    />
                  </Box>
                  <Container
                    sx={{
                      height: "90%",
                      display: "flex",
                      flexDirection: "column",
                      justifyContent: "space-between",
                    }}>
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
                    <Button onClick={() => handleAddToBasket(course.id)}>
                      <Box
                        sx={{
                          width: 200,
                          height: 30,
                          padding: 1,
                          fontWeight: 700,
                        }}>
                        <AddShoppingCartIcon
                          sx={{
                            paddingRight: "0.5rem",
                            paddingBottom: "0.5rem",
                          }}
                        />
                        {course.price}
                      </Box>
                    </Button>
                  </Container>
                </Paper>
              </Grid>
            ))}
          </Grid>
        </Container>
        <Snackbar
          open={alreadyInBasket}
          autoHideDuration={5000}
          onClose={handleClose}>
          <Alert
            onClose={handleClose}
            severity="info"
            variant="filled"
            sx={{ width: "100%" }}>
            Курс уже куплен вами, или находится в вашей корзине
          </Alert>
        </Snackbar>
        <Snackbar
          open={addedToBasket}
          autoHideDuration={5000}
          onClose={handleClose}>
          <Alert
            onClose={handleClose}
            severity="success"
            variant="filled"
            sx={{ width: "100%" }}>
            Курс добавлен в корзину
          </Alert>
        </Snackbar>
      </Container>
    </RequireAuth>
  );
}

export default Courses;
