import { Outlet } from "@tanstack/react-router";
import { Toaster } from "react-hot-toast";
import QuizHeader from "./Header/QuizHeader";
// import Background from "./Background/Background";

export default function QuizLayout() {
  return (
    <>
      <Toaster />
      <QuizHeader />
      {/* <Background /> */}
      <Outlet />
    </>
  );
}
