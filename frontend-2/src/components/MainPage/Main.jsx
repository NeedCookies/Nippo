import React from "react";
import { CourseCard } from "./CourseCard";
import { fakeData } from "../../utils/courses";

export default function Main() {
  return (
    <div className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 my-10 mx-20 gap-4">
      {fakeData.courses.map((course, index) => (
        <CourseCard
          key={index}
          name={course.name}
          description={course.description}
          price={course.price}
        />
      ))}
    </div>
  );
}
