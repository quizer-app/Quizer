import QuizTile from "@/components/Home/TileGrid/QuizTile";
import FiltersBar from "@/components/QuizSearch/FiltersBar";
import PagingBar from "@/components/QuizSearch/PagingBar";
import Loading from "@/components/Status/Loading";
import NotFound from "@/components/Status/NotFound/NotFound";
import useQuizesPaging from "@/hooks/quizes/useQuizesPaging";
import { quizSearchSchema } from "@/types/schema/quizSearchSchema";
import { QuizResponse } from "@/types/types/quiz";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_layout/quiz-search")({
  component: QuizSearch,
  validateSearch: quizSearchSchema,
});

function QuizSearch() {
  const searchParams = Route.useSearch();
  const { isLoading, isError, quizes } = useQuizesPaging(searchParams);

  const resetFilters = () => {};

  return (
    <>
      <div className="bg-primary flex items-center justify-center gap-20 p-20">
        <FiltersBar fullPath="/quiz-search" searchParams={searchParams} />
        {quizes && (
          <PagingBar
            quizes={quizes}
            fullPath={Route.fullPath}
            searchParams={searchParams}
          />
        )}
      </div>

      <div className="bg-primary flex-col gap-16 w-full flex items-center justify-center py-14 md:py-16 lg:py-20">
        {isLoading && <Loading />}
        {isError && <NotFound />}
        {quizes?.items.length !== 0 ? (
          <div className="mainContainer grid grid-cols-1 gap-4 lg:gap-6 md:grid-cols-2 xl:grid-cols-3">
            {quizes?.items.map((el: QuizResponse, id: number) => {
              return <QuizTile quiz={el} key={id} />;
            })}
          </div>
        ) : (
          <div className="flex flex-col gap-6">
            <p className="text-center text-white text-3xl font-bold">
              No Quizes Found
            </p>
            <button onClick={resetFilters} className="buttonPrimary mx-auto">
              Reset Filters
            </button>
          </div>
        )}
      </div>
    </>
  );
}
