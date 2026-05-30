import useQuizesPaging from "@/hooks/quizes/useQuizesPaging";
import { QuizResponse } from "@/types/types/quiz";
import Loading from "../../Status/Loading";
import NotFound from "../../Status/NotFound/NotFound";
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
    <section className="bg-primary flex-col gap-16 w-full flex items-center justify-center py-14 md:py-16 lg:py-20">
      {isLoading && <Loading />}
      {isError && <NotFound />}
      <div className="mainContainer grid grid-cols-1 gap-4 lg:gap-6 md:grid-cols-2 xl:grid-cols-3">
        {quizes?.items.map((el, id) => {
          return <QuizTile quiz={el} key={id} />;
        })}
      </div>
    </section>
  );
}
