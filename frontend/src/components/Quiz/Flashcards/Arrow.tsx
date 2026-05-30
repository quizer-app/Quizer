import { twMerge } from "tailwind-merge";

interface ArrowProps {
  dir: "left" | "right";
}

export default function Arrow({ dir }: ArrowProps) {
  return (
    <button
      aria-label={`${dir === "right" ? "Next question" : "Previous question"}`}
      className="group flex w-10 h-10 md:w-12 md:h-12 items-center justify-center 
      rounded-full shadow-md transition bg-secondary ring-[0.5px] ring-lightBlueB/30 hover:ring-lightBlue/60"
    >
      <svg
        viewBox="0 0 16 16"
        aria-hidden="true"
        className={twMerge(
          "w-4 h-4 md:w-6 md:h-6 stroke-lightBlueB group-hover:stroke-lightBlue transition",
          dir === "right" ? "rotate-180" : ""
        )}
      >
        <path
          d="M7.25 11.25 3.75 8m0 0 3.5-3.25M3.75 8h8.5"
          strokeWidth="1.5"
          strokeLinecap="round"
          strokeLinejoin="round"
        ></path>
      </svg>
    </button>
  );
}
