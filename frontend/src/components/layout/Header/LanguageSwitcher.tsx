import { useTranslation } from "react-i18next";

export default function LanguageSwitcher() {
  const { i18n, t } = useTranslation();

  const nextLanguage = i18n.resolvedLanguage === "pl" ? "en" : "pl";

  return (
    <button
      type="button"
      className="text-xs font-semibold px-2 py-1 border border-white/30 rounded-sm hover:border-white/50"
      aria-label={t("nav.switchLanguage")}
      onClick={() => {
        void i18n.changeLanguage(nextLanguage);
      }}
    >
      {i18n.resolvedLanguage === "pl" ? "EN" : "PL"}
    </button>
  );
}
