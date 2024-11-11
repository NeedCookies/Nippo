import { useRouteError } from "react-router-dom";

export default function ErrorPage() {
  const error = useRouteError();
  console.error(error);

  return (
    <div
      id="error-page"
      className="flex flex-col items-center justify-center h-screen">
      <h1 className="text-6xl font-bold m-2 p-2">Oops!</h1>
      <p className="text-4xl font-semibold m-2 p-2">
        Sorry, an unexpected error has occurred.
      </p>
      <p>
        <i>{error.statusText || error.message}</i>
      </p>
      <i
        className="fa-solid fa-bug text-2xl m-4 text-gray-400"
        id="bug-sign"></i>
    </div>
  );
}
