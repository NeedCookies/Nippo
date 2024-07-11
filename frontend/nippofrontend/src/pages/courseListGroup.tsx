import CourseListGroupProps from "../components/courseComponents/Interfaces/courseListGroupProps";
import CourseCard from "../components/courseComponents/courseCard";
import Container from "@mui/material/Container";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import { Paper, Typography, Button } from "@mui/material";
import RequireAuth from "../components/RequireAuth";
import { useEffect, useState } from "react";
import axios from "axios";
import myLogo from "../components/Images/logo.jpg";
import AddShoppingCartIcon from "@mui/icons-material/AddShoppingCart";

interface CourseCardProps {
  id: number;
  title: string;
  description: string;
  price: number;
  logo: string;
  authorId: string;
}

function Courses() {
  const [courses, setCourses] = useState<CourseCardProps[]>();

  const handleAddToBasket = (courseId: number) => {
    async function addToBasket(courseId: number) {
      try {
        const response = await axios.post(
          `/course/add-to-basket?courseId=${courseId}`
        );

        if (response.status === 200) {
          console.log("Курс успешно добавлен в корзину");
          console.log("Надо сделать Snackbat");
        } else {
          console.log(response.status);
        }
      } catch (error) {
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
                  <Container
                    component="img"
                    src={course.logo}
                    alt="Course logo"
                    sx={{
                      height: 233,
                      width: 350,
                      maxHeight: { xs: 233, md: 167 },
                      maxWidth: { xs: 350, md: 250 },
                    }}
                  />
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
      </Container>
    </RequireAuth>
  );
}

export default Courses;
