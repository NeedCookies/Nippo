import CourseCardProps from "./Interfaces/courseCardProps";
import Paper from "@mui/material/Paper";
import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import Link from "@mui/material/Link";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import AddShoppingCartIcon from "@mui/icons-material/AddShoppingCart";

function CourseCard({ title, description, price, logo }: CourseCardProps) {
  return (
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
          src={logo}
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
            {title}
          </Typography>
          <Typography
            sx={{
              overflow: "hidden",
              WebkitLineClamp: 3,
              WebkitBoxOrient: "vertical",
              display: "-webkit-box",
              textOverflow: "ellipsis",
            }}>
            {description}
          </Typography>
          <Link
            href="#"
            underline="none"
            variant="body2"
            onClick={() => {
              console.info("Buy Course");
            }}>
            <Box
              sx={{
                width: 200,
                height: 30,
                padding: 1,
                fontWeight: 700,
              }}>
              <AddShoppingCartIcon
                sx={{ paddingRight: "0.5rem", paddingBottom: "0.5rem" }}
              />
              {price}
            </Box>
          </Link>
        </Container>
      </Paper>
    </Grid>
  );
}

export default CourseCard;
