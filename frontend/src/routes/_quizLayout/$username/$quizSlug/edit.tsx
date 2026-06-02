import useApiPrivate from "@/hooks/auth/useApiPrivate";
import useQuizData from "@/hooks/quizes/useQuizData";
import { selectIsAuthenticated, selectTokenData, useAuthStore } from "@/atoms/auth";
import QuizEditor from "@/components/Quiz/QuizEditor";
import Error from "@/components/Status/Error";
import Loading from "@/components/Status/Loading";
import Unauthorized from "@/components/Status/Unauthorized";
import {
  CreateQuestionRequest,
  QuizResponse,
  UpdateQuestionRequest,
  UpdateQuizRequest,
} from "@/types/types/quiz";
import { useQueryClient } from "@tanstack/react-query";
import { createFileRoute, Navigate, useNavigate } from "@tanstack/react-router";
import toast from "react-hot-toast";

export const Route = createFileRoute("/_quizLayout/$username/$quizSlug/edit")({
  component: EditQuiz,
});

export default function EditQuiz() {
  const { username, quizSlug } = Route.useParams();
  const { isLoading, isError, quiz } = useQuizData(username, quizSlug);
  const apiPrivate = useApiPrivate();
  const queryClient = useQueryClient();
  const navigate = useNavigate();
  const isAuthenticated = useAuthStore(selectIsAuthenticated);
  const tokenData = useAuthStore(selectTokenData);

  if (!isAuthenticated) {
    return <Navigate to="/signin" replace />;
  }

  if (isLoading) {
    return <Loading />;
  }

  if (isError || !quiz) {
    return <Error />;
  }

  if (tokenData?.sub !== quiz.createdBy) {
    return <Unauthorized />;
  }

  const invalidateQuizQueries = async () => {
    await queryClient.invalidateQueries({ queryKey: ["quiz"] });
    await queryClient.invalidateQueries({ queryKey: ["quizes"] });
  };

  const saveQuiz = async (request: UpdateQuizRequest) => {
    await apiPrivate.put(`/api/v1/Quiz/${quiz.id}`, request);
    const response = await apiPrivate.get<QuizResponse>(`/api/v1/Quiz/${quiz.id}`);
    await invalidateQuizQueries();
    toast.success("Quiz updated");
    await navigate({
      to: "/$username/$quizSlug/edit",
      params: { username: response.data.userName, quizSlug: response.data.slug },
      replace: true,
    });
  };

  const saveQuestion = async (
    questionId: string,
    request: UpdateQuestionRequest
  ) => {
    await apiPrivate.put(`/api/v1/Question/${questionId}`, request);
    await invalidateQuizQueries();
    toast.success("Question updated");
  };

  const createQuestion = async (request: CreateQuestionRequest) => {
    await apiPrivate.post(`/api/v1/Question/${quiz.id}`, request);
    await invalidateQuizQueries();
    toast.success("Question created");
  };

  const deleteQuestion = async (questionId: string) => {
    await apiPrivate.delete(`/api/v1/Question/${questionId}`);
    await invalidateQuizQueries();
    toast.success("Question deleted");
  };

  return (
    <QuizEditor
      quiz={quiz}
      onSaveQuiz={saveQuiz}
      onSaveQuestion={saveQuestion}
      onCreateQuestion={createQuestion}
      onDeleteQuestion={deleteQuestion}
    />
  );
}
