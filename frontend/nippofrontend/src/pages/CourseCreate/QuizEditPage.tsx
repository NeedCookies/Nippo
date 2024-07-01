import Container from "@mui/material/Container";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";

function QuizEditPage() {
  const { courseId, quizId } = useParams<{
    courseId: string;
    quizId: string;
  }>();
  const [questions, setQuestions] = useState([]);
  const [quizTitle, setQuizTitle] = useState("");

  const handleAddQuestion = (typeNum: number) => {
    const newQuestion = {
      id: questions.length == 0 ? 1 : questions.length + 1,
      title: "Unnamed question",
      answer: "No answer",
      type: typeNum,
    };
    setQuestions([...questions, newQuestion]);
  };

  const handleDeleteQuestion = (id: number) => {};
  const handleEditQuestion = (question: any) => {};

  useEffect(() => {
    /*async () => {
      try {
        const response = await axios.get(
          "https://localhost:8080/quiz/get-quiz"
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
          {questions.map((question: any) => (
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
                key={question.id}
                textAlign={"start"}
                sx={{ width: "50%" }}>
                {question.id}. {question.name}
              </Typography>
              <Typography textAlign={"end"} sx={{ width: "50%" }}>
                тип: {question.type}
              </Typography>
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
            onClick={() => handleAddQuestion(1)}>
            Добавить вопрос с текстовым ответом
          </Button>
          <Button
            variant="contained"
            size="large"
            sx={{ margin: 2 }}
            onClick={() => handleAddQuestion(2)}>
            Добавить вопрос с одним вариантом ответа
          </Button>
          <Button
            variant="contained"
            size="large"
            sx={{ margin: 2 }}
            onClick={() => handleAddQuestion(3)}>
            Добавить вопрос с несколькими вариантами ответа
          </Button>
        </Box>
      </Container>
    </Container>
  );
}

export default QuizEditPage;
