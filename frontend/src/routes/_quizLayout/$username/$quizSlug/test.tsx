import QuestionNav from "@/components/Quiz/QuestionNav";
import TestBlocks from "@/components/Quiz/TestBlocks";
import Error from "@/components/Status/Error";
import Loading from "@/components/Status/Loading";
import useQuizData from "@/hooks/quizes/useQuizData";
import { createFileRoute } from "@tanstack/react-router";
import { useRef, useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";

export const Route = createFileRoute("/_quizLayout/$username/$quizSlug/test")({
  component: Test,
});

export interface FormFields {
  [key: string]: string;
}

export interface AnswerProps {
  number: number;
  isCorrect: boolean;
}

export default function Test() {
  const { username, quizSlug } = Route.useParams();
  const { isLoading, isError, quiz } = useQuizData(username, quizSlug);
  const {
    register,
    handleSubmit,
    formState: { isSubmitted },
  } = useForm<FormFields>();

  const [answers, setAnswers] = useState<AnswerProps[]>([]);
  const inputRefs = useRef<HTMLElement[]>([]);
  // is state necessarry?
  const [score, setScore] = useState<number>(0);

  const isValid = (answer: string, user: string) => {
    return answer === user;
  };

  const onSubmit: SubmitHandler<FormFields> = data => {
    if (!quiz) return;
    window.scrollTo(0, 0);

    Object.keys(data).forEach(key => {
      const answer = quiz.questions[parseInt(key) - 1].answers[0].text;
      const formAnswer = data[key];
      const isCorrect = isValid(answer, formAnswer);

      if (isCorrect) {
        setScore(prev => prev + 1);
      }
      setAnswers(prev => [
        ...prev,
        { number: parseInt(key), isCorrect: isCorrect },
      ]);
    });
  };

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
              onSubmit={handleSubmit(onSubmit)}
            >
              <TestBlocks
                quiz={quiz}
                isSubmitted={isSubmitted}
                answers={answers}
                register={register}
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
