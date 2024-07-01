import Container from "@mui/material/Container";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import Modal from "@mui/material/Modal";
import axios from "axios";
import Button from "@mui/material/Button";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

interface PointModalProps {
  courseId: number;
  isOpen: boolean;
  handleClose: () => void;
}

export const QuizModal = ({
  isOpen,
  handleClose,
  courseId,
}: PointModalProps) => {
  var quizId = -1;
  const navigate = useNavigate();

  const [title, setTitle] = useState("");

  const handleAddQuiz = async () => {
    /*try {
      const response = await axios.post("https://localhost:8080/quiz/create", {
        courseId: courseId,
        Title: title,
      });
      if (response.status === 200) {
        quizId = response.data.id;
        navigate(`/course/${courseId}/quiz/${quizId}/edit`);
      }
    } catch (error) {
      console.error("Can't create quiz ", error);
    }*/
    quizId = 1;
    navigate(`/course/${courseId}/quiz/${quizId}/edit`);
  };

  return (
    <Modal
      open={isOpen}
      onClose={handleClose}
      aria-labelledby="modal-modal-title"
      aria-describedby="modal-modal-description">
      <Container>
        <Box
          sx={{
            flex: 4,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
          }}>
          <Typography>Заголовок теста:</Typography>
          <TextField
            id="filled-basic"
            label="Заголовок теста"
            variant="filled"
            sx={{ width: "100%" }}
            value={title}
            onChange={(e) => setTitle(e.target.value)}
          />
        </Box>
        <Box
          sx={{
            flex: 2,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
            display: "flex",
            justifyContent: "space-between",
          }}>
          <Button variant="contained" sx={{ margin: 2 }} onClick={handleClose}>
            Отмена
          </Button>
          <Button
            variant="contained"
            sx={{ margin: 2 }}
            onClick={handleAddQuiz}>
            Добавить
          </Button>
        </Box>
      </Container>
    </Modal>
  );
};
