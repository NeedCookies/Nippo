import React, { FC, useEffect, useState } from "react";
import { useLocation } from "react-router-dom";

const NavbarLayout: FC<{ children: React.ReactElement }> = ({ children }) => {
  const location = useLocation();

  const [showNavbar, setShowNavbar] = useState<boolean>(false);

  useEffect(() => {
    if (location.pathname === "/login" || location.pathname === "/register") {
      setShowNavbar(false);
    } else {
      setShowNavbar(true);
    }
  }, [location]);
  return <>{showNavbar && children}</>;
};

export default NavbarLayout;
