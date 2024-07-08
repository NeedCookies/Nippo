import Container from "@mui/material/Container";
import CourseCardProps from "../components/courseComponents/Interfaces/courseCardProps";
import CourseListGroupProps from "../components/courseComponents/Interfaces/courseListGroupProps";
import Grid from "@mui/material/Grid";
import CourseCard from "../components/courseComponents/courseCard";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import RequireAuth from "../components/RequireAuth";

function likedCoursesList({ cards }: CourseListGroupProps) {
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
            Избранные курсы
          </Typography>
        </Box>
        <Container sx={{ bgcolor: "#707070" }}>
          <Grid
            container
            rowSpacing={1}
            columnSpacing={{ xs: 1, sm: 2, md: 3 }}>
            {cards.map((card: CourseCardProps) => (
              <CourseCard
                title={card.title}
                description={card.description}
                price={card.price}
                logo={card.logo}
              />
            ))}
          </Grid>
        </Container>
      </Container>
    </RequireAuth>
  );
}

export default likedCoursesList;
