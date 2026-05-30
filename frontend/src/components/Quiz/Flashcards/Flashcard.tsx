import {
  QuizPageFullPath,
  QuizPageSearchParams,
} from "@/types/schema/quizPageSchema";
import { QuestionResponse } from "@/types/types/quiz";
import { Link } from "@tanstack/react-router";
import { useState } from "react";
import { twMerge } from "tailwind-merge";
import Arrow from "./Arrow";

interface FlashcardProps {
  questions: QuestionResponse[];
  className: string;
  fullPath: QuizPageFullPath;
  term: number;
}

export default function Flashcard({
  questions,
  className,
  fullPath,
  term,
}: FlashcardProps) {
  const [isFlipped, setIsFlipped] = useState<boolean>(false);

  const handleFlip = () => {
    setIsFlipped(!isFlipped);
  };

  const decrease = (prev: QuizPageSearchParams) => {
    if (prev.term === 1) {
      return { term: questions.length };
    }
    return { term: prev.term - 1 };
  };

  const increase = (prev: QuizPageSearchParams) => {
    if (prev.term === questions.length) {
      return { term: 1 };
    }
    return { term: prev.term + 1 };
  };

  return (
    <>
      <div
        className={twMerge(
          "flex items-center justify-center [perspective:1000px]",
          className
        )}
      >
        <button
          onClick={handleFlip}
          className={`w-full h-full transition-transform duration-300 [transform-style:preserve-3d] ${
            isFlipped ? `[transform:rotateX(180deg)]` : ``
          }`}
        >
          <div className="bg-secondary flex items-center justify-center rounded-sm p-4 absolute w-full h-full [backface-visibility:hidden]">
            <p className="text-xl sm:text-2xl xl:text-3xl">
              {questions.length > 0
                ? questions[term - 1].question
                : "No questions available"}
            </p>
          </div>
          <div className="bg-secondary flex items-center justify-center rounded-sm p-4 w-full h-full overflow-auto [backface-visibility:hidden] [transform:rotateX(180deg)]">
            <p className="text-xl sm:text-2xl xl:text-3xl">
              {questions.length > 0
                ? questions[term - 1].answers[0].text
                : "No questions available"}
            </p>
          </div>
        </button>
      </div>

      {questions.length > 0 && (
        <div className="flex items-center justify-center gap-8">
          <Link from={fullPath} search={decrease} replace>
            <Arrow dir="left" />
          </Link>

          <p>{`${term} / ${questions.length}`}</p>

          <Link from={fullPath} search={increase} replace>
            <Arrow dir="right" />
          </Link>
        </div>
      )}
    </>
  );
}
