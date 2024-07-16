import { Box, Container, Modal, Typography } from "@mui/material";
import axios from "axios";
import { useEffect, useState } from "react";
import { Button } from "react-bootstrap";

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

interface QuizModalProps {
  courseId: number;
  quizId: number;
  isOpen: boolean;
  handleClose: () => void;
}

export const QuizCheckModal = ({
  isOpen,
  handleClose,
  courseId,
  quizId,
}: QuizModalProps) => {
  const [questions, setQuestions] = useState<Question[]>([]);
  const [courseName, setCourseName] = useState<string>("");
  const [quizTitle, setQuizTitle] = useState<string>("");

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
  }, [quizId]);

  return (
    <Modal
      open={isOpen}
      onClose={handleClose}
      aria-labelledby="modal-modal-title"
      aria-describedby="modal-modal-description">
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
            Проверка теста "{quizTitle}"
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
              </Box>
            ))}
          </Box>
          <Button onClick={handleClose}>Закрыть</Button>
        </Container>
      </Container>
    </Modal>
  );
};
