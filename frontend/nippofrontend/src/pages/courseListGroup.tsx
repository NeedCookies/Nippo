import CourseCardProps from "../components/courseComponents/Interfaces/courseCardProps";
import CourseListGroupProps from "../components/courseComponents/Interfaces/courseListGroupProps";
import CourseCard from "../components/courseComponents/courseCard";
import Container from "@mui/material/Container";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import { Typography } from "@mui/material";

function Courses({ cards }: CourseListGroupProps) {
  return (
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
        <Grid container rowSpacing={1} columnSpacing={{ xs: 1, sm: 2, md: 3 }}>
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
  );
}

export default Courses;
