import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import Image from "react-bootstrap/Image";
import NavbarPages from "./navbarPages.js";
import logo from "../Images/logo.jpg";
import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import "./navbar.css";

interface UserInfo {
  firstName: string | "undefined";
  lastName: string | null;
  userName: string;
  phoneNumber: string | null;
  email: string;
  pictureUrl: string | null;
  birthDate: string | null;
  points: number;
  role: string;
}

const NippoNavbar = () => {
  const [userInfo, setUserInfo] = useState<UserInfo>({
    firstName: "Error",
    lastName: "empty",
    userName: "error",
    phoneNumber: "+-1 -1 -1 -1 -1",
    email: "testUser@mail.ru",
    pictureUrl: null,
    birthDate: "2004-06-09",
    points: 0,
    role: "user",
  });

  useEffect(() => {
    const getUserInfo = async () => {
      try {
        const response = await axios.get("/user/get-personal-info");
        if (response.status == 200) {
          setUserInfo(response.data);
        } else {
          console.log("Different response status");
          console.log(response.status);
        }
      } catch (error) {
        setUserInfo({
          firstName: "Error",
          lastName: "empty",
          userName: "error",
          phoneNumber: "+-1 -1 -1 -1 -1",
          email: "testUser@mail.ru",
          pictureUrl: null,
          birthDate: "2004-06-09",
          points: 0,
          role: "user",
        });
        console.log("Fali to get data");
        console.error(error);
      }
    };
    getUserInfo();
  }, []);
  return (
    <Navbar
      bg="dark"
      data-bs-theme="dark"
      expand="lg"
      className="bg-body-tertiary d-sm-flex justify-content-around">
      <Container
        fluid
        className="d-flex justify-content-between align-items-center">
        <div className="d-flex align-items-center">
          <Nav.Item className="justify-content-end">
            <Link to={"/courses"} className="navbar-nippo">
              <Image
                src={logo}
                className="d-inline-block align-center"
                width="40"
                height="40"
                alt="logo"
                rounded
              />
            </Link>
          </Nav.Item>
          <Nav.Item>
            <Link to={"/courses"} className="text-light custom-nippo">
              <h2 className="text-uppercase custom-nippo">Nippo</h2>
            </Link>
          </Nav.Item>
        </div>
        <NavbarPages />
        <Navbar.Text>
          <Link
            to={"/profile"}
            className="text-uppercase font-weight-bold text-light mr-2 d-flex align-items-center">
            {userInfo.userName}
          </Link>
          <div className="font-weight-bold">{userInfo?.points}</div>
        </Navbar.Text>
      </Container>
    </Navbar>
  );
};

export default NippoNavbar;
