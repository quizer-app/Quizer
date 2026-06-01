const THEME_TRANSITION_CLASS = "theme-switching";
const THEME_TRANSITION_DURATION_MS = 250;

const applyTheme = (theme: "light" | "dark") => {
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

export const toggleTheme = () => {
  const currentTheme = document.documentElement.classList.contains("light")
    ? "light"
    : "dark";
  const nextTheme = currentTheme === "light" ? "dark" : "light";

  startThemeTransition();
  applyTheme(nextTheme);
};

export const getTheme = (): string => {
  const theme = localStorage.getItem("theme");
  return theme ?? "dark";
};

export const setStartTheme = () => {
  const initialTheme = localStorage.getItem("theme") === "light" ? "light" : "dark";
  applyTheme(initialTheme);
};