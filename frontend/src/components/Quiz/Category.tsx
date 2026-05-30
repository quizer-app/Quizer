import { Link } from "@tanstack/react-router";

interface CategoryProps {
  text: string;
  to?: string;
  search?: { term: number };
}

export default function Category({ text, to, search }: CategoryProps) {
  return (
    <Link
      to={to ? to : "/"}
      search={search}
      className="bg-secondary flex items-center gap-3 rounded-sm px-4 py-3 font-medium w-full
                    max-w-[592px] sm:max-w-[500px] hover:bg-opacity-90"
    >
      <div className="bg-lightBlue w-8 h-8 rounded-sm"></div>
      {text}
    </Link>
  );
}
