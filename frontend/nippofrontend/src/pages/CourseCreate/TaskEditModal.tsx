import { RadioButtonChecked, RadioButtonUnchecked } from "@mui/icons-material";
import {
  Box,
  Checkbox,
  TextField,
  Typography,
  Button,
  Container,
  Modal,
} from "@mui/material";
import axios from "axios";
import { useEffect, useState } from "react";

interface TaskProps {
  isOpen: boolean;
  handleClose: () => void;
  questionId: number;
}

interface Answer {
  id: number;
  isCorrect: boolean;
  text: string;
}

interface Task {
  id: number;
  question: string;
  type: string;
  answers: Answer[];
}

export const TaskEditModal = ({
  isOpen,
  handleClose,
  questionId,
}: TaskProps) => {
  const [task, setTask] = useState<Task>({
    id: -1,
    question: "Can't get question from DB",
    type: "Written",
    answers: [] as Answer[],
  });

  const fetchQuestion = async (questionId: number) => {
    try {
      const response = await axios.get(
        `/question/get-by-id?questionId=${questionId}`
      );

      if (response.status === 200) {
        return response.data;
      } else {
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const fetchAnswers = async (questionId: number) => {
    try {
      const response = await axios.get(
        `answer/get-by-question?questionId=${questionId}`
      );

      if (response.status === 200) {
        return response.data;
      } else {
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const fetchQuestionWithAnswers = async (questionId: number) => {
    const [questionData, answersData] = await Promise.all([
      await fetchQuestion(questionId),
      await fetchAnswers(questionId),
    ]);

    if (questionData) {
      setTask({ ...questionData, answers: answersData });
    }
  };

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

  const handleEditTask = async () => {
    try {
      const response = await axios.post(
        `/queston/update?questionId=${questionId}`,
        {
          QuizId: task.id,
          Text: task.question,
          Type: task.type,
        }
      );
      if (response.status === 200) {
        fetchQuestionWithAnswers(questionId);
      }
    } catch (error) {
      console.error(error);
    }

    task.answers.map(async (ans) => {
      try {
        const response = await axios.post("/answer/update", {
          QuestionId: questionId,
          Text: ans.text,
          IsCOrrect: ans.isCorrect,
        });

        if (response.status === 200) {
        }
      } catch (error) {
        console.error(error);
      }
    });
  };

  useEffect(() => {
    fetchQuestionWithAnswers(questionId);
  }, []);

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
            value={task?.question}
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
          {task.type === "Written" && (
            <TextField
              id="written-answer"
              label="Ответ"
              variant="filled"
              sx={{ width: "100%", marginTop: 2 }}
            />
          )}
          {task.type === "MultipleChoice" && (
            <>
              {task.answers.map((ans) => (
                <Box
                  key={ans.id}
                  sx={{ display: "flex", alignItems: "center" }}>
                  <Checkbox
                    checked={ans.isCorrect}
                    onChange={(e) => handleChecksChange(ans.id)}
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
          {task.type === "SingleChoice" && (
            <>
              {task.answers.map((ans) => (
                <Box
                  key={ans.id}
                  sx={{ display: "flex", alignItems: "center" }}>
                  <Checkbox
                    icon={<RadioButtonUnchecked />}
                    checkedIcon={<RadioButtonChecked />}
                    checked={ans.isCorrect}
                    onChange={(e) => {
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
                id: -1,
                question: "",
                answers: [] as Answer[],
                type: "Written",
              });
            }}>
            Отмена
          </Button>
          <Button
            variant="contained"
            size="large"
            sx={{ margin: 2 }}
            onClick={() => handleEditTask()}>
            Сохранить
          </Button>
        </Box>
      </Container>
    </Modal>
  );
};
