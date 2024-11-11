import React, { useState } from "react";
import { fakeData } from "../../utils/courses";
import av1 from "../../Pictures/images.png";

export default function BacketCard(props) {
  const { name, description, price } = props;
  const [isActive, setActive] = useState(false);
  return (
    <div className="flex bg-gray-200 my-2 rounded p-2 gap-2 justify-between">
      <div className="flex justify-left">
        <button onClick={() => setActive(!isActive)}>
          {isActive ? (
            <i className="fa-solid fa-circle-check text-blue-800"></i>
          ) : (
            <i className="fa-regular fa-circle-check text-gray-400 hover:text-blue-200 duration-200"></i>
          )}
        </button>
        <img src={av1} className="rounded m-2 w-[80px] sm:w-[100px]"></img>
      </div>
      <h4 className="m-2 text-slate-900 font-medium max-w-48 md:max-w-64 md:min-w-64">
        {name}
      </h4>
      <p className="m-2 flex gap-1 text-slate-600 font-medium">
        {price + " "}
        <i className="fa-solid fa-ruble-sign mt-1"></i>
      </p>
      <div className="flex flex-col p-2 gap-2 max-w-fit w-full">
        <button className="bg-slate-400 w-full px-2 py-1 rounded hover:bg-slate-600">
          <i className="fa-solid fa-bolt mr-1"></i>
          Купить
        </button>
        <button className="bg-slate-400 w-full px-2 py-1 rounded hover:bg-slate-600">
          <i className="fa-solid fa-trash mr-1"></i>
          Удалить
        </button>
      </div>
    </div>
  );
}
