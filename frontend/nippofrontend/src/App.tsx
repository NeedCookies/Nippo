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

function App() {
  return (
    <BrowserRouter>
      <NavbarLayout>
        <NippoNavbar />
      </NavbarLayout>
      <Routes>
        <Route path="/courses" element={<CourseListGroup cards={courses} />} />
        <Route
          path="/courses/liked"
          element={<LikedCoursesList cards={courses} />}
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
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/login" element={<LoginPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
