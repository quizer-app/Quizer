import Background from "@/components/layout/Background/Background";
import { FadeLoader } from "react-spinners";
import { useTranslation } from "react-i18next";

export default function Loading() {
  const { t } = useTranslation();

  return (
    <>
      <Background />
      <div
        className="w-full h-[80vh] top-0 left-0 flex items-center justify-center"
        role="status"
        aria-live="polite"
        aria-label={t("status.loading")}
      >
        <FadeLoader color="white" />
      </div>
    </>
  );
}
