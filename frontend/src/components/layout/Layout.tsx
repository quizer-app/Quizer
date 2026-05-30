import { Outlet } from "@tanstack/react-router";
import { Toaster } from "react-hot-toast";
import Footer from "./Footer/Footer";
import { Header } from "./Header/Header";

export default function Layout() {
  return (
    <>
      <Toaster />
      <Header />
      <Outlet />
      <Footer />
    </>
  );
}
