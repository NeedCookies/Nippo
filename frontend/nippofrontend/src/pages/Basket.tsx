import { Box, Typography, Container } from "@mui/material";
import RequireAuth from "../components/RequireAuth";
import CourseCard from "../components/courseComponents/courseCard";
import { useEffect, useState } from "react";
import axios from "axios";

interface CourseCardProps {
  id: number;
  title: string;
  description: string;
  price: number;
  logo: string;
  authorId: string;
}

export const Basket = () => {
  const [basketCoursesIds, setBasketCoursesIds] = useState<number[]>();
  const [basketCourses, setBasketCourses] = useState<CourseCardProps[]>();

  useEffect(() => {
    const getBasketCourses = async () => {
      try {
        const response = await axios.get(`/course/get-basket-courses`);

        if (response.status === 200) {
          const courseIds = response.data.map((bc: any) => bc.CourseId);
          setBasketCoursesIds(courseIds);
          fetchCoursesData();
        }
      } catch (error) {
        console.error(error);
      }
    };

    const fetchCoursesData = async () => {
      if (basketCoursesIds === undefined || basketCoursesIds.length === 0)
        return;
      try {
        const coursePromises = basketCoursesIds.map((bcId) =>
          axios.get(`/course/get-course?id=${bcId}`)
        );

        const responses = await Promise.all(coursePromises);
        const courseData = responses.map((response) => response.data);
        setBasketCourses(courseData);
      } catch (error) {
        console.error(error);
      }
    };
    getBasketCourses();
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
            Корзина
          </Typography>
        </Box>
        <Container sx={{ bgcolor: "#707070" }}>
          {basketCourses != undefined &&
            basketCourses.map((course: CourseCardProps) => (
              <CourseCard
                title={course.title}
                description={course.description}
                price={course.price}
                logo={course.logo}
              />
            ))}
        </Container>
      </Container>
    </RequireAuth>
  );
};
