import CourseCardProps from "../components/courseComponents/Interfaces/courseCardProps";
import CourseListGroupProps from "../components/courseComponents/Interfaces/courseListGroupProps";
import CourseCard from "../components/courseComponents/courseCard";
import Container from "@mui/material/Container";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import { Typography } from "@mui/material";
import RequireAuth from "../components/RequireAuth";
import { useEffect, useState } from "react";
import axios from "axios";
import myLogo from "../components/Images/logo.jpg";

interface CourseCard {
  id: number;
  title: string;
  description: string;
  price: number;
  logo: string;
  authorId: string;
}

function Courses() {
  const [courses, setCourses] = useState<CourseCard[]>();

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
        <Container sx={{ bgcolor: "#707070" }}>
          <Grid
            container
            rowSpacing={1}
            columnSpacing={{ xs: 1, sm: 2, md: 3 }}>
            {courses?.map((course) => (
              <CourseCard
                title={course.title}
                description={course.description}
                price={course.price}
                logo={course.logo === "c://dataStorage" ? myLogo : course.logo}
              />
            ))}
          </Grid>
        </Container>
      </Container>
    </RequireAuth>
  );
}

export default Courses;
