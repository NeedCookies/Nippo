import Container from "@mui/material/Container";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { TaskModal } from "./TaskModal";
import axios from "axios";

interface Answer {
  id: number;
  text: string;
  isCorrect: boolean;
}

interface Question {
  id: number;
  text: string;
  type: string;
  order: number;
  answers: Answer[];
}

function QuizEditPage() {
  const { courseId, quizId } = useParams<{
    courseId: string;
    quizId: string;
  }>();
  const [questions, setQuestions] = useState<Question[]>([]);
  const [quizTitle, setQuizTitle] = useState("");
  const [isTaskModalOpen, setTaskModalOpen] = useState<boolean>(false);

  const [newQuestionType, setNewQuestionType] = useState<string>("Written");

  const handleTaskModalClose = () => {
    setTaskModalOpen(false);
  };

  const handleAddQuestion = (type: string) => {
    setNewQuestionType(type);
    setTaskModalOpen(true);
    //setQuestions([...questions, newQuestion]);
  };

  const handleDeleteQuestion = (id: number) => {};
  const handleEditQuestion = (question: any) => {};

  useEffect(() => {
    /*async () => {
      try {
        const response = await axios.get(
          `https://localhost:8080/quiz/get-quiz?quizId=${quizId}`
        );
        if (response.status === 200) {
          setQuizTitle(response.data.Title);
        } else {
          console.log(response.status);
        }
      } catch (error) {
        console.error(error);
      }
    };*/
    setQuizTitle("afaf");
  });

  useEffect(() => {
    axios
      .get(`/Question/get-by-quiz?quizId=${quizId}`)
      .then(async (response) => {
        const questions = response.data;

        const questionsWithAnswers = await Promise.all(
          questions.map(async (question: Question) => {
            const answerResponse = await axios.get(
              `https://localhost:8080/answer/get-by-question?questionId=${question.id}`
            );
            return { ...question, answers: answerResponse.data };
          })
        );

        questionsWithAnswers.sort((a, b) => a.order - b.order);

        setQuestions(questionsWithAnswers);
      })
      .catch((error) => {
        console.error(error);
      });
  }, [isTaskModalOpen, quizId]);

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
          Создание теста
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
            flex: 2,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
          }}>
          <Typography sx={{ fontStyle: "italic" }}>
            Здесь будет название курса
          </Typography>
          <Typography sx={{ fontStyle: "italic" }}>
            Тест: {quizTitle}
          </Typography>
        </Box>
        <Box
          sx={{
            flex: 6,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
          }}>
          {questions.map((question) => (
            <Box
              key={question.order}
              sx={{
                backgroundColor: "#bfbfbf",
                marginY: 1,
                borderRadius: "4px",
                display: "flex",
                justifyContent: "space-between",
                flexWrap: "wrap",
              }}>
              <Typography
                key={question.id}
                textAlign={"start"}
                sx={{ width: "50%" }}>
                {question.order}. {question.text}
              </Typography>
              <Typography textAlign={"end"} sx={{ width: "50%" }}>
                тип: {question.type}
              </Typography>
              <Box sx={{ width: "100%" }}>
                {question.answers.map((answer) => (
                  <Typography key={answer.id} sx={{ marginLeft: "1rem" }}>
                    {answer.text}
                  </Typography>
                ))}
              </Box>
              <Button
                variant="contained"
                size="small"
                sx={{ marginY: 1 }}
                onClick={() => handleDeleteQuestion(question.id)}>
                Удалить
              </Button>
              <Button
                variant="contained"
                size="small"
                sx={{ marginY: 1 }}
                onClick={() => handleEditQuestion(question)}>
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
            onClick={() => handleAddQuestion("Written")}>
            Добавить вопрос с текстовым ответом
          </Button>
          <Button
            variant="contained"
            size="large"
            sx={{ margin: 2 }}
            onClick={() => handleAddQuestion("SingleChoice")}>
            Добавить вопрос с одним вариантом ответа
          </Button>
          <Button
            variant="contained"
            size="large"
            sx={{ margin: 2 }}
            onClick={() => handleAddQuestion("MultipleChoice")}>
            Добавить вопрос с несколькими вариантами ответа
          </Button>
        </Box>
        <TaskModal
          isOpen={isTaskModalOpen}
          handleClose={handleTaskModalClose}
          type={newQuestionType}
          quizId={Number(quizId)}></TaskModal>
      </Container>
    </Container>
  );
}

export default QuizEditPage;
