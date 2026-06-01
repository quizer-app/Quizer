import QuestionNav from "@/components/Quiz/QuestionNav";
import TestBlocks from "@/components/Quiz/TestBlocks";
import Error from "@/components/Status/Error";
import Loading from "@/components/Status/Loading";
import useQuizData from "@/hooks/quizes/useQuizData";
import { useForm } from "@tanstack/react-form";
import { createFileRoute } from "@tanstack/react-router";
import { useMemo, useRef, useState } from "react";

export const Route = createFileRoute("/_quizLayout/$username/$quizSlug/test")({
  component: Test,
});

export interface FormFields {
  [key: string]: string[];
}

export interface AnswerProps {
  number: number;
  isCorrect: boolean;
}

export default function Test() {
  const { username, quizSlug } = Route.useParams();
  const { isLoading, isError, quiz } = useQuizData(username, quizSlug);
  const [isSubmitted, setIsSubmitted] = useState(false);
  const [answers, setAnswers] = useState<AnswerProps[]>([]);
  const inputRefs = useRef<HTMLElement[]>([]);
  const [score, setScore] = useState<number>(0);

  const defaultValues = useMemo<FormFields>(() => {
    if (!quiz) {
      return {};
    }

    return quiz.questions.reduce<FormFields>((accumulator, _, index) => {
      accumulator[String(index + 1)] = [];
      return accumulator;
    }, {});
  }, [quiz]);

  const form = useForm({
    defaultValues,
    onSubmit: ({ value }) => {
      if (!quiz) {
        return;
      }

      window.scrollTo(0, 0);
      let nextScore = 0;
      const nextAnswers = Object.keys(value).map(key => {
        const questionIndex = parseInt(key, 10) - 1;
        const selectedAnswers = new Set(value[key] ?? []);
        const correctAnswers = quiz.questions[questionIndex].answers
          .filter(answer => answer.isCorrect)
          .map(answer => answer.text);

        const isCorrect =
          selectedAnswers.size === correctAnswers.length &&
          correctAnswers.every(answer => selectedAnswers.has(answer));

        if (isCorrect) {
          nextScore += 1;
        }

        return { number: parseInt(key, 10), isCorrect };
      });

      setScore(nextScore);
      setAnswers(nextAnswers);
      setIsSubmitted(true);
    },
  });

  return (
    <>
      {isLoading && <Loading />}
      {isError && <Error />}
      <div className="bg-primary text-white px-4 py-10 sm:py-14 md:py-16 lg:py-20 relative">
        <div className="mx-auto rounded-sm max-w-[592px] md:max-w-[720px] lg:max-w-[976px]">
          <div className="flex flex-col gap-10 w-full mb-16">
            {isSubmitted && (
              <QuestionNav
                answers={answers}
                score={score}
                length={quiz ? quiz.questions.length : 0}
                inputRefs={inputRefs}
              />
            )}
            <form
              className="flex flex-col gap-10 items-center"
              onSubmit={event => {
                event.preventDefault();
                event.stopPropagation();
                void form.handleSubmit();
              }}
            >
              <TestBlocks
                quiz={quiz}
                isSubmitted={isSubmitted}
                answers={answers}
                values={form.state.values}
                onChange={(name, fieldValue) => {
                  form.setFieldValue(name, fieldValue);
                }}
                inputRefs={inputRefs}
              />
              {!isSubmitted && (
                <div className="w-full">
                  <button
                    type="submit"
                    className="bg-lightBlue px-8 py-4 lg:py-5 rounded-sm hover:bg-opacity-95"
                  >
                    Check Answers
                  </button>
                </div>
              )}
            </form>
          </div>
        </div>
      </div>
    </>
  );
}
