import { Outlet } from "@tanstack/react-router";
import { Toaster } from "react-hot-toast";
import { Header } from "./Header/Header";

export default function Layout() {
  return (
    <>
      <Toaster position="top-center" toastOptions={{ duration: 4000 }} />
      <Header />
      <Outlet />
    </>
  );
}
