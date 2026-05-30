import { QuizResponse } from "@/types/types/quiz";
import { Link } from "@tanstack/react-router";

interface QuizStripeProps {
  quiz: QuizResponse;
}

export default function QuizStripe({ quiz }: QuizStripeProps) {
  return (
    <Link
      to="/$username/$quizSlug"
      params={{ username: quiz.userName, quizSlug: quiz.slug }}
      search={{ term: 1 }}
      className="w-full text-white bg-secondary hover:opacity-90 flex flex-col rounded-sm py-5 px-6 gap-3"
    >
      <p className="text-2xl font-bold">{quiz.name}</p>

      <div className="flex gap-6">
        <p className="font-bold text-textPrimary">{`${quiz.numberOfQuestions} questions`}</p>
        <span className="h-auto w-[1px] bg-textPrimary/50"></span>
        <p className="text-textPrimary">{quiz.createdAt.slice(0, 10)}</p>
      </div>
    </Link>
  );
}
