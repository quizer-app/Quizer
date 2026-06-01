import Background from "@/components/layout/Background/Background";
import HomePageButton from "@/components/Status/HomePageButton";
import { useTranslation } from "react-i18next";
import NotFoundSvg from "./NotFoundSvg";

export default function NotFound() {
  const { t } = useTranslation();

  return (
    <>
      <Background />
      <div
        className="text-white w-full h-[80vh] top-0 left-0 flex flex-col gap-10 items-center justify-center font-bold caret-transparent"
        role="alert"
        aria-live="polite"
      >
        <NotFoundSvg />
        <p className="text-4xl text-center">{t("status.notFound")}</p>
        <HomePageButton />
      </div>
    </>
  );
}
