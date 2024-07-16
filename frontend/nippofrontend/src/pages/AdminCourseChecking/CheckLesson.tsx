import Container from "@mui/material/Container";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";

interface Block {
    id: number;
    type: string;
    content: string;
  }

function LessonMainPage () {
    const navigate = useNavigate();
    const { courseId, lessonId } = useParams();
    const [lessonTitle, setLessonTitle] = useState<string>("");
    const [blocks, setBlocks] = useState<Block[]>([]);

    const handleBackToCourse = () => {
        navigate(`/check-course/${courseId}`);
    };

    const fetchLessonData = async () => {
        try {
          const response = await axios.get(`/lesson/get-by-id?id=${lessonId}`);
          if (response.status === 200) {
            setLessonTitle(response.data.title);
          } else {
            console.log("Another response status", response.status);
          }
        } catch (error) {
          console.error(error);
        }
    };

    const fetchBlocks = async () => {
        try {
          const response = await axios.get(`/block/get-blocks-by-lesson?lessonId=${lessonId}`);
          if (response.status === 200) {
            setBlocks(response.data);
          } else {
            console.log("Another response status", response.status);
          }
        } catch (error) {
          console.error(error);
        }
    };

    useEffect(() => {
        fetchLessonData();
        fetchBlocks();
      }, [lessonId]);

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
                Тест {lessonTitle}
                </Typography>
            </Box>
            <Container>
                {blocks.map((block) => (
                    <Box
                        key={block.id}
                        sx={{
                            backgroundColor: "#bfbfbf",
                            marginY: 1,
                            borderRadius: "4px",
                            display: "flex",
                            justifyContent: "space-between",
                            flexWrap: "wrap",
                        }}
                        >
                        {Number(block.type) === 0 && (
                            <Typography textAlign={"center"} sx={{ width: "100%" }}>
                            {block.content}
                            </Typography>
                        )}
                        {Number(block.type) === 1 && (
                            <Box component="img" src={block.content} alt="block content" sx={{ width: "100%" }} />
                        )}
                        {Number(block.type) === 2 && (
                            <Box component="video" controls src={block.content} sx={{ width: "100%" }} />
                        )}
                    </Box>
                ))}
            </Container>
            <Box display="flex" justifyContent="space-between">
                <Button
                    variant="contained"
                    size="small"
                    sx={{ marginY: 1 }}
                    onClick={handleBackToCourse}>
                    Вернуться к курсу
                </Button>
            </Box>
        </Container>
    );
}

export default LessonMainPage;