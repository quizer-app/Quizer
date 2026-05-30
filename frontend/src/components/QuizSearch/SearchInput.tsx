import { GetQuizesQueryParams } from "@/types/schema/quizSearchSchema";
import { Link, useNavigate } from "@tanstack/react-router";
import { SearchIcon, XCircle } from "lucide-react";
import { useRef } from "react";

interface SearchInputProps {
  fullPath: "/quiz-search";
  searchParams: GetQuizesQueryParams;
}

interface SearchInputProps2 {
  fullPath: "/$username";
  searchParams: { searchTerm: string };
}

export default function SearchInput({
  fullPath,
  searchParams,
}: SearchInputProps | SearchInputProps2) {
  const inputRef = useRef<HTMLInputElement>(null);
  const navigate = useNavigate({ from: fullPath });

  const handleChange = (e: string) => {
    if (fullPath === "/$username") {
      navigate({
        from: fullPath,
        search: { ...searchParams, searchTerm: e },
        replace: true,
      });
    } else if (fullPath === "/quiz-search") {
      navigate({
        from: fullPath,
        search: { ...searchParams, searchTerm: e },
        replace: true,
      });
    }
  };

  const clearInput = (prev: GetQuizesQueryParams) => {
    return { ...prev, searchTerm: "" };
  };

  return (
    <div className="relative">
      <input
        onChange={e => handleChange(e.target.value)}
        value={searchParams.searchTerm}
        ref={inputRef}
        className="bg-secondary w-full rounded-sm p-3 text-textPrimary pl-12 focus:outline-none min-w-99"
        placeholder="Search for quizes..."
      />
      <SearchIcon
        onClick={() => inputRef.current?.focus()}
        color="white"
        className="absolute top-3 left-3 cursor-text"
        strokeWidth={1.6}
      />
      {/* avoid types */}
      {searchParams.searchTerm !== "" &&
        (fullPath === "/$username" ? (
          <Link from={fullPath} search={clearInput} replace>
            <XCircle
              color="#959CB1"
              className="absolute top-3 right-3 cursor-pointer"
              strokeWidth={1.6}
            />
          </Link>
        ) : (
          <Link from={fullPath} search={clearInput} replace>
            <XCircle
              color="#959CB1"
              className="absolute top-3 right-3 cursor-pointer"
              strokeWidth={1.6}
            />
          </Link>
        ))}
    </div>
  );
}
