import av1 from "../../Pictures/images.png";

export function CourseCard(props) {
  const { name, description, price } = props;
  return (
    <div className="h-[450px] flex flex-col pb-3 max-w-sm bg-white border-border-gray-200 rounded-lg shadow relative">
      <a
        href="#"
        className="relative overflow-hidden min-h-[200px] max-h-[200px]">
        <img
          src={av1}
          className="absolute top-0 left-0 w-full h-full object-cover rounded-t-lg"
        />
      </a>
      <div className="p-2 relative h-[220px]">
        <a href="#">
          <h5 className="wrap mb-2 text-2xl font-bold tracking-tight text-gray-900 ">
            {name}
          </h5>
        </a>
        <p className="mb-3 font-normal text-gray-700 wrap overflow-hidden line-clamp-4">
          {description}
        </p>
        <a
          href="#"
          title="Добавить в корзину"
          className={`
            items-center px-5 py-2 text-md font-medium 
            text-center text-white rounded-lg absolute bottom-0 right-2 
            ${
              Number(price) > 0
                ? "bg-blue-600 hover:bg-blue-800"
                : "bg-green-600 hover:bg-green-800"
            }
          `}>
          {Number(price) > 0 ? `${price}` : "Бесплатно"}
          <i className="fa-solid fa-basket-shopping ml-2"></i>
        </a>
      </div>
    </div>
  );
}
