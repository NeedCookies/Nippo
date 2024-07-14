import CourseListGroup from "./pages/courseListGroup";
import LikedCoursesList from "./pages/likedCoursesGroup";
import courses from "./components/FakeData/fakeCourses";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import CreateCourse from "./pages/CourseCreate/createCoursePage";
import QuizEditPage from "./pages/CourseCreate/QuizEditPage";
import LessonEditPage from "./pages/CourseCreate/LessonEditPage";
import { PersonalAccount } from "./pages/PersonalAccount/PersonalAccount";
import { RegisterPage } from "./pages/RegisterPage";
import { LoginPage } from "./pages/LoginPage";
import RequireAuth from "./components/RequireAuth";
import NavbarLayout from "./components/NavbarLayout";
import NippoNavbar from "./components/navbar/navbarMain";
import { Basket } from "./pages/Basket";
import { CourseMainPage } from "./pages/CourseWatch/CourseMainPage";
import { QuizMainPage } from "./pages/CourseWatch/QuizMainPage";

function App() {
  return (
    <BrowserRouter>
      <NavbarLayout>
        <NippoNavbar />
      </NavbarLayout>
      <Routes>
        <Route path="/courses" element={<CourseListGroup />} />
        <Route path="/my-courses" element={<LikedCoursesList />} />
        <Route
          path="/my-course/:courseId"
          element={
            <RequireAuth>
              <CourseMainPage />
            </RequireAuth>
          }
        />
        <Route
          path="/my-course/:courseId/quiz/:quizId"
          element={
            <RequireAuth>
              <QuizMainPage />
            </RequireAuth>
          }
        />
        <Route
          path="course/create"
          element={
            <RequireAuth>
              <CreateCourse />
            </RequireAuth>
          }
        />
        <Route
          path="course/:courseId/create"
          element={
            <RequireAuth>
              <CreateCourse />
            </RequireAuth>
          }
        />
        <Route
          path="/course/:courseId/quiz/:quizId/edit"
          element={<QuizEditPage />}
        />
        <Route
          path="/course/:courseId/lesson/:lessonId/edit"
          element={<LessonEditPage />}
        />
        <Route
          path="/profile"
          element={
            <RequireAuth>
              <PersonalAccount />
            </RequireAuth>
          }
        />
        <Route path="/basket" element={<Basket />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/" element={<LoginPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
