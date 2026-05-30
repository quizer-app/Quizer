import { AnswerProps } from "@/routes/_quizLayout/$username/$quizSlug/test";
import { Check, X } from "lucide-react";
import { RefObject } from "react";

interface QuestionNavProps {
  answers: AnswerProps[];
  score: number;
  length: number;
  inputRefs: RefObject<HTMLElement[]>;
}

export default function QuestionNav({
  answers,
  score,
  length,
  inputRefs,
}: QuestionNavProps) {
  const onClick = (id: number) => {
    if (inputRefs.current && inputRefs.current[id]) {
      inputRefs.current[id]?.scrollIntoView({
        behavior: "smooth",
        block: id === 0 ? "end" : "center",
      });
    }
  };

  return (
    <>
      <p className="text-3xl font-bold text-green-600">{`Correct: ${score}`}</p>
      <p className="text-3xl font-bold text-red-600">{`Incorrect: ${length - score}`}</p>
      <div className="fixed top-0 left-10 flex flex-col gap-1 p-4 bg-secondary rounded-sm border-[1px] border-opacity-10 border-white">
        {answers.map((answer, id) => {
          return (
            <button
              onClick={() => onClick(id)}
              key={id}
              className="flex gap-2 items-center hover:bg-blue-100"
            >
              {answer.isCorrect ? (
                <Check
                  key={id}
                  size={22}
                  className="text-green-600"
                  strokeWidth={1.6}
                />
              ) : (
                <X
                  key={id}
                  size={22}
                  className="text-red-600"
                  strokeWidth={1.6}
                />
              )}
              <p className="pb-[2px]">{answer.number}</p>
            </button>
          );
        })}
      </div>
    </>
  );
}
