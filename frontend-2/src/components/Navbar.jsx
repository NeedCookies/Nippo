import React, { useState } from "react";
import { Search } from "./Search";
import { NavLink, Outlet } from "react-router-dom";

export default function Navbar() {
  const [isAuthorized, setAuthorized] = useState(false);
  return (
    <div className="root-layout">
      <header className="header">
        <nav className="header-container flex items-center justify-between p-2 gap-2">
          <div className="flex items-center gap-4">
            <img
              src="./logo.jpg"
              className="w-[40px] sm:w-[60px] md:w-[80px]"></img>
            <a
              href="/"
              className="name text-blue-400 text-2xl sm:text-3xl md:text-4xl">
              Nippo
            </a>
          </div>
          <Search />
          <div className="flex items-center gap-4 mr-8">
            <NavLink
              to="profile"
              className="hover:shadow-slate-400 hover:shadow-sm px-2 py-1 rounded-md">
              <i className="fa-solid fa-user "></i>
            </NavLink>
            <NavLink
              to="login"
              className="border border-solid rounded-lg border-blue-400 px-2 blueShadow ">
              {isAuthorized ? "Профиль" : "Войти"}
            </NavLink>
          </div>
        </nav>
      </header>

      <main>
        <Outlet />
      </main>
    </div>
  );
}
