import React, { useState } from "react";
import Nav from "react-bootstrap/Nav";
import { Link } from "react-router-dom";
import "./navbar.css";

function NavbarPages() {
  const [activeItem, setActiveItem] = useState("/course");

  const handleSelect = (selectedKey) => {
    if (selectedKey !== null) {
      setActiveItem(selectedKey);
    }
  };

  const CustomNavLink = ({ to, children }) => (
    <Link to={to} className="custom-nav-link">
      {children}
    </Link>
  );

  return (
    <Nav variant="tabs" activeKey={activeItem} onSelect={handleSelect}>
      <Nav.Item>
        <CustomNavLink to="/courses">Главная страница</CustomNavLink>
      </Nav.Item>
      <Nav.Item>
        <CustomNavLink to="/my-courses">Я прохожу</CustomNavLink>
      </Nav.Item>
      <Nav.Item>
        <CustomNavLink to="/basket">Корзина</CustomNavLink>
      </Nav.Item>
    </Nav>
  );
}

export default NavbarPages;
