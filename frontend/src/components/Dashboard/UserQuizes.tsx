import { QuizResponse } from "@/types/types/quiz";
import QuizStripe from "./QuizStripe";
import SearchInput from "../QuizSearch/SearchInput";
import useQuizesPaging from "@/hooks/quizes/useQuizesPaging";
import Loading from "../Status/Loading";
import Error from "../Status/Error";
import { GetQuizesQueryParams } from "@/types/schema/quizSearchSchema";
import { Link } from "@tanstack/react-router";

interface UserQuizesProps {
  fullPath: "/$username";
  searchParams: GetQuizesQueryParams;
}

export default function UserQuizes({
  fullPath,
  searchParams,
}: UserQuizesProps) {
  const { isLoading, isError, quizes } = useQuizesPaging(searchParams);

  const increaseQuery = (prev: GetQuizesQueryParams) => {
    return { ...prev, pageSize: prev.pageSize + 3 };
  };

  return (
    <>
      {isLoading && <Loading />}
      {isError && <Error />}
      <SearchInput
        fullPath={fullPath}
        searchParams={{
          ...searchParams,
          searchTerm: searchParams.searchTerm || "",
        }}
      />
      <div className="flex flex-col gap-6">
        {quizes?.items.map((el: QuizResponse, id: number) => {
          return <QuizStripe quiz={el} key={id} />;
        })}
      </div>
      {quizes?.hasNextPage ? (
        <div>
          <Link
            from={fullPath}
            search={increaseQuery}
            className="buttonPrimary"
            replace
          >
            Load More
          </Link>
        </div>
      ) : null}
    </>
  );
}
