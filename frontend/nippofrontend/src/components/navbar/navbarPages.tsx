import Nav from "react-bootstrap/Nav";

function NavbarPages() {
  return (
    <Nav.Item className="col-8">
      <Nav variant="tabs" defaultActiveKey="/home">
        <Nav.Item>
          <Nav.Link href="/home">Главная страница</Nav.Link>
        </Nav.Item>
        <Nav.Item>
          <Nav.Link eventKey="link-1">Избранные курсы</Nav.Link>
        </Nav.Item>
        <Nav.Item>
          <Nav.Link eventKey="link-2">Личный кабинет</Nav.Link>
        </Nav.Item>
      </Nav>
    </Nav.Item>
  );
}

export default NavbarPages;
