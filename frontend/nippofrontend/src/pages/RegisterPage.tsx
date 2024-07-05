import { Box, Button, Container, TextField, Typography } from "@mui/material";
import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

export const RegisterPage = () => {
  const [userName, setUserName] = useState<string>("Emil");
  const [userEmail, setUserEmail] = useState<string>("emilkalim@gmail.com");
  const [userPassword, setUserPassword] = useState<string>("Qwerty210");
  const navigate = useNavigate();

  const handleRegister = () => {
    fetchRegister();
  };

  const fetchRegister = async () => {
    try {
      const response = await axios.post("/auth/register", {
        userName: userName,
        Email: userEmail,
        userPassword: userPassword,
      });

      if (response.status === 200) {
        navigate("/login");
      } else {
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const handleAlreadyHaveAccount = () => {
    navigate("/login");
  };

  return (
    <Container>
      <Box textAlign={"center"}>
        <Typography>Регистрация</Typography>
      </Box>
      <Button onClick={handleAlreadyHaveAccount}>Уже есть аккаунт?</Button>
      <Container sx={{ justifyContent: "center" }}>
        <Box marginY={1}>
          <Typography>Введите ваше имя</Typography>
          <TextField
            sx={{ width: "100%" }}
            value={userName}
            onChange={(e) => {
              setUserName(e.target.value);
            }}
          />
        </Box>
        <Box marginY={1}>
          <Typography>Введите вашу почту</Typography>
          <TextField
            sx={{ width: "100%" }}
            value={userEmail}
            onChange={(e) => {
              setUserEmail(e.target.value);
            }}
          />
        </Box>
        <Box marginY={1}>
          <Typography>Введите пароль</Typography>
          <TextField
            sx={{ width: "100%" }}
            value={userPassword}
            onChange={(e) => {
              setUserPassword(e.target.value);
            }}
          />
        </Box>
      </Container>
      <Box>
        <Button onClick={handleRegister}>Регистрация</Button>
      </Box>
    </Container>
  );
};
