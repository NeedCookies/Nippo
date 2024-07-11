import React, { useState } from "react";
import Nav from "react-bootstrap/Nav";

function NavbarPages() {
  const [activeItem, setActiveItem] = useState("/course");

  const handleSelect = (selectedKey) => {
    if (selectedKey !== null) {
      setActiveItem(selectedKey);
    }
  };

  return (
    <Nav.Item className="col-8">
      <Nav variant="tabs" activeKey={activeItem} onSelect={handleSelect}>
        <Nav.Item>
          <Nav.Link href="/courses" eventKey="/courses">
            Главная страница
          </Nav.Link>
        </Nav.Item>
        <Nav.Item>
          <Nav.Link href="/my-courses" eventKey="/my-courses">
            Я прохожу
          </Nav.Link>
        </Nav.Item>
        <Nav.Item>
          <Nav.Link href="/basket" eventKey="/basket">
            Корзина
          </Nav.Link>
        </Nav.Item>
      </Nav>
    </Nav.Item>
  );
}

export default NavbarPages;
