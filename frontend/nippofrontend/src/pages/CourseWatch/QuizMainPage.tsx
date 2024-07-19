import { RadioButtonChecked, RadioButtonUnchecked } from "@mui/icons-material";
import {
  Box,
  Checkbox,
  Typography,
  Button,
  Container,
  TextField,
} from "@mui/material";
import axios from "axios";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

interface Answer {
  id: number;
  text: string;
  isCorrect: boolean;
}

interface Question {
  id: number;
  text: string;
  type: number;
  order: number;
  answers: Answer[];
}

interface QuizResult {
  id: number;
  quizId: number;
  score: number;
  attempt: number;
}

export const QuizMainPage = () => {
  const navigate = useNavigate();
  const { courseId, quizId } = useParams();
  const [quizTitle, setQuizTitle] = useState<string>("");
  const [questions, setQuestions] = useState<Question[]>();
  const [quizResult, setQuizResult] = useState<QuizResult>();

  const handleBackToCourse = () => {
    navigate(`/my-course/${courseId}`);
  };

  const handleChecksChange = (questionId: number, answerId: number) => {
    setQuestions((prevQuestions) =>
      prevQuestions?.map((question) =>
        question.id === questionId
          ? {
              ...question,
              answers: question.answers.map((answer) =>
                answer.id === answerId
                  ? { ...answer, isCorrect: !answer.isCorrect }
                  : answer
              ),
            }
          : question
      )
    );
  };

  const handleOneRightAnswer = (questionId: number, answerId: number) => {
    setQuestions((prevQuestions) =>
      prevQuestions?.map((question) =>
        question.id === questionId
          ? {
              ...question,
              answers: question.answers.map((answer) =>
                answer.id === answerId
                  ? { ...answer, isCorrect: true }
                  : { ...answer, isCorrect: false }
              ),
            }
          : question
      )
    );
  };

  const handleSaveAnswer = (questionId: number, questionType: number) => {
    const question = questions?.find((q) => q.id === questionId);
    if (!question) return;

    let answerText: string;

    if (questionType === 2) {
      // Для вопросов типа "written"
      answerText = question.answers[0].text;
    } else {
      // Для других типов вопросов
      answerText = question.answers
        .filter((answer) => answer.isCorrect)
        .map((answer) => answer.id.toString())
        .join(" ");
    }

    const requestData = {
      questionId: questionId,
      text: answerText,
    };

    axios
      .post("/useranswer/save", requestData)
      .then((response) => {
        console.log("Answer saved successfully:", response.data);
      })
      .catch((error) => {
        console.error("Error saving answer:", error);
      });
  };

  async function saveQuizResults(quizId: number) {
    try {
      const response = await axios.post(`/quizresult/save`, { quizId: quizId });

      if (response.status === 200) {
        setQuizResult(response.data);
        console.log(quizResult?.score);
        console.log(quizResult?.attempt);
      } else {
        console.log("Another response status");
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  }

  const handleSaveQuizResult = async () => {
    if (!questions) return;

    try {
      const savePromises = questions.map((question) =>
        handleSaveAnswer(question.id, question.type)
      );

      await Promise.all(savePromises);

      saveQuizResults(Number(quizId));
    } catch (error) {
      console.error("Error saving quiz results:", error);
    }
  };

  const handleAnswerInput = (questionId: number, ansText: string) => {
    setQuestions((prevQuestions) =>
      prevQuestions?.map((q) =>
        q.id === questionId
          ? { ...q, answers: [{ ...q.answers[0], text: ansText }] }
          : q
      )
    );
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [quizTitleResponse, quizResultResponse, questionsResponse] =
          await Promise.all([
            axios.get(`/quiz/get-quiz?quizId=${quizId}`),
            axios.get(`/quizresult/get-by-quiz?quizId=${quizId}`),
            axios.get(`/question/get-by-quiz?quizId=${quizId}`),
          ]);

        if (quizTitleResponse.status === 200) {
          setQuizTitle(quizTitleResponse.data.title);
        }

        if (quizResultResponse.status === 200) {
          setQuizResult(quizResultResponse.data);
        }

        const questions = questionsResponse.data;

        const questionsWithAnswers = await Promise.all(
          questions.map(async (question: Question) => {
            const answerResponse = await axios.get(
              `/answer/get-by-question?questionId=${question.id}`
            );
            const answers = answerResponse.data.map((answer: Answer) => {
              const text = question.type === 2 ? "Ваш ответ" : answer.text;
              return {
                ...answer,
                text,
                isCorrect: false,
              };
            });
            return { ...question, answers };
          })
        );

        questionsWithAnswers.sort((a, b) => a.order - b.order);

        setQuestions(questionsWithAnswers);
      } catch (error) {
        console.error(error);
      }
    };

    fetchData();
  }, []);

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
          Тест {quizTitle}
        </Typography>
      </Box>
      <Container>
        {questions?.map((question) => (
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
              {question.type === 2 && (
                <>
                  <Typography>Введите ваш ответ</Typography>
                  <TextField
                    id="written-answer"
                    label="Ответ"
                    variant="filled"
                    sx={{ width: "100%", marginTop: 2 }}
                    value={question.answers[0].text}
                    onChange={(e) =>
                      handleAnswerInput(question.id, e.target.value)
                    }>
                    Ответ
                  </TextField>
                </>
              )}
              {question.type === 1 &&
                question.answers.map((answer) => (
                  <Box
                    key={answer.id}
                    sx={{ display: "flex", alignItems: "center" }}>
                    <Checkbox
                      checked={answer.isCorrect}
                      onChange={() =>
                        handleChecksChange(question.id, answer.id)
                      }
                    />
                    <Typography
                      key={answer.id}
                      sx={{
                        marginLeft: "1rem",
                        display: "flex",
                        alignItems: "space-between",
                      }}>
                      {answer.text}
                    </Typography>
                  </Box>
                ))}
              {question.type === 0 &&
                question.answers.map((answer) => (
                  <Box
                    key={answer.id}
                    sx={{ display: "flex", alignItems: "center" }}>
                    <Checkbox
                      icon={<RadioButtonUnchecked />}
                      checkedIcon={<RadioButtonChecked />}
                      checked={answer.isCorrect}
                      onChange={(_) => {
                        handleChecksChange(question.id, answer.id),
                          handleOneRightAnswer(question.id, answer.id);
                      }}
                    />
                    <Typography>{answer.text}</Typography>
                  </Box>
                ))}
            </Box>
            <Button
              variant="contained"
              size="small"
              sx={{ marginY: 1 }}
              onClick={() => handleSaveAnswer(question.id, question.type)}>
              Сохранить
            </Button>
          </Box>
        ))}
        <Box display="flex" justifyContent="space-between">
          <Button
            variant="contained"
            size="small"
            sx={{ marginY: 1 }}
            onClick={handleBackToCourse}>
            Вернуться к курсу
          </Button>
          {quizResult && (
            <Typography>
              Ваш последний результат: {quizResult.score}/10
            </Typography>
          )}
          <Button
            variant="contained"
            size="large"
            sx={{ marginY: 1 }}
            onClick={handleSaveQuizResult}>
            Отправить
          </Button>
        </Box>
      </Container>
    </Container>
  );
};
