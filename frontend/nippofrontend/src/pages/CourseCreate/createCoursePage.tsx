import Container from "@mui/material/Container";
import Box from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import { useState, useEffect } from "react";
import Input from "@mui/material/Input";
import Button from "@mui/material/Button";
import { useNavigate } from "react-router-dom";
import { QuizModal } from "./QuizModal";
import axios from "axios";

function createCourse() {
  const [logo, setLogo] = useState({
    preview: "",
    raw: "",
  });

  var courseId = -1;
  const [modules, setModules] = useState([]);
  const [isQuizModalOpen, setQuizModalOpen] = useState<boolean>(false);
  const [courseData, setCourseData] = useState({
    Title: "",
    Description: "",
    Price: "",
  });

  const navigate = useNavigate();
  const haldleCloseModal = () => setQuizModalOpen(false);

  const handleLogoButton = (e: any) => {
    if (e.target.files.length) {
      setLogo({
        preview: URL.createObjectURL(e.target.files[0]),
        raw: e.target.files[0],
      });
    }
  };

  const addModules = async (type: any) => {
    const formData = new FormData();
    formData.append("Title", courseData.Title);
    formData.append("Description", courseData.Description);
    formData.append("Price", courseData.Price);
    formData.append("ImgPath", logo.raw);

    /*try {
      const response = await axios.post(
        "https://localhost:8080/course/create-course",
        formData
      );
      if (response.status === 200) {
        courseId = response.data.id;
        if (type === "lesson") {
        } else {
          setQuizModalOpen(true);
        }
      } else {
        console.log(response.status);
        console.log(response);
      }
    } catch (error) {
      console.error("Can't create course", error);
    }*/
    if (type === "lesson") {
    } else {
      setQuizModalOpen(true);
    }
  };

  const handleDeleteModule = (id: number) => {
    const clearModules = modules.filter((module) => module.id !== id);
    setModules(clearModules);
  };

  const handleEditModule = (module: any) => {
    const moduleType = module.type === "lesson" ? "lesson" : "quiz";
    navigate("/edit-" + moduleType + "/id=" + module.id);
  };

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
          Создание курса
        </Typography>
      </Box>
      <Container
        sx={{
          bgcolor: "#707070",
          display: "flex",
          justifyContent: "space-between",
          borderRadius: "10px",
        }}>
        <Box
          sx={{
            flex: 4,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
          }}>
          <Typography>Название:</Typography>
          <TextField
            id="filled-basic"
            label="Название курса"
            variant="filled"
            sx={{ width: "100%" }}
            value={courseData.Title}
            onChange={(e) =>
              setCourseData({ ...courseData, Title: e.target.value })
            }
          />
          <Typography>Описание:</Typography>
          <TextField
            id="filled-basic"
            label="Описание курса"
            variant="filled"
            sx={{ width: "100%" }}
            value={courseData.Description}
            onChange={(e) =>
              setCourseData({ ...courseData, Description: e.target.value })
            }
            multiline
          />
          <Typography>Цена курса:</Typography>
          <TextField
            id="filled-basic"
            label="Цена курса должна быть числом"
            variant="filled"
            sx={{ width: "100%" }}
            type="number"
            inputProps={{
              pattern: "^[0-9]+(.[0-9]+)?$",
              title: "Введите положительное число",
              min: 0,
            }}
            value={courseData.Price}
            onChange={(e) =>
              setCourseData({ ...courseData, Price: e.target.value })
            }
            multiline
          />
          <Typography>Логотип курса:</Typography>
          <Input
            type="file"
            name="image"
            onChange={handleLogoButton}
            sx={{ width: "100%" }}
          />
          {logo.preview && (
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
                src={logo.preview}
                alt="Preview"
                style={{ maxWidth: "100%", maxHeight: "200px" }}
              />
            </Box>
          )}
        </Box>
        <Box
          sx={{
            flex: 6,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
          }}>
          {modules.map((module: any) => (
            <Box
              sx={{
                backgroundColor: "#bfbfbf",
                marginY: 1,
                borderRadius: "4px",
                display: "flex",
                justifyContent: "space-between",
                flexWrap: "wrap",
              }}>
              <Typography
                key={module.id}
                textAlign={"start"}
                sx={{ width: "50%" }}>
                {module.id}. {module.name}
              </Typography>
              <Typography textAlign={"end"} sx={{ width: "50%" }}>
                тип: {module.type}
              </Typography>
              <Button
                variant="contained"
                size="small"
                sx={{ marginY: 1 }}
                onClick={() => handleDeleteModule(module.id)}>
                Удалить
              </Button>
              <Button
                variant="contained"
                size="small"
                sx={{ marginY: 1 }}
                onClick={() => handleEditModule(module)}>
                Редактировать
              </Button>
            </Box>
          ))}
        </Box>
        <Box
          sx={{
            flex: 2,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
            display: "flex",
            flexDirection: "column",
            justifyContent: "start",
          }}>
          <Button
            variant="contained"
            size="large"
            sx={{ margin: 2 }}
            onClick={() => addModules("lesson")}>
            Создать урок
          </Button>
          <Button
            variant="contained"
            size="large"
            sx={{ margin: 2 }}
            onClick={() => addModules("test")}>
            Создать тест
          </Button>
          <QuizModal
            courseId={courseId > 0 ? courseId : 1}
            isOpen={isQuizModalOpen}
            handleClose={haldleCloseModal}></QuizModal>
        </Box>
      </Container>
    </Container>
  );
}

export default createCourse;
