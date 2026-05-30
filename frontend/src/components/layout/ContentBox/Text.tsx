import { twMerge } from "tailwind-merge";

interface TextProps {
  children: React.ReactNode;
  className?: string;
}

export default function Text({ children, className }: TextProps) {
  return (
    <p
      className={twMerge(
        "text-textPrimary text-center font-medium mb-11",
        className
      )}
    >
      {children}
    </p>
  );
}
