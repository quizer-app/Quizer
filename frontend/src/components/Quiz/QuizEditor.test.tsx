import { fireEvent, render, screen, within } from "@testing-library/react";
import { describe, expect, it, vi } from "vitest";
import QuizEditor from "./QuizEditor";
import { QuizResponse } from "@/types/types/quiz";

const quiz: QuizResponse = {
  id: "quiz-1",
  userName: "owner",
  name: "Original quiz",
  slug: "original-quiz",
  location: "owner/original-quiz",
  description: "Original description",
  averageRating: 0,
  numberOfRatings: 0,
  timeLimitInSeconds: null,
  autoSubmitWhenTimeExpires: false,
  maxAttemptsPerUser: null,
  numberOfQuestions: 1,
  imageUrl: "https://example.test/image.png",
  imageId: "image-1",
  createdBy: "user-1",
  updatedBy: "user-1",
  createdAt: "2026-06-01T00:00:00Z",
  updatedAt: "2026-06-01T00:00:00Z",
  questions: [
    {
      id: "question-1",
      question: "What is edited?",
      answers: [
        { text: "Old answer", isCorrect: true },
        { text: "Wrong answer", isCorrect: false },
      ],
    },
  ],
};

const setup = () => {
  const handlers = {
    onSaveQuiz: vi.fn().mockResolvedValue(undefined),
    onSaveQuestion: vi.fn().mockResolvedValue(undefined),
    onCreateQuestion: vi.fn().mockResolvedValue(undefined),
    onDeleteQuestion: vi.fn().mockResolvedValue(undefined),
  };

  render(<QuizEditor quiz={quiz} {...handlers} />);

  return handlers;
};

describe("QuizEditor", () => {
  it("saves quiz details using the update contract", async () => {
    const { onSaveQuiz } = setup();

    fireEvent.change(screen.getByLabelText("Name"), {
      target: { value: "Updated quiz" },
    });
    fireEvent.change(screen.getByLabelText("Description"), {
      target: { value: "Updated description" },
    });
    fireEvent.change(screen.getByLabelText("Time limit in seconds"), {
      target: { value: "90" },
    });
    fireEvent.click(screen.getByLabelText("Auto-submit when the time limit expires"));
    fireEvent.click(screen.getByRole("button", { name: "Save quiz" }));

    expect(onSaveQuiz).toHaveBeenCalledWith({
      name: "Updated quiz",
      description: "Updated description",
      imageId: "image-1",
      timeLimitInSeconds: 90,
      autoSubmitWhenTimeExpires: true,
      maxAttemptsPerUser: null,
    });
  });

  it("saves edited question text and answers", () => {
    const { onSaveQuestion } = setup();
    const questionEditor = screen.getByText("Question 1").closest("article");

    expect(questionEditor).not.toBeNull();
    const editor = within(questionEditor!);

    fireEvent.change(editor.getByLabelText("Question 1"), {
      target: { value: "Updated question" },
    });
    fireEvent.change(editor.getByLabelText("Answer 1"), {
      target: { value: "Updated answer" },
    });
    fireEvent.click(editor.getByRole("button", { name: "Save question" }));

    expect(onSaveQuestion).toHaveBeenCalledWith("question-1", {
      question: "Updated question",
      answers: [
        { text: "Updated answer", isCorrect: true },
        { text: "Wrong answer", isCorrect: false },
      ],
    });
  });

  it("creates a new question with editable answers", () => {
    const { onCreateQuestion } = setup();

    fireEvent.click(screen.getByRole("button", { name: "Add question" }));

    const createButton = screen.getByRole("button", { name: "Create question" });
    const questionEditor = createButton.closest("article");

    expect(questionEditor).not.toBeNull();
    const editor = within(questionEditor!);

    fireEvent.change(editor.getByLabelText("Question 2"), {
      target: { value: "New question" },
    });
    fireEvent.change(editor.getByLabelText("Answer 1"), {
      target: { value: "New wrong answer" },
    });
    fireEvent.change(editor.getByLabelText("Answer 2"), {
      target: { value: "New correct answer" },
    });
    fireEvent.click(createButton);

    expect(onCreateQuestion).toHaveBeenCalledWith({
      question: "New question",
      answers: [
        { text: "New wrong answer", isCorrect: false },
        { text: "New correct answer", isCorrect: true },
      ],
    });
  });

  it("deletes existing questions through the delete endpoint callback", () => {
    const { onDeleteQuestion } = setup();

    fireEvent.click(screen.getByRole("button", { name: "Delete question" }));

    expect(onDeleteQuestion).toHaveBeenCalledWith("question-1");
  });
});
