import { Link } from "@tanstack/react-router";
import SearchInput from "./SearchInput";
import { twMerge } from "tailwind-merge";
import { GetQuizesQueryParams } from "@/types/schema/quizSearchSchema";

interface FiltersBarProps {
  fullPath: "/quiz-search";
  searchParams: GetQuizesQueryParams;
}

export default function FiltersBar({
  fullPath,
  searchParams,
}: FiltersBarProps) {
  const arr: number[] = [1, 3, 6, 9, 12];

  return (
    <>
      <div className="flex flex-col gap-6">
        <SearchInput fullPath={fullPath} searchParams={searchParams} />

        <div className="flex gap-4">
          {arr.map((el, id) => {
            return (
              <Link
                from={fullPath}
                search={prev => {
                  return { ...prev, pageSize: el };
                }}
                key={id}
                className={twMerge(
                  "buttonSecondary w-12 h-12 flex items-center justify-center",
                  searchParams.pageSize === el ? "bg-lightBlue" : "bg-secondary"
                )}
                replace
              >
                {el}
              </Link>
            );
          })}
          {/* <DropDown text="Order By" /> */}
        </div>
      </div>
    </>
  );
}
