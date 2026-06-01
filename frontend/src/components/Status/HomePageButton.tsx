import { Link } from "@tanstack/react-router";
import { useTranslation } from "react-i18next";

export default function HomePageButton() {
  const { t } = useTranslation();

  return (
    <Link
      to="/"
      className="bg-lightBlue py-4 px-8 rounded-sm hover:bg-opacity-95 z-50"
    >
      {t("status.homePage")}
    </Link>
  );
}
