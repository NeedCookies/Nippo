import { useState } from "react";
import { AuthPage } from "./components/AuthPage";
import {
  createBrowserRouter,
  createRoutesFromElements,
  Route,
  RouterProvider,
} from "react-router-dom";
import Navbar from "./components/Navbar";
import Main from "./components/MainPage/Main";

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route path="/" element={<Navbar />}>
      <Route index element={<Main />} />
      <Route path="login" element={<AuthPage />} />
    </Route>
  )
);

function App() {
  const [isAuthorized, setAuthorized] = useState(false);

  return <RouterProvider router={router} />;
}

export default App;
