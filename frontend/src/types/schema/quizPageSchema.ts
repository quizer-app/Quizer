import { z } from "zod";

export const quizPageSchema = z.object({
  term: z.number().min(1).catch(1),
});

export type QuizPageSearchParams = z.infer<typeof quizPageSchema>;

export type QuizPageFullPath =
  | "/$username/$quizSlug"
  | "/$username/$quizSlug/flashcards";
