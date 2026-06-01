const THEME_TRANSITION_CLASS = "theme-switching";
const THEME_TRANSITION_DURATION_MS = 250;

type Theme = "light" | "dark";

const isTheme = (value: string | null): value is Theme => value === "light" || value === "dark";

const applyTheme = (theme: Theme) => {
  const htmlTag = document.documentElement;
  htmlTag.classList.remove("light", "dark");
  htmlTag.classList.add(theme);
  localStorage.setItem("theme", theme);
};

const startThemeTransition = () => {
  const htmlTag = document.documentElement;
  htmlTag.classList.add(THEME_TRANSITION_CLASS);
  window.setTimeout(() => {
    htmlTag.classList.remove(THEME_TRANSITION_CLASS);
  }, THEME_TRANSITION_DURATION_MS);
};

export const getTheme = (): Theme => {
  const theme = localStorage.getItem("theme");

  if (isTheme(theme)) {
    return theme;
  }

  return "dark";
};

export const setStartTheme = (): Theme => {
  const initialTheme = getTheme();
  applyTheme(initialTheme);
  return initialTheme;
};

export const toggleTheme = (): Theme => {
  const currentTheme = document.documentElement.classList.contains("light") ? "light" : "dark";
  const nextTheme = currentTheme === "light" ? "dark" : "light";

  startThemeTransition();
  applyTheme(nextTheme);

  return nextTheme;
};
