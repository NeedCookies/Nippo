import CourseCardProps from "./Interfaces/CourseCardProps";

function CourseCard({ title, description, price }: CourseCardProps) {
  return (
    <>
      <div className="card">
        <div className="card-body ">
          <h5 className="card-title">{title}</h5>
          <p className="card-text">{description.substring(0, 100) + "..."}</p>
          <a href="#" className="btn btn-primary">
            {price}
          </a>
        </div>
      </div>
    </>
  );
}

export default CourseCard;
