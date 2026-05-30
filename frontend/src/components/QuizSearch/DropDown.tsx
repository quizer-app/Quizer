import { ChevronDown, ChevronUp } from "lucide-react";
import { useState } from "react";
import { twMerge } from "tailwind-merge";

interface DropDownProps {
  text: string;
}

export default function DropDown({ text }: DropDownProps) {
  const [isOpen, setIsOpen] = useState<boolean>(false);

  const handleClick = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div className="relative">
      <button
        onClick={handleClick}
        className={twMerge(
          "dropDown relative h-12 w-36",
          isOpen ? "rounded-t-sm" : "rounded-sm"
        )}
      >
        {isOpen ? (
          <ChevronUp
            className="absolute top-[12px] right-3"
            strokeWidth={1.6}
          />
        ) : (
          <ChevronDown
            className="absolute top-[12px] right-3"
            strokeWidth={1.6}
          />
        )}
        {text}
      </button>
      {isOpen && (
        <div className="absolute top-12">
          <button className="dropDown h-12 w-36">Latest</button>
          <button className="dropDown h-12 w-36">Quiz Name</button>
          <button className="dropDown h-12 w-36 rounded-b-sm">User Name</button>
        </div>
      )}
    </div>
  );
}
