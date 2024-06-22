import CourseCardProps from "./Interfaces/courseCardProps";
import CourseListGroupProps from "./Interfaces/courseListGroupProps";
import CourseCard from "./courseCard";

function CourseListGroup({ cards }: CourseListGroupProps) {
  const listCards = cards.map((card: CourseCardProps, index: number) => (
    <li key={index} className="list-group-item d-inline-block">
      <CourseCard
        title={card.title}
        description={card.description}
        price={card.price}></CourseCard>
    </li>
  ));

  return (
    <ul className="list-group d-flex col-6 col-md-4 col-lg-3 col-xl-2 horizontal">
      {listCards}
    </ul>
  );
}

export default CourseListGroup;
