import React, { FC } from "react";
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
  if (!userIsLogged) {
    return <LoginPage />;
  }
  return children;
};

export default RequireAuth;
