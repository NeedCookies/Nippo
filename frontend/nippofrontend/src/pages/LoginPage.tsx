import { Box, Container, TextField, Typography } from "@mui/material";
import axios from "axios";
import { useState } from "react";
import { Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

export const LoginPage = () => {
  const [userName, setUserName] = useState<string>("");
  const [userPassword, setUserPassword] = useState<string>("");

  const navigate = useNavigate();
  const handleNoAccount = () => {
    navigate("/register");
  };

  const handleLogin = async () => {
    try {
      const response = await axios.post("/auth/login", {
        UserName: userName,
        Password: userPassword,
      });
      if (response.status === 200) {
        sessionStorage.setItem("accessToken", response.data);
        navigate("/courses");
      } else {
        console.log(response.status);
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <Container>
      <Box textAlign={"center"}>
        <Typography>Авторизация</Typography>
      </Box>
      <Button onClick={handleNoAccount}>Ещё нет аккаунта?</Button>
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
        <Button onClick={handleLogin}>Войти</Button>
      </Box>
    </Container>
  );
};
