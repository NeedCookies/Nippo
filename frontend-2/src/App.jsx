import { useState } from "react";
import { Search } from "./components/Search";
import { AuthPage } from "./components/AuthPage";
import { CourseCard } from "./components/CourseCard";

function App() {
  const [isAuthorized, setAuthorized] = useState(false);

  return (
    <>
      <header className="header">
        <div className="header-container flex items-center justify-between p-2 gap-2">
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
            <button className="hover:shadow-slate-400 hover:shadow-sm px-2 py-1 rounded-md">
              <i className="fa-solid fa-user "></i>
            </button>
            <button className="border border-solid rounded-lg border-blue-400 px-2 blueShadow ">
              {isAuthorized ? "Профиль" : "Войти"}
            </button>
          </div>
        </div>
      </header>
      <div className="grid grid-col-4 m-4">
        <CourseCard></CourseCard>
      </div>
    </>
  );
}

export default App;
