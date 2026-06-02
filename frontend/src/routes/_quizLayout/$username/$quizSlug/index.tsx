import { selectTokenData, useAuthStore } from "@/atoms/auth";
import Category from "@/components/Quiz/Category";
import Flashcard from "@/components/Quiz/Flashcards/Flashcard";
import Term from "@/components/Quiz/Term";
import Loading from "@/components/Status/Loading";
import NotFound from "@/components/Status/NotFound/NotFound";
import useQuizData from "@/hooks/quizes/useQuizData";
import { quizPageSchema } from "@/types/schema/quizPageSchema";
import { QuestionResponse } from "@/types/types/quiz";
import { Link, createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_quizLayout/$username/$quizSlug/")({
  component: Quiz,
  validateSearch: quizPageSchema,
});

export default function Quiz() {
  const tokenData = useAuthStore(selectTokenData);
  const { username, quizSlug } = Route.useParams();
  const { isLoading, isError, quiz } = useQuizData(username, quizSlug);
  const { term } = Route.useSearch();
  const canEdit = tokenData?.sub === quiz?.createdBy;
  return (
    <>
      {isLoading && <Loading />}
      {isError && <NotFound />}
      {quiz && (
        <div className="bg-primary w-full text-white px-4 py-10 sm:py-12 md:py-16 lg:py-20">
          <div className="mx-auto rounded-sm max-w-[592px] md:max-w-[720px] lg:max-w-[976px]">
            <div className="mb-10 flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
              <h2 className="text-2xl font-bold xl:text-3xl">{quiz.name}</h2>
              {canEdit && (
                <Link
                  to="/$username/$quizSlug/edit"
                  params={{ username: quiz.userName, quizSlug: quiz.slug }}
                  className="buttonSecondary bg-secondary px-5 py-3 text-center"
                >
                  Edit quiz
                </Link>
              )}
            </div>
            <div className="flex flex-col gap-10 w-full mb-16">
              <Flashcard
                questions={quiz.questions}
                className="h-[280px] sm:h-[320px] md:h-[360px] lg:h-[420px]"
                fullPath="/$username/$quizSlug/"
                term={term}
              />

              <div className="grid gap-2 sm:grid-cols-2 lg:grid-cols-4">
                <Category
                  text="Flashcards"
                  to={`/${quiz.location}/flashcards`}
                  search={{ term }}
                />
                <Category text="Test" to={`/${quiz.location}/test`} />

                <Category text="Coming Soon!" />
                <Category text="Coming Soon!" />
              </div>
            </div>
            <div className="mb-16">
              <div className="flex items-center gap-4 mb-6">
                <div className="w-12 h-12 bg-lightBlue rounded-3xl"></div>
                <p className="font-semibold">{quiz.userName}</p>
              </div>
              <p className="overflow-auto">{quiz.description}</p>
            </div>
            <h3 className="text-xl font-bold mb-10 xl:text-2xl">
              Terms and Definitions
            </h3>
            <div className="flex flex-col gap-5">
              {quiz.questions.map((el: QuestionResponse, id: number) => {
                return (
                  <Term key={id} term={el.question} desc={el.answers[0].text} />
                );
              })}
            </div>
          </div>
        </div>
      )}
    </>
  );
}
