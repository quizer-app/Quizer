/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  darkMode: "class",
  theme: {
    extend: {
      colors: {
        primary: "#090E34",
        secondary: "#1D2144",
        lightBlue: "#4A6CF7",
        lightBlueB: "#6f86e0",
        darkBlue: "#2C409E",
        textHover: "#9BA1B3",
        textPrimary: "#959CB1",
        input: "#242B51",
      },
      fontFamily: {
        primary: ["Inter"],
      },
      opacity: {
        2: "0.98",
      },
    },
  },
  plugins: [],
};
