import Container from "@mui/material/Container";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import TextField from "@mui/material/TextField";
import Select from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";
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

    const fetchLessonData = async () => {
        try {
          const response = await axios.get(`/lesson/get-lesson?lessonId=${lessonId}`);
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
          const response = await axios.get(`/block/get-by-lesson?lessonId=${lessonId}`);
          if (response.status === 200) {
            setBlocks(response.data);
          } else {
            console.log("Another response status", response.status);
          }
        } catch (error) {
          console.error(error);
        }
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
                        }}>
                        <Typography
                            key={block.id}
                            textAlign={"start"}
                            sx={{ width: "50%" }}>
                            {block.id}. {block.type}
                        </Typography>
                        <Typography textAlign={"end"} sx={{ width: "50%" }}>
                            {block.content}
                        </Typography>
                    </Box>
                ))}
            </Container>
        </Container>
    );
}

export default LessonMainPage;