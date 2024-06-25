import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import Image from "react-bootstrap/Image";
import AppUser from "../Interfaces.js";
import NavbarPages from "./navbarPages.js";
import logo from "../Images/logo.jpg";

function NippoNavbar({ nick, points }: AppUser) {
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
            <Navbar.Brand href="#home">
              <Image
                src={logo}
                className="d-inline-block align-center"
                width="30"
                height="30"
                alt="logo"
                rounded
              />
            </Navbar.Brand>
          </Nav.Item>
          <Nav.Item>
            <Nav.Link eventKey={"#home"} className="text-light">
              <h2 className="text-uppercase">Nippo</h2>
            </Nav.Link>
          </Nav.Item>
        </div>
        <NavbarPages />
        <Navbar.Text>
          <a
            href="#login"
            className="text-uppercase font-weight-bold text-light mr-2 d-flex align-items-center">
            {nick}
          </a>
          <div className="font-weight-bold">{points}</div>
        </Navbar.Text>
      </Container>
    </Navbar>
  );
}

export default NippoNavbar;
