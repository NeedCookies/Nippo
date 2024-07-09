import CourseListGroup from "./pages/courseListGroup";
import LikedCoursesList from "./pages/likedCoursesGroup";
import courses from "./components/FakeData/fakeCourses";
import NippoNavbar from "./components/navbar/navbarMain";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import CreateCourse from "./pages/CourseCreate/createCoursePage";
import QuizEditPage from "./pages/CourseCreate/QuizEditPage";
import { PersonalAccount } from "./pages/PersonalAccount/PersonalAccount";
import { RegisterPage } from "./pages/RegisterPage";
import { LoginPage } from "./pages/LoginPage";

function App() {
  return (
    <BrowserRouter>
      <NippoNavbar nick="Cookie" points={1500} userName="Emil" />
      <Routes>
        <Route path="/courses" element={<CourseListGroup cards={courses} />} />
        <Route
          path="/courses/liked"
          element={<LikedCoursesList cards={courses} />}
        />
        <Route path="course/create" element={<CreateCourse />} />
        <Route
          path="/course/:courseId/quiz/:quizId/edit"
          element={<QuizEditPage />}
        />
        <Route path="/profile" element={<PersonalAccount />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/login" element={<LoginPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
