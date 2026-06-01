import { Link } from "@tanstack/react-router";
import { useTranslation } from "react-i18next";

interface NavLinksProps {
  isOpen: boolean;
  onClick: () => void;
  menuId: string;
}

export default function NavLinks({ isOpen, onClick, menuId }: NavLinksProps) {
  const { t } = useTranslation();

  const data = [
    { href: "/", text: t("nav.home") },
    {
      href: "/quiz-search",
      text: t("nav.quizzes"),
      search: { pageNumber: 1, pageSize: 6 },
    },
    {
      href: "/support",
      text: t("nav.support"),
    },
    { href: "/signin", text: t("nav.signIn"), auth: true },
    { href: "/signup", text: t("nav.signUp"), auth: true },
  ];

  return (
    <div>
      <ul
        id={menuId}
        aria-hidden={!isOpen}
        className={`${isOpen ? "flex" : "hidden"} flex-col bg-secondary
          absolute top-20 rounded-sm border-[.5px] gap-[10px] p-5 z-50
          border-textPrimary border-opacity-20 right-4 
          sm:right-[calc(50%-18.5rem)] md:right-[calc(50%-22.5rem)]
          sm:gap-4 sm:top-[5.5rem] lg:bg-transparent lg:flex-row lg:static
          lg:border-none lg:flex lg:gap-9 lg:ml-8 xl:gap-12 xl:ml-14
          `}
      >
        {data.map((el, id) => (
          <li key={id} className={`${el.auth ? "sm:hidden" : ""}`}>
            <Link
              className={`${el.auth ? "" : "lg:w-auto"} flex w-52 textHover`}
              onClick={onClick}
              to={el.href}
              search={el.search}
            >
              {el.text}
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
}
