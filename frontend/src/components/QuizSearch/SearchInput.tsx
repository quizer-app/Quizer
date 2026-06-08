import { GetQuizesQueryParams } from "@/types/schema/quizSearchSchema";
import { Link, useNavigate } from "@tanstack/react-router";
import { SearchIcon, XCircle } from "lucide-react";
import { useEffect, useRef, useState } from "react";

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

  const [inputValue, setInputValue] = useState(searchParams.searchTerm);

  useEffect(() => {
    setInputValue(searchParams.searchTerm);
  }, [searchParams.searchTerm]);

  useEffect(() => {
    if (fullPath !== "/quiz-search") {
      return;
    }

    const timeoutId = window.setTimeout(() => {
      if (inputValue === searchParams.searchTerm) {
        return;
      }

      navigate({
        from: fullPath,
        search: { ...searchParams, searchTerm: inputValue, pageNumber: 1 },
        replace: true,
      });
    }, 300);

    return () => {
      window.clearTimeout(timeoutId);
    };
  }, [fullPath, inputValue, navigate, searchParams]);

  const handleChange = (value: string) => {
    setInputValue(value);

    if (fullPath !== "/$username") {
      return;
    }

    navigate({
      from: fullPath,
      search: { ...searchParams, searchTerm: value },
      replace: true,
    });
  };

  const clearQuizSearch = (prev: GetQuizesQueryParams) => {
    return { ...prev, searchTerm: "", pageNumber: 1 };
  };

  const clearUserSearch = () => {
    return { searchTerm: "" };
  };

  return (
    <div className="relative">
      <input
        onChange={e => handleChange(e.target.value)}
        value={inputValue}
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
          <Link from={fullPath} search={clearUserSearch} replace>
            <XCircle
              color="#959CB1"
              className="absolute top-3 right-3 cursor-pointer"
              strokeWidth={1.6}
            />
          </Link>
        ) : (
          <Link from={fullPath} search={clearQuizSearch} replace>
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
