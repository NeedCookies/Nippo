import Container from "@mui/material/Container";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { TaskCreateModal } from "./TaskCreateModal";
import axios from "axios";
import { TaskEditModal } from "./TaskEditModal";

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
  const [courseName, setCourseName] = useState<string>("");
  const [quizTitle, setQuizTitle] = useState<string>("");

  const [isTaskModalOpen, setTaskModalOpen] = useState<boolean>(false);
  const [isEditTaskModal, setEditTaskModal] = useState<boolean>(false);

  const [newQuestionType, setNewQuestionType] = useState<string>("Written");
  const [editQuestionId, setEditQuestionId] = useState<number>(-1);

  const navigate = useNavigate();

  const handleTaskModalClose = () => {
    setTaskModalOpen(false);
    setEditTaskModal(false);
  };

  const handleAddQuestion = (type: string) => {
    setNewQuestionType(type);
    setTaskModalOpen(true);
    //setQuestions([...questions, newQuestion]);
  };

  const handleEditQuestion = (questionId: number) => {
    setEditQuestionId(questionId);
    setEditTaskModal(true);
  };

  const handleDeleteQuestion = async (questionId: number) => {
    try {
      const response = await axios.delete(
        `/question/delete?qustionId=${questionId}`
      );

      if (response.status === 200) {
        fetchQuestions();
      } else {
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const handleQuizDone = () => {
    navigate(`/course/${courseId}/create`);
  };

  const fetchQuestions = async () => {
    try {
      const response = await axios.get(
        `/Question/get-by-quiz?quizId=${quizId}`
      );
      const questions = response.data;

      const questionsWithAnswers = await Promise.all(
        questions.map(async (question: Question) => {
          const answerResponse = await axios.get(
            `/answer/get-by-question?questionId=${question.id}`
          );
          return { ...question, answers: answerResponse.data };
        })
      );

      questionsWithAnswers.sort((a, b) => a.order - b.order);

      setQuestions(questionsWithAnswers);
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    const getCourseName = async () => {
      try {
        const response = await axios.get(`/course/get-course?id=${courseId}`);
        if (response.status === 200) {
          setCourseName(response.data.title);
        } else {
          console.log("Another response status");
          console.log(response.status);
        }
      } catch (error) {
        console.error(error);
      }
    };

    const getQuizTitle = async () => {
      try {
        const response = await axios.get(`/quiz/get-quiz?quizId=${quizId}`);
        if (response.status === 200) {
          setQuizTitle(response.data.title);
        } else {
          console.log("Another response status");
          console.log(response.status);
        }
      } catch (error) {
        console.error(error);
      }
    };

    getCourseName();
    getQuizTitle();
  }, []);

  useEffect(() => {
    fetchQuestions();
  }, [isTaskModalOpen, quizId]);

  useEffect(() => {
    axios
      .get(`/Question/get-by-quiz?quizId=${quizId}`)
      .then(async (response) => {
        const questions = response.data;

        const questionsWithAnswers = await Promise.all(
          questions.map(async (question: Question) => {
            const answerResponse = await axios.get(
              `/answer/get-by-question?questionId=${question.id}`
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
            Курс: {courseName}
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
                  <Typography
                    key={answer.id}
                    sx={{
                      marginLeft: "1rem",
                      display: "flex",
                      alignItems: "space-between",
                    }}>
                    {answer.text}
                    {"   "}
                    {answer.isCorrect && (
                      <Typography sx={{ color: "green", fontWeight: 900 }}>
                        +
                      </Typography>
                    )}
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
                onClick={() => handleEditQuestion(question.id)}>
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
        <TaskCreateModal
          isOpen={isTaskModalOpen}
          handleClose={handleTaskModalClose}
          type={newQuestionType}
          quizId={Number(quizId)}></TaskCreateModal>
        <TaskEditModal
          isOpen={isEditTaskModal}
          handleClose={handleTaskModalClose}
          questionId={editQuestionId}></TaskEditModal>
      </Container>
      <Box justifyContent={"end"} width={"100%"} color={"green"}>
        <Button variant="contained" onClick={handleQuizDone}>
          Готово
        </Button>
      </Box>
    </Container>
  );
}

export default QuizEditPage;
