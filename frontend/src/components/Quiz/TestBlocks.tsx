import {
  AnswerProps,
  FormFields,
} from "@/routes/_quizLayout/$username/$quizSlug/test";
import { QuizResponse } from "@/types/types/quiz";
import { RefObject } from "react";
import { twMerge } from "tailwind-merge";

interface TestBlocksProps {
  quiz: QuizResponse | undefined;
  isSubmitted: boolean;
  answers: AnswerProps[];
  values: FormFields;
  onChange: (name: string, value: string[]) => void;
  inputRefs: RefObject<HTMLElement[]>;
}

export default function TestBlocks({
  quiz,
  isSubmitted,
  answers,
  values,
  onChange,
  inputRefs,
}: TestBlocksProps) {
  const setInputRef = (element: HTMLElement | null, id: number) => {
    if (inputRefs.current && element) {
      inputRefs.current[id] = element;
    }
  };

  return (
    <>
      {quiz?.questions.map((question, id) => {
        const isQuestionCorrect = answers[id] ? answers[id].isCorrect : false;
        const selected = new Set(values[`${id + 1}`] ?? []);

        return (
          <div
            key={id}
            ref={element => setInputRef(element, id)}
            className="bg-secondary flex flex-col justify-between rounded-sm w-full px-6 py-12 gap-8"
          >
            <div className="flex items-center justify-between font-semibold pb-8">
              <p className="text-lg">{question.question}</p>
              <p className="text-textPrimary">{`${id + 1} / ${quiz.questions.length}`}</p>
            </div>

            <div className="flex flex-col gap-3">
              {question.answers.map(answer => {
                const isSelected = selected.has(answer.text);
                const showCorrectOption = isSubmitted && answer.isCorrect;
                const showIncorrectSelected = isSubmitted && isSelected && !answer.isCorrect;

                return (
                  <label
                    key={answer.text}
                    className={twMerge(
                      "bg-input text-textPrimary rounded-sm w-full py-3 px-4 shadow-md border border-transparent flex items-center gap-3",
                      !isSubmitted && "cursor-pointer",
                      showCorrectOption && "border-green-600",
                      showIncorrectSelected && "border-red-600"
                    )}
                  >
                    <input
                      type="checkbox"
                      checked={isSelected}
                      disabled={isSubmitted}
                      onChange={event => {
                        const current = values[`${id + 1}`] ?? [];
                        const next = event.target.checked
                          ? [...current, answer.text]
                          : current.filter(value => value !== answer.text);

                        onChange(`${id + 1}`, next);
                      }}
                    />
                    <span>{answer.text}</span>
                  </label>
                );
              })}
            </div>

            {isSubmitted && (
              <p className={isQuestionCorrect ? "text-green-500" : "text-red-500"}>
                {isQuestionCorrect ? "Correct" : "Incorrect"}
              </p>
            )}
          </div>
        );
      })}
    </>
  );
}
