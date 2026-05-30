import { Link } from "@tanstack/react-router";

export default function HomePageButton() {
  return (
    <Link
      to="/"
      className="bg-lightBlue py-4 px-8 rounded-sm hover:bg-opacity-95 z-50"
    >
      Home Page
    </Link>
  );
}
