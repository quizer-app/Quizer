import { Moon, Sun } from "lucide-react";
import { getTheme, setStartTheme, toggleTheme } from "@/utils/theme";
import { useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";

export default function DarkModeButton() {
  const [isDark, setIsDark] = useState<boolean>(getTheme() === "dark");
  const [isIconAnimating, setIsIconAnimating] = useState<boolean>(false);
  const animationTimeoutRef = useRef<number | null>(null);
  const { t } = useTranslation();
  useEffect(() => {
    return () => {
      if (animationTimeoutRef.current !== null) {
        window.clearTimeout(animationTimeoutRef.current);
      }
    };
  }, []);
  useEffect(() => {
    setStartTheme();
  }, []);

  const handleThemeChange = () => {
    setIsIconAnimating(true);
    toggleTheme();
    setIsDark(previousState => !previousState);

    if (animationTimeoutRef.current !== null) {
      window.clearTimeout(animationTimeoutRef.current);
    }

    animationTimeoutRef.current = window.setTimeout(() => {
      setIsIconAnimating(false);
      animationTimeoutRef.current = null;
    }, 250);
  };

  return (
    <div className="z-50 flex p-3 lg:pr-0">
      <button
        onClick={handleThemeChange}
        aria-label={t("nav.themeToggle")}
        type="button"
        className="group rounded-full p-1 transition duration-200 ease-out hover:scale-110 hover:bg-white/10 active:scale-95"
      >
        {isDark ? (
          <Sun
            className={`h-8 w-8 lg:h-9 lg:w-9 transition duration-200 ease-out group-hover:-rotate-12 group-hover:text-lightBlue ${
              isIconAnimating ? "rotate-180 scale-90" : ""
            }`}
            strokeWidth={1.6}
          />
        ) : (
          <Moon
            className={`h-8 w-8 lg:h-9 lg:w-9 transition duration-200 ease-out group-hover:rotate-12 group-hover:text-lightBlue ${
              isIconAnimating ? "-rotate-180 scale-90" : ""
            }`}
            strokeWidth={1.6}
          />
        )}
      </button>
    </div>
  );
}
