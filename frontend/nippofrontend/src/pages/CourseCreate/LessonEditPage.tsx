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

function LessonEditPage() {
  const { courseId, lessonId } = useParams<{ courseId: string; lessonId: string }>();

  const [lessonTitle, setLessonTitle] = useState<string>("");
  const [isEditingTitle, setEditingTitle] = useState<boolean>(false);
  const [blocks, setBlocks] = useState<Block[]>([]);
  const [isAddingBlock, setAddingBlock] = useState<boolean>(false);
  const [newBlockType, setNewBlockType] = useState<string>("");
  const [newBlockContent, setNewBlockContent] = useState<string | File>("");

  const navigate = useNavigate();

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
        console.log("we get all blocks again!");
      } else {
        console.log("Another response status", response.status);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const handleSaveTitleChanges = async () => {
    try {
      const response = await axios.post(`/lesson/update?lessonId=${lessonId}&title=${lessonTitle}`);
      if (response.status === 200) {
        setEditingTitle(false);
      } else {
        console.log("Another response status", response.status);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const handleChangeBlockOrderUp = async (blockId: number) => {
    try {
      const response = await axios.post(`/block/move-up?id=${blockId}`);

      if (response.status === 200) {
        setAddingBlock(false);
        setNewBlockType("");
        setNewBlockContent("");
        fetchBlocks();
      } else {
        console.log("Another response status", response.status);
      }
    } catch (error) {
      console.error(error);
    }
  }

  const handleChangeBlockOrderDown = async (blockId: number) => {
    try {
      const response = await axios.post(`/block/move-down?id=${blockId}`);

      if (response.status === 200) {
        setAddingBlock(false);
        setNewBlockType("");
        setNewBlockContent("");
        fetchBlocks();
      } else {
        console.log("Another response status", response.status);
      }
    } catch (error) {
      console.error(error);
    }
  }

  const handleAddBlock = async () => {
    try {
      const formData = new FormData();
      formData.append("lessonId", String(lessonId));

      if (newBlockType === "Text") {
        formData.append("type", String(0));
        formData.append("content", newBlockContent as string);
      } else {
        if (newBlockType === "Image") {
          formData.append("type", String(1));
        } else {
          formData.append("type", String(2));
        }

        formData.append("media", newBlockContent as File);
      }

      const response = await axios.post(`/block/create-block`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });

      if (response.status === 200) {
        setAddingBlock(false);
        setNewBlockType("");
        setNewBlockContent("");
        fetchBlocks();
      } else {
        console.log("Another response status", response.status);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const handleDeleteBlock = async (blockId: any) => {
    try {
      const response = await axios.post(`/block/delete-block?id=${blockId}`);
      if (response.status === 200) {
        fetchBlocks();
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
        }}
      >
        <Typography variant="h4" sx={{ fontWeight: "500" }}>
          Редактирование урока
        </Typography>
      </Box>
      <Container
        sx={{
          bgcolor: "#707070",
          display: "flex",
          justifyContent: "space-between",
          borderRadius: "10px",
        }}
      >
        <Box
          sx={{
            flex: 2,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
          }}
        >
          {isEditingTitle ? (
            <Box>
              <TextField
                value={lessonTitle}
                onChange={(e) => setLessonTitle(e.target.value)}
                sx={{ width: "100%" }}
              />
              <Button onClick={handleSaveTitleChanges}>Сохранить изменения</Button>
              <Button onClick={() => setEditingTitle(false)}>Отклонить изменения</Button>
            </Box>
          ) : (
            <Box>
              <Typography sx={{ fontStyle: "italic" }}>
                Название урока: {lessonTitle}
              </Typography>
              <Button onClick={() => setEditingTitle(true)}>Редактировать</Button>
            </Box>
          )}
        </Box>
        <Box
          sx={{
            flex: 6,
            backgroundColor: "#dedede",
            margin: 1,
            borderRadius: "6px",
          }}
        >
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
              <Button
                variant="contained"
                size="small"
                sx={{ marginY: 3 }}
                onClick={() => handleDeleteBlock(block.id)}
              >
                Удалить
              </Button>

              <Button
                variant="contained"
                size="small"
                sx={{ marginY: 3 }}
                onClick={() => handleChangeBlockOrderUp(block.id)}
              >
                 ⬆️
              </Button>
              <Button
                variant="contained"
                size="small"
                sx={{ marginY: 3 }}
                onClick={() => handleChangeBlockOrderDown(block.id)}
              >
                  ⬇️
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
          }}
        >
          {isAddingBlock ? (
            <Box>
              <Select
                value={newBlockType}
                onChange={(e) => setNewBlockType(e.target.value)}
                sx={{ width: "100%", marginBottom: "1rem" }}
              >
                <MenuItem value="Text">Текст</MenuItem>
                <MenuItem value="Video">Видео</MenuItem>
                <MenuItem value="Image">Картинка</MenuItem>
              </Select>
              {newBlockType === "Text" && (
                <TextField
                  multiline
                  rows={4}
                  value={newBlockContent as string}
                  onChange={(e) => setNewBlockContent(e.target.value)}
                  sx={{ width: "100%", marginBottom: "1rem" }}
                />
              )}
              {(newBlockType === "Video" || newBlockType === "Image") && (
                <TextField
                  type="file"
                  onChange={(e) =>
                    setNewBlockContent(e.target.files ? e.target.files[0] : "")
                  }
                  sx={{ width: "100%", marginBottom: "1rem" }}
                />
              )}
              <Button onClick={handleAddBlock}>Добавить блок</Button>
              <Button onClick={() => setAddingBlock(false)}>Отмена</Button>
            </Box>
          ) : (
            <Button onClick={() => setAddingBlock(true)}>+</Button>
          )}
        </Box>
      </Container>
      <Box justifyContent={"end"} width={"100%"} color={"green"}>
        <Button variant="contained" onClick={() => navigate(`/course/${courseId}/create`)}>
          Готово
        </Button>
      </Box>
    </Container>
  );
}

export default LessonEditPage;
