import { AuthPage } from "./components/AuthPage";
import {
  createBrowserRouter,
  createRoutesFromElements,
  Route,
  RouterProvider,
} from "react-router-dom";
import Navbar from "./components/Navbar";
import Main from "./components/MainPage/Main";
import Backet from "./components/Backet/Backet";
import ErrorsPage from "./components/ErrorsPage";
import Profile from "./components/Profile";

const router = createBrowserRouter(
  [
    {
      path: "/",
      element: <Navbar />,
      errorElement: <ErrorsPage />,
      children: [
        {
          path: "",
          element: <Main />,
        },
        {
          path: "/backet",
          element: <Backet />,
        },
        {
          path: "login",
          element: <AuthPage />,
        },
        {
          path: "profile",
          element: <Profile />,
        },
      ],
    },
  ]
  /*createRoutesFromElements(
    <Route path="/" element={<Navbar />}>
      <Route index element={<Main />} />
      <Route path="login" element={<AuthPage />} />
      <Route path="backet" element={<Backet />} />
    </Route>
  )*/
);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
