import React, { useEffect, useState } from "react";
import { fakeData } from "../../utils/courses";
import BacketCard from "./BacketCard";

export default function Backet() {
  const backetGoods = fakeData;
  const [totalSum, setTotalSum] = useState(0);

  useEffect(() => {
    setTotalSum(
      backetGoods.courses.reduce(
        (res, course) => (res = res + Number(course.price)),
        0
      )
    );
  }, []);

  return (
    <div className="grid lg:grid-cols-8">
      <div className="lg:col-span-1"></div>
      <div className="lg:col-span-6 gap-4 grid grid-cols-3 text-white p-6 rounded-lg">
        <div className="bg-gray-300 shadow-lg col-span-2 rounded-lg p-4 gap-2">
          {backetGoods.courses.map((course, index) => (
            <BacketCard
              key={index}
              name={course.name}
              description={course.description}
              price={course.price}
            />
          ))}
        </div>
        <div className="bg-gray-300 shadow-lg col-span-1 rounded-lg p-4 h-fit">
          <h2 className="text-slate-950 text-2xl font-semibold text-center mb-4">
            Ваша корзина
          </h2>
          <div className="bg-slate-400 h-[2px] mb-4"></div>
          {backetGoods.length == 0 ? (
            <p className="text-slate-900 text-xl font-semibold">
              Ваша корзина пуста
            </p>
          ) : (
            <div className="flex gap-2 justify-between m-6 mb-4">
              <p className="text-slate-400 text-sm font-semibold">
                {backetGoods.courses.length} курсов
              </p>
              <p className="text-slate-500 text-sm font-semibold">
                На общую сумму {totalSum}
                <i className="fa-solid fa-ruble-sign mt-1 ml-1 text-sm"></i>
              </p>
            </div>
          )}
          <div className="text-center">Общая скидка = 0 Р</div>
          <div className="bg-slate-400 h-[2px] mb-4 my-2"></div>
          <button className="bg-green-600 rounded-lg m-4 p-4 w-5/6 hover:bg-green-700">
            Купить
          </button>
        </div>
      </div>
      <div className="lg:col-span-1"></div>
    </div>
  );
}
