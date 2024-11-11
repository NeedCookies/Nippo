import React, { useState } from "react";
import { NavLink } from "react-router-dom";

export default function Profile() {
  const { haveAvatar, setHaveAvatar } = useState(false);
  const [name, setName] = useState("Ivan");
  const [surname, setSurname] = useState("Ivanov");
  const [phone, setPhone] = useState("0-000-000-00-0");
  const [email, setEmail] = useState("empty@email.com");
  const [points, setPoints] = useState(0);
  const [solvedTasks, setSolvedTasks] = useState(0);
  const [registerDate, setRegisterDate] = useState(
    new Date().getFullYear() - 1
  );
  const [status, setUserStatus] = useState("admin");

  function calcRegisterPeriod() {
    return new Date().getFullYear() - registerDate;
  }

  return (
    <div className="p-2 md:p-6 lg:p-8">
      <div className="grid grid-cols-4 gap-2">
        <div className="col-span-1 gap-2 p-2 flex flex-col items-center justify-between">
          <div
            className="flex avatar-name bg-cyan-400 rounded-lg text-6xl md:text-8xl text-white 
          shadow shadow-lg p-8 w-[80px] md:w-[150px] h-[80px] md:h-[150px] items-center justify-center
          mb-4">
            {name.slice(0, 1)}
            {surname.slice(0, 1)}
          </div>
          <p className="text-gray-400 font-semibold">
            Выполнено: <span className="text-gray-600 ">{solvedTasks} </span>
            <i className="fa-solid fa-list-check"></i>
          </p>
          <p className="text-gray-400 font-semibold">
            Статус: <span className="text-gray-600 italic">{status}</span>
          </p>
          <p className="text-blue-600 hover:text-blue-800 cursor-pointer hover:underline">
            Сертификаты
          </p>
          <div className="text-gray-400 wrap pb-2 text-center italic">
            Вы с нами уже{" "}
            <span className="text-gray-600">
              {calcRegisterPeriod()}{" "}
              {calcRegisterPeriod() > 4
                ? "лет"
                : calcRegisterPeriod() > 1
                ? "года"
                : "год"}
            </span>
          </div>
          <div className="w-full bg-gray-400 h-[1px] rounded"></div>
          <p className="text-gray-400 font-semibold">
            Телефон: <span className="text-gray-600">{phone}</span>
          </p>
          <p className="text-gray-400 font-semibold">
            Email: <span className="text-gray-600">{email}</span>
          </p>
          <button className="font-semibold text-gray-600 hover:text-gray-800 p-2 rounded-lg hover:shadow-lg hover:bg-gray-100">
            Редактировать
          </button>
        </div>
        <div className="col-span-3 gap-2 p-2">
          <h2 className="flex profile-name mb-4 text-4xl md:text-6xl justify-center">
            {name} {surname}
          </h2>
          <div className="flex items-center justify-between gap-6 p-4 flex-wrap">
            <NavLink
              to={"my-stats"}
              className="bg-slate-200 p-4 rounded-lg shadow-lg hover:bg-slate-300 cursor:pointer font-semibold text-gray-800 basis-1/4">
              Моя статистика{" "}
              <i className="fa-solid fa-ranking-star text-blue-400"></i>
            </NavLink>
            <NavLink
              to={"favorite-courses"}
              className="bg-slate-200 p-4 rounded-lg shadow-lg hover:bg-slate-300 cursor:pointer font-semibold text-gray-800 basis-1/4">
              Избранные курсы <i className="fa-solid fa-heart text-red-400"></i>
            </NavLink>
            <NavLink
              to={"my-courses"}
              className="bg-slate-200 p-4 rounded-lg shadow-lg hover:bg-slate-300 cursor:pointer font-semibold text-gray-800 basis-1/4">
              Купленные курсы{" "}
              <i className="fa-solid fa-clipboard-check text-green-400"></i>
            </NavLink>

            {(status === "author" || status === "admin") && (
              <>
                <div className="w-full bg-gray-400 h-[1px] rounded"></div>
                <NavLink
                  to={"create-course"}
                  className="bg-slate-200 p-4 rounded-lg shadow-lg hover:bg-slate-300 cursor:pointer font-semibold text-gray-800 basis-1/3">
                  Создать курс{" "}
                  <i className="fa-solid fa-screwdriver-wrench text-blue-400"></i>
                </NavLink>
                <NavLink
                  to={"my-courses-stats"}
                  className="bg-slate-200 p-4 rounded-lg shadow-lg hover:bg-slate-300 cursor:pointer font-semibold text-gray-800 basis-1/3">
                  Статистика моих курсов
                </NavLink>
              </>
            )}
            {status === "admin" && (
              <>
                <div className="w-full bg-gray-400 h-[1px] rounded"></div>
                <NavLink
                  to={"course-moderation"}
                  className="bg-slate-200 p-4 rounded-lg shadow-lg hover:bg-slate-300 cursor:pointer font-semibold text-gray-800 basis-1/3">
                  Модерация курсов{" "}
                  <i className="fa-solid fa-user-tie text-blue-600"></i>
                </NavLink>
                <button className="bg-slate-200 p-4 rounded-lg shadow-lg hover:bg-slate-300 cursor:pointer font-semibold text-gray-800 basis-1/3">
                  Создать промокод{" "}
                  <i className="fa-solid fa-percent text-red-600"></i>
                </button>
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
