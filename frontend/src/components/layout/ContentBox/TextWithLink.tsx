import { Link } from "@tanstack/react-router";

interface TextWithLinkProps {
  children: React.ReactNode;
  link: string;
  path: string;
}

export default function TextWithLink({
  children,
  link,
  path,
}: TextWithLinkProps) {
  return (
    <p className="text-textPrimary font-medium text-center">
      {children}
      <Link to={path} className="text-lightBlue">
        {" " + link}
      </Link>
    </p>
  );
}
