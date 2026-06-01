import { useMemo } from "react";
import { useTranslation } from "react-i18next";

const SUPPORTED_LANGUAGES = [
  { value: "en", label: "🇬🇧 EN" },
  { value: "pl", label: "🇵🇱 PL" },
] as const;

export default function LanguageSwitcher() {
  const { i18n, t } = useTranslation();

  const selectedLanguage = useMemo(() => {
    const resolvedLanguage = i18n.resolvedLanguage;
    return resolvedLanguage === "pl" ? "pl" : "en";
  }, [i18n.resolvedLanguage]);

  return (
    <label className="relative">
      <span className="sr-only">{t("nav.switchLanguage")}</span>
      <select
        value={selectedLanguage}
        aria-label={t("nav.switchLanguage")}
        className="h-10 min-w-24 rounded-md border border-white/30 bg-transparent px-3 text-sm font-semibold hover:border-white/50 focus:border-white/60 focus:outline-none"
        onChange={event => {
          void i18n.changeLanguage(event.target.value);
        }}
      >
        {SUPPORTED_LANGUAGES.map(language => (
          <option key={language.value} value={language.value} className="text-primary">
            {language.label}
          </option>
        ))}
      </select>
    </label>
  );
}
