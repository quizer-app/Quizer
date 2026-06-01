import { Moon, Sun } from "lucide-react";
import { getTheme, setStartTheme, toggleTheme } from "@/utils/theme";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";

export default function DarkModeButton() {
  const [isDark, setIsDark] = useState<boolean>(getTheme() === "dark");
  const { t } = useTranslation();
  useEffect(() => {
    setStartTheme();
  }, []);

  const handleThemeChange = () => {
    toggleTheme();
    setIsDark(previousState => !previousState);
  };

  return (
    <div className="flex p-3 lg:pr-0 z-50">
      <button onClick={handleThemeChange} aria-label={t("nav.themeToggle")} type="button">
        {isDark ? (
          <Sun className="w-8 h-8 lg:w-9 lg:h-9" strokeWidth={1.6} />
        ) : (
          <Moon className="w-8 h-8 lg:w-9 lg:h-9" strokeWidth={1.6} />
        )}
      </button>
    </div>
  );
}
