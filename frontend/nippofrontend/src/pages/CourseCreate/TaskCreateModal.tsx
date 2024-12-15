import { RadioButtonChecked, RadioButtonUnchecked } from "@mui/icons-material";
import {
  TextField,
  Typography,
  Button,
  Container,
  Checkbox,
} from "@mui/material";
import Box from "@mui/material/Box";
import Modal from "@mui/material/Modal";
import { useState } from "react";
import axios from "axios";

interface TaskProps {
  type: string;
  isOpen: boolean;
  handleClose: () => void;
  quizId: number;
}

interface Answer {
  id: number;
  isCorrect: boolean;
  text: string;
}

export const TaskCreateModal = ({
  type,
  isOpen,
  handleClose,
  quizId,
}: TaskProps) => {
  const [task, setTask] = useState({
    question: "",
    answers: [{ id: 1, text: "Ответ", isCorrect: true }] as Answer[],
  });

  const handleAnswerChange = (id: number, value: any) => {
    setTask((prev) => ({
      ...prev,
      answers: prev.answers.map((ans) =>
        ans.id === id ? { ...ans, text: value } : ans
      ),
    }));
  };

  const handleChecksChange = (id: number) => {
    setTask((prev) => ({
      ...prev,
      answers: prev.answers.map((ans) =>
        ans.id === id ? { ...ans, isCorrect: !ans.isCorrect } : ans
      ),
    }));
  };

  const handleOneRightAnswer = (id: number) => {
    setTask((prev) => ({
      ...prev,
      answers: prev.answers.map((ans) =>
        ans.id === id
          ? { ...ans, isCorrect: true }
          : { ...ans, isCorrect: false }
      ),
    }));
  };

  const handleAddAnswer = () => {
    setTask((prev) => ({
      ...prev,
      answers: [
        ...prev.answers,
        { id: Date.now(), text: "", isCorrect: false },
      ],
    }));
  };

  const handleAddTask = async () => {
    try {
      const response = await axios.post("/question/create", {
        quizId: quizId,
        text: task.question,
        type: type,
      });
      if (response.status === 200) {
        const Id = response.data.id;
        for (const ans of task.answers) {
          try {
            const response = await axios.post("/answer/create", {
              questionId: Id,
              text: ans.text,
              isCorrect: ans.isCorrect,
            });

            if (response.status === 200) {
            }
          } catch (error) {
            console.error(error);
          }
        }
        setTask({
          question: "",
          answers: [{ id: 1, text: "Ответ", isCorrect: true }] as Answer[],
        });
        handleClose();
      } else {
        console.log("Another response status");
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
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
          <Typography marginTop={1}>Вопрос:</Typography>
          <TextField
            id="filled-basic"
            label="Введите вопрос"
            variant="filled"
            sx={{ width: "100%" }}
            value={task.question}
            onChange={(e) => setTask({ ...task, question: e.target.value })}
          />
        </Box>
        <Box
          sx={{
            flex: 2,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
          }}>
          <Typography marginTop={1}>Варианты ответа:</Typography>
          {type === "Written" && (
            <>
              {task.answers.map((ans) => (
                <TextField
                  id="written-answer"
                  variant="filled"
                  sx={{ width: "100%", marginTop: 2 }}
                  value={ans.text}
                  onChange={(e) => handleAnswerChange(ans.id, e.target.value)}
                />
              ))}
            </>
          )}
          {type === "MultipleChoice" && (
            <>
              {task.answers.map((ans) => (
                <Box
                  key={ans.id}
                  sx={{ display: "flex", alignItems: "center" }}>
                  <Checkbox
                    checked={ans.isCorrect}
                    onChange={(_) => handleChecksChange(ans.id)}
                  />
                  <TextField
                    value={ans.text}
                    onChange={(e) => handleAnswerChange(ans.id, e.target.value)}
                    variant="filled"
                    sx={{ width: "100%" }}
                  />
                </Box>
              ))}
              <Button
                variant="outlined"
                onClick={handleAddAnswer}
                sx={{ marginTop: 2 }}>
                +
              </Button>
            </>
          )}
          {type === "SingleChoice" && (
            <>
              {task.answers.map((ans) => (
                <Box
                  key={ans.id}
                  sx={{ display: "flex", alignItems: "center" }}>
                  <Checkbox
                    icon={<RadioButtonUnchecked />}
                    checkedIcon={<RadioButtonChecked />}
                    checked={ans.isCorrect}
                    onChange={(_) => {
                      handleChecksChange(ans.id), handleOneRightAnswer(ans.id);
                    }}
                  />
                  <TextField
                    value={ans.text}
                    onChange={(e) => handleAnswerChange(ans.id, e.target.value)}
                    variant="filled"
                    sx={{ width: "100%" }}
                  />
                </Box>
              ))}
              <Button
                variant="outlined"
                onClick={handleAddAnswer}
                sx={{ marginTop: 2 }}>
                +
              </Button>
            </>
          )}
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
          <Button
            variant="contained"
            size="large"
            sx={{ margin: 2 }}
            onClick={() => {
              handleClose();
              setTask({
                question: "",
                answers: [
                  { id: 1, text: "Ответ", isCorrect: true },
                ] as Answer[],
              });
            }}>
            Отмена
          </Button>
          <Button
            variant="contained"
            size="large"
            sx={{ margin: 2 }}
            onClick={() => handleAddTask()}>
            Добавить
          </Button>
        </Box>
      </Container>
    </Modal>
  );
};
