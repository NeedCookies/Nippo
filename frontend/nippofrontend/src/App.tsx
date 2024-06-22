import CourseListGroup from "./components/courseComponents/courseListGroup";
import courses from "./components/FakeData/fakeCourses";
import NippoNavbar from "./components/navbar/navbarMain";

function App() {
  return (
    <div>
      <NippoNavbar nick="Cookie" points={1500} userName="Emil" />
      <CourseListGroup cards={courses} />
    </div>
  );
}

export default App;
