import { api } from "@/api/axios";
import { GetQuizesQueryParams } from "@/types/schema/quizSearchSchema";
import { PaginatedQuizResponse } from "@/types/types/quiz";
import { useQuery } from "@tanstack/react-query";

export default function useQuizesPaging(params: GetQuizesQueryParams) {
  const { isLoading, isError, data } = useQuery({
    queryKey: [
      "quizes",
      Object.getOwnPropertyNames(params),
      Object.values(params),
    ],
    queryFn: () =>
      api.get<PaginatedQuizResponse>("/api/v1/Quiz", {
        params: params,
      }),
  });
  const quizes = data?.data;

  return { isLoading, isError, quizes };
}
