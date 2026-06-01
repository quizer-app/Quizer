import useQuizesPaging from "@/hooks/quizes/useQuizesPaging";
import { QuizResponse } from "@/types/types/quiz";
import Loading from "../../Status/Loading";
import QuizTile from "./QuizTile";

export type TileGridViewProps = {
  quizes: QuizResponse[];
};

export default function TileGrid() {
  const { isLoading, isError, quizes } = useQuizesPaging({
    pageNumber: 1,
    pageSize: 6,
    sortColumn: "createdAt",
    sortOrder: "desc",
    searchTerm: "",
  });

  return (
    <section className="bg-primary h-full w-full">
      {isLoading && <Loading />}
      {isError ? (
        <div className="mainContainer w-full rounded-sm border border-white/10 bg-white/5 p-8 text-center">
          <p className="text-2xl font-bold text-white">Unable to load quizzes right now</p>
          <p className="mt-2 text-textPrimary">
            Try again in a moment or use quiz search to browse available categories.
          </p>
        </div>
      ) : (
        <div className="mainContainer grid h-full grid-cols-1 content-center gap-4 py-4 md:grid-cols-2 md:py-6 lg:gap-6 xl:grid-cols-3">
          {quizes?.items.map((el, id) => {
            return <QuizTile quiz={el} key={id} />;
          })}
        </div>
      )}
    </section>
  );
}
