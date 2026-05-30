import QuizLayout from "@/components/layout/QuizLayout";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_quizLayout")({
  component: QuizLayout,
});
