import Background from "@/components/layout/Background/Background";
import { useTranslation } from "react-i18next";
import HomePageButton from "./HomePageButton";

interface ErrorProps {
  msg?: string;
}

export default function Error({ msg }: ErrorProps) {
  const { t } = useTranslation();

  return (
    <>
      <Background />
      <div
        className="text-white w-full h-[80vh] top-0 left-0 flex flex-col gap-16 items-center justify-center font-bold caret-transparent"
        role="alert"
        aria-live="assertive"
      >
        <p className="text-4xl text-center">{msg ? msg : t("status.errorOccurred")}</p>
        <HomePageButton />
      </div>
    </>
  );
}
