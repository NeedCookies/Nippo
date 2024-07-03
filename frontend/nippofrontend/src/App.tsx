import CourseListGroup from "./pages/courseListGroup";
import LikedCoursesList from "./pages/likedCoursesGroup";
import courses from "./components/FakeData/fakeCourses";
import NippoNavbar from "./components/navbar/navbarMain";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import CreateCourse from "./pages/CourseCreate/createCoursePage";
import QuizEditPage from "./pages/CourseCreate/QuizEditPage";

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
      </Routes>
    </BrowserRouter>
  );
}

export default App;
