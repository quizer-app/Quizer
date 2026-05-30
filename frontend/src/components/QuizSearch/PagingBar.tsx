import { PaginatedQuizResponse } from "@/types/types/quiz";
import PagingButton from "./PagingButton";
import { useNavigate } from "@tanstack/react-router";
import { GetQuizesQueryParams } from "@/types/schema/quizSearchSchema";

interface PagingBarProps {
  quizes: PaginatedQuizResponse;
  fullPath: "/quiz-search";
  searchParams: GetQuizesQueryParams;
}

export default function PagingBar({
  quizes,
  fullPath,
  searchParams,
}: PagingBarProps) {
  const navigate = useNavigate();
  const elements = Array.from(
    Array(quizes?.totalCount ? quizes.totalCount : 1),
    (_, index) => index + 1
  );

  const newPageNumber = (newPage: number) => {
    navigate({
      from: fullPath,
      search: { ...searchParams, pageNumber: newPage },
      replace: true,
    });
  };

  const setCurrPage = (action: number | "increment" | "decrement") => {
    if (typeof action === "number") {
      newPageNumber(action);
    } else if (
      action === "decrement" &&
      searchParams.pageNumber &&
      quizes.hasPreviousPage
    ) {
      newPageNumber(searchParams.pageNumber - 1);
    } else if (
      action === "increment" &&
      searchParams.pageNumber &&
      quizes.hasNextPage
    ) {
      newPageNumber(searchParams.pageNumber + 1);
    }
  };

  const isDisplayed = (n: number) => {
    let currPage = 1;
    let maxPage = 1;

    if (quizes?.pageNumber) {
      currPage = quizes.pageNumber;
      maxPage = quizes.totalPages;
    }

    if (
      maxPage <= 6 ||
      Math.abs(currPage - n) <= 1 ||
      n === 1 ||
      n === maxPage
    ) {
      return true;
    } else if (currPage === maxPage && n + 2 === maxPage) {
      return true;
    } else if (currPage === 1 && n === 3) {
      return true;
    }

    return false;
  };

  return (
    <div className="flex gap-2 text-white">
      <PagingButton text="PREV" onClick={() => setCurrPage("decrement")} />

      {elements
        .slice(0, quizes?.totalPages ? quizes.totalPages : 1)
        .map((el, id) => {
          return isDisplayed(el) ? (
            <PagingButton
              text={el}
              onClick={() => setCurrPage(el)}
              current={
                quizes?.pageNumber !== null &&
                quizes?.pageNumber !== undefined &&
                id + 1 === quizes.pageNumber
              }
              dots={
                quizes?.pageNumber && quizes.pageNumber >= 4 && el === 1
                  ? "AFTER"
                  : quizes?.pageNumber &&
                      quizes.pageNumber <= quizes.totalPages - 3 &&
                      el === quizes.totalPages
                    ? "BEFORE"
                    : ""
              }
              key={id}
            />
          ) : null;
        })}

      <PagingButton text="NEXT" onClick={() => setCurrPage("increment")} />
    </div>
  );
}
