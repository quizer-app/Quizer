import { useEffect, useRef, useState } from "react";
import useQuizesPaging from "@/hooks/quizes/useQuizesPaging";
import { ShortQuizResponse } from "@/types/types/quiz";
import Loading from "../../Status/Loading";
import QuizTile from "./QuizTile";

export type TileGridViewProps = {
  quizes: ShortQuizResponse[];
};

const getPageSize = (availableHeight: number) => {
  if (availableHeight < 860) {
    return 3;
  }

  return 6;
};

export default function TileGrid() {
  const sectionRef = useRef<HTMLElement | null>(null);
  const [pageSize, setPageSize] = useState<number>(6);

  useEffect(() => {
    const sectionElement = sectionRef.current;
    if (sectionElement === null) {
      return;
    }

    const updatePageSize = () => {
      const nextPageSize = getPageSize(sectionElement.clientHeight);
      setPageSize(previousPageSize =>
        previousPageSize === nextPageSize ? previousPageSize : nextPageSize
      );
    };

    updatePageSize();

    const resizeObserver = new ResizeObserver(updatePageSize);
    resizeObserver.observe(sectionElement);

    return () => {
      resizeObserver.disconnect();
    };
  }, []);

  const { isLoading, isError, quizes } = useQuizesPaging({
    pageNumber: 1,
    pageSize,
    sortColumn: "createdAt",
    sortOrder: "desc",
    searchTerm: "",
  });

  return (
    <section ref={sectionRef} className="h-full w-full overflow-hidden bg-primary">
      {isLoading && <Loading />}
      {isError ? (
        <div className="mainContainer w-full rounded-sm border border-white/10 bg-white/5 p-8 text-center">
          <p className="text-2xl font-bold text-white">Unable to load quizzes right now</p>
          <p className="mt-2 text-textPrimary">
            Try again in a moment or use quiz search to browse available categories.
          </p>
        </div>
      ) : (
        <div className="mainContainer grid h-full grid-cols-1 content-start gap-4 pt-4 pb-1 md:grid-cols-2 md:pt-5 md:pb-2 lg:gap-6 xl:grid-cols-3">
          {quizes?.items.map((el, id) => {
            return <QuizTile quiz={el} key={id} />;
          })}
        </div>
      )}
    </section>
  );
}
