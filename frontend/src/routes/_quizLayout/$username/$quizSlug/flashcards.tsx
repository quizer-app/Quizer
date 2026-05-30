import Flashcard from "@/components/Quiz/Flashcards/Flashcard";
import Loading from "@/components/Status/Loading";
import NotFound from "@/components/Status/NotFound/NotFound";
import useQuizData from "@/hooks/quizes/useQuizData";
import { quizPageSchema } from "@/types/schema/quizPageSchema";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute(
  "/_quizLayout/$username/$quizSlug/flashcards"
)({
  component: Flashcards,
  validateSearch: quizPageSchema,
});

function Flashcards() {
  const { username, quizSlug } = Route.useParams();
  const { isLoading, isError, quiz } = useQuizData(username, quizSlug);

  const { term } = Route.useSearch();

  if (!quiz) {
    return <NotFound />;
  }

  return (
    <>
      {isLoading && <Loading />}
      {isError && <NotFound />}
      <div className="bg-primary text-white fixed w-full h-[100vh] px-4 py-12 sm:py-16 md:py-20 lg:py-24">
        <div className="mx-auto rounded-sm max-w-[592px] md:max-w-[720px] lg:max-w-[976px]">
          <div className="flex flex-col gap-10 w-full mb-16">
            <Flashcard
              questions={quiz.questions}
              className="h-[420px] sm:h-[460px] md:h-[460px] lg:h-[500px]"
              fullPath="/$username/$quizSlug/flashcards"
              term={term}
            />
          </div>
        </div>
      </div>
    </>
  );
}
