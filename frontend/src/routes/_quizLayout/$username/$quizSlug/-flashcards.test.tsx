import { render, screen } from "@testing-library/react";
import { beforeEach, describe, expect, it, vi } from "vitest";

const useQuizDataMock = vi.fn();

vi.mock("@/hooks/quizes/useQuizData", () => ({
  default: (username: string, quizSlug: string) => useQuizDataMock(username, quizSlug),
}));

vi.mock("@/components/Status/Loading", () => ({
  default: () => <div>Loading quiz</div>,
}));

vi.mock("@/components/Status/NotFound/NotFound", () => ({
  default: () => <div>Quiz not found</div>,
}));

vi.mock("@/components/Quiz/Flashcards/Flashcard", () => ({
  default: ({ questions }: { questions: unknown[] }) => (
    <div>Flashcard count: {questions.length}</div>
  ),
}));

vi.mock("@tanstack/react-router", () => ({
  createFileRoute: () => (routeOptions: object) => ({
    ...routeOptions,
    useParams: () => ({ username: "alice", quizSlug: "networking" }),
    useSearch: () => ({ term: undefined }),
  }),
}));

import { Flashcards } from "./flashcards";

describe("Flashcards route", () => {
  beforeEach(() => {
    useQuizDataMock.mockReset();
  });

  it("shows loading before treating a missing quiz as not found", () => {
    useQuizDataMock.mockReturnValue({ isLoading: true, isError: false, quiz: undefined });

    render(<Flashcards />);

    expect(screen.getByText("Loading quiz")).toBeInTheDocument();
    expect(screen.queryByText("Quiz not found")).not.toBeInTheDocument();
  });

  it("renders flashcards after data loads", () => {
    useQuizDataMock.mockReturnValue({
      isLoading: false,
      isError: false,
      quiz: { questions: [{ id: "q1" }, { id: "q2" }] },
    });

    render(<Flashcards />);

    expect(screen.getByText("Flashcard count: 2")).toBeInTheDocument();
  });
});
