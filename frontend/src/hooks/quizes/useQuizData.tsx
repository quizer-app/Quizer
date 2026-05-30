import { api } from "@/api/axios";
import { QuizResponse } from "@/types/types/quiz";
import { useQuery } from "@tanstack/react-query";

export default function useQuizData(userName: string, quizSlug: string) {
  const { isLoading, isError, data } = useQuery({
    queryKey: ["quiz", userName, quizSlug],
    queryFn: () =>
      api.get<QuizResponse>(`/api/v1/Quiz/${userName}/${quizSlug}`),
  });
  const quiz = data?.data;

  return { isLoading, isError, quiz };
}
