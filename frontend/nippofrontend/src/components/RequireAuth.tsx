import React, { FC, useEffect } from "react";
import { LoginPage } from "../pages/LoginPage";
import { useNavigate } from "react-router-dom";

const useLoginStatus = () => {
  if (sessionStorage.getItem("accessToken")) {
    return true;
  }
  return false;
};

const RequireAuth: FC<{ children: React.ReactElement }> = ({ children }) => {
  const userIsLogged = useLoginStatus();
  const navigate = useNavigate();
  useEffect(() => {
    if (!userIsLogged) {
      navigate("/login");
    }
  }, [userIsLogged, navigate]);

  if (!userIsLogged) {
    return null; // Возвращаем null, чтобы ничего не рендерить до тех пор, пока не произойдет перенаправление
  }

  return children;
};

export default RequireAuth;
