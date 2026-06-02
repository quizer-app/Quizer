import {
  Answer,
  CreateQuestionRequest,
  QuestionResponse,
  QuizResponse,
  UpdateQuestionRequest,
  UpdateQuizRequest,
} from "@/types/types/quiz";
import { FormEvent, useEffect, useMemo, useState } from "react";

interface QuizEditorProps {
  quiz: QuizResponse;
  onSaveQuiz: (request: UpdateQuizRequest) => Promise<void>;
  onSaveQuestion: (
    questionId: string,
    request: UpdateQuestionRequest
  ) => Promise<void>;
  onCreateQuestion: (request: CreateQuestionRequest) => Promise<void>;
  onDeleteQuestion: (questionId: string) => Promise<void>;
}

type QuestionDraft = {
  id: string | null;
  question: string;
  answers: Answer[];
};

const emptyAnswer = (): Answer => ({ text: "", isCorrect: false });

const createDraft = (question?: QuestionResponse): QuestionDraft => {
  if (!question) {
    return {
      id: null,
      question: "",
      answers: [emptyAnswer(), { text: "", isCorrect: true }],
    };
  }

  return {
    id: question.id,
    question: question.question,
    answers: question.answers.map(answer => ({ ...answer })),
  };
};

const toPositiveNumberOrNull = (value: string) => {
  if (value.trim() === "") {
    return null;
  }

  const parsed = Number(value);
  if (!Number.isFinite(parsed) || parsed <= 0) {
    return null;
  }

  return Math.trunc(parsed);
};

const hasValidPositiveNumber = (value: string) => {
  if (value.trim() === "") {
    return true;
  }

  const parsed = Number(value);
  return Number.isFinite(parsed) && parsed > 0;
};

const toRequestAnswers = (answers: Answer[]) =>
  answers
    .filter(answer => answer.text.trim().length > 0)
    .map(answer => ({ text: answer.text.trim(), isCorrect: answer.isCorrect }));

const isQuestionValid = (draft: QuestionDraft) => {
  const answers = toRequestAnswers(draft.answers);

  if (draft.question.trim().length === 0) {
    return false;
  }

  if (answers.length === 0) {
    return false;
  }

  return answers.some(answer => answer.isCorrect);
};

export default function QuizEditor({
  quiz,
  onSaveQuiz,
  onSaveQuestion,
  onCreateQuestion,
  onDeleteQuestion,
}: QuizEditorProps) {
  const [name, setName] = useState(quiz.name);
  const [description, setDescription] = useState(quiz.description);
  const [timeLimit, setTimeLimit] = useState(
    quiz.timeLimitInSeconds?.toString() ?? ""
  );
  const [autoSubmit, setAutoSubmit] = useState(quiz.autoSubmitWhenTimeExpires);
  const [maxAttempts, setMaxAttempts] = useState(
    quiz.maxAttemptsPerUser?.toString() ?? ""
  );
  const [questions, setQuestions] = useState<QuestionDraft[]>(() =>
    quiz.questions.map(createDraft)
  );
  const [error, setError] = useState<string | null>(null);
  const [pendingKey, setPendingKey] = useState<string | null>(null);
  useEffect(() => {
    setName(quiz.name);
    setDescription(quiz.description);
    setTimeLimit(quiz.timeLimitInSeconds?.toString() ?? "");
    setAutoSubmit(quiz.autoSubmitWhenTimeExpires);
    setMaxAttempts(quiz.maxAttemptsPerUser?.toString() ?? "");
    setQuestions(quiz.questions.map(createDraft));
  }, [quiz]);

  const canSaveQuiz = useMemo(() => {
    if (name.trim().length === 0 || description.trim().length === 0) {
      return false;
    }

    if (!hasValidPositiveNumber(timeLimit) || !hasValidPositiveNumber(maxAttempts)) {
      return false;
    }

    if (autoSubmit && timeLimit.trim().length === 0) {
      return false;
    }

    return true;
  }, [autoSubmit, description, maxAttempts, name, timeLimit]);

  const updateQuestion = (index: number, value: string) => {
    setQuestions(current =>
      current.map((question, questionIndex) => {
        if (questionIndex !== index) {
          return question;
        }

        return { ...question, question: value };
      })
    );
  };

  const updateAnswerText = (
    questionIndex: number,
    answerIndex: number,
    value: string
  ) => {
    setQuestions(current =>
      current.map((question, currentQuestionIndex) => {
        if (currentQuestionIndex !== questionIndex) {
          return question;
        }

        return {
          ...question,
          answers: question.answers.map((answer, currentAnswerIndex) => {
            if (currentAnswerIndex !== answerIndex) {
              return answer;
            }

            return { ...answer, text: value };
          }),
        };
      })
    );
  };

  const updateAnswerCorrectness = (
    questionIndex: number,
    answerIndex: number,
    isCorrect: boolean
  ) => {
    setQuestions(current =>
      current.map((question, currentQuestionIndex) => {
        if (currentQuestionIndex !== questionIndex) {
          return question;
        }

        return {
          ...question,
          answers: question.answers.map((answer, currentAnswerIndex) => {
            if (currentAnswerIndex !== answerIndex) {
              return answer;
            }

            return { ...answer, isCorrect };
          }),
        };
      })
    );
  };

  const addAnswer = (questionIndex: number) => {
    setQuestions(current =>
      current.map((question, currentQuestionIndex) => {
        if (currentQuestionIndex !== questionIndex) {
          return question;
        }

        return { ...question, answers: [...question.answers, emptyAnswer()] };
      })
    );
  };

  const removeAnswer = (questionIndex: number, answerIndex: number) => {
    setQuestions(current =>
      current.map((question, currentQuestionIndex) => {
        if (currentQuestionIndex !== questionIndex) {
          return question;
        }

        return {
          ...question,
          answers: question.answers.filter((_, index) => index !== answerIndex),
        };
      })
    );
  };

  const saveQuiz = async (event: FormEvent) => {
    event.preventDefault();
    setError(null);

    if (!canSaveQuiz) {
      setError("Quiz name, description, and timed settings must be valid.");
      return;
    }

    setPendingKey("quiz");
    try {
      await onSaveQuiz({
        name: name.trim(),
        description: description.trim(),
        imageId: quiz.imageId,
        timeLimitInSeconds: toPositiveNumberOrNull(timeLimit),
        autoSubmitWhenTimeExpires: autoSubmit,
        maxAttemptsPerUser: toPositiveNumberOrNull(maxAttempts),
      });
    } catch {
      setError("Unable to save quiz changes.");
    } finally {
      setPendingKey(null);
    }
  };

  const saveQuestion = async (draft: QuestionDraft, index: number) => {
    setError(null);

    if (!isQuestionValid(draft)) {
      setError("Questions need text and at least one correct answer.");
      return;
    }

    const request = {
      question: draft.question.trim(),
      answers: toRequestAnswers(draft.answers),
    };

    const pendingQuestionKey = draft.id ?? `new-${index}`;
    setPendingKey(pendingQuestionKey);
    try {
      if (draft.id) {
        await onSaveQuestion(draft.id, request);
        return;
      }

      await onCreateQuestion(request);
      setQuestions(current => current.filter((_, currentIndex) => currentIndex !== index));
    } catch {
      setError("Unable to save question changes.");
    } finally {
      setPendingKey(null);
    }
  };

  const deleteQuestion = async (draft: QuestionDraft, index: number) => {
    setError(null);

    if (!draft.id) {
      setQuestions(current => current.filter((_, currentIndex) => currentIndex !== index));
      return;
    }

    setPendingKey(draft.id);
    try {
      await onDeleteQuestion(draft.id);
      setQuestions(current => current.filter((_, currentIndex) => currentIndex !== index));
    } catch {
      setError("Unable to delete question.");
    } finally {
      setPendingKey(null);
    }
  };

  return (
    <div className="bg-primary min-h-screen w-full text-white px-4 py-10 sm:py-12 md:py-16">
      <div className="mx-auto flex max-w-[976px] flex-col gap-10">
        <div>
          <p className="text-textPrimary font-semibold">Quiz editor</p>
          <h1 className="text-3xl font-bold">Edit {quiz.name}</h1>
        </div>

        {error && (
          <p className="rounded-sm bg-red-950/50 p-4 text-red-200" role="alert">
            {error}
          </p>
        )}

        <form
          onSubmit={saveQuiz}
          className="flex flex-col gap-5 rounded-sm bg-secondary p-6 shadow-md"
          aria-label="Edit quiz details"
        >
          <label className="flex flex-col gap-2 font-semibold">
            Name
            <input
              value={name}
              onChange={event => setName(event.target.value)}
              className="rounded-sm bg-input px-4 py-3 text-textPrimary outline-none focus:border-lightBlue border border-transparent"
            />
          </label>

          <label className="flex flex-col gap-2 font-semibold">
            Description
            <textarea
              value={description}
              onChange={event => setDescription(event.target.value)}
              rows={5}
              className="resize-none rounded-sm bg-input px-4 py-3 text-textPrimary outline-none focus:border-lightBlue border border-transparent"
            />
          </label>

          <div className="grid gap-5 md:grid-cols-2">
            <label className="flex flex-col gap-2 font-semibold">
              Time limit in seconds
              <input
                value={timeLimit}
                onChange={event => setTimeLimit(event.target.value)}
                inputMode="numeric"
                className="rounded-sm bg-input px-4 py-3 text-textPrimary outline-none focus:border-lightBlue border border-transparent"
              />
            </label>

            <label className="flex flex-col gap-2 font-semibold">
              Max attempts per user
              <input
                value={maxAttempts}
                onChange={event => setMaxAttempts(event.target.value)}
                inputMode="numeric"
                className="rounded-sm bg-input px-4 py-3 text-textPrimary outline-none focus:border-lightBlue border border-transparent"
              />
            </label>
          </div>

          <label className="flex items-center gap-3 text-textPrimary">
            <input
              type="checkbox"
              checked={autoSubmit}
              onChange={event => setAutoSubmit(event.target.checked)}
            />
            Auto-submit when the time limit expires
          </label>

          <button
            type="submit"
            disabled={!canSaveQuiz || pendingKey === "quiz"}
            className="buttonPrimary disabled:cursor-not-allowed disabled:opacity-60"
          >
            {pendingKey === "quiz" ? "Saving quiz..." : "Save quiz"}
          </button>
        </form>

        <section className="flex flex-col gap-5" aria-label="Edit questions">
          <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
            <h2 className="text-2xl font-bold">Questions and answers</h2>
            <button
              type="button"
              onClick={() => setQuestions(current => [...current, createDraft()])}
              className="buttonSecondary bg-secondary px-5 py-3"
            >
              Add question
            </button>
          </div>

          {questions.map((question, questionIndex) => {
            const pendingQuestionKey = question.id ?? `new-${questionIndex}`;
            return (
              <article
                key={question.id ?? `new-question-${questionIndex}`}
                className="flex flex-col gap-5 rounded-sm bg-secondary p-6 shadow-md"
              >
                <label className="flex flex-col gap-2 font-semibold">
                  Question {questionIndex + 1}
                  <textarea
                    value={question.question}
                    onChange={event =>
                      updateQuestion(questionIndex, event.target.value)
                    }
                    rows={3}
                    className="resize-none rounded-sm bg-input px-4 py-3 text-textPrimary outline-none focus:border-lightBlue border border-transparent"
                  />
                </label>

                <div className="flex flex-col gap-3">
                  {question.answers.map((answer, answerIndex) => (
                    <div
                      key={answerIndex}
                      className="grid gap-3 rounded-sm bg-primary/40 p-3 md:grid-cols-[1fr_auto_auto] md:items-center"
                    >
                      <label className="flex flex-col gap-2 text-sm font-semibold">
                        Answer {answerIndex + 1}
                        <input
                          value={answer.text}
                          onChange={event =>
                            updateAnswerText(
                              questionIndex,
                              answerIndex,
                              event.target.value
                            )
                          }
                          className="rounded-sm bg-input px-4 py-3 text-textPrimary outline-none focus:border-lightBlue border border-transparent"
                        />
                      </label>

                      <label className="flex items-center gap-2 text-textPrimary">
                        <input
                          type="checkbox"
                          checked={answer.isCorrect}
                          onChange={event =>
                            updateAnswerCorrectness(
                              questionIndex,
                              answerIndex,
                              event.target.checked
                            )
                          }
                        />
                        Correct
                      </label>

                      <button
                        type="button"
                        onClick={() => removeAnswer(questionIndex, answerIndex)}
                        className="buttonSecondary px-4 py-2 text-sm"
                      >
                        Remove answer
                      </button>
                    </div>
                  ))}
                </div>

                <div className="flex flex-col gap-3 sm:flex-row">
                  <button
                    type="button"
                    onClick={() => addAnswer(questionIndex)}
                    className="buttonSecondary px-4 py-2"
                  >
                    Add answer
                  </button>
                  <button
                    type="button"
                    onClick={() => saveQuestion(question, questionIndex)}
                    disabled={pendingKey === pendingQuestionKey}
                    className="buttonPrimary disabled:cursor-not-allowed disabled:opacity-60"
                  >
                    {pendingKey === pendingQuestionKey
                      ? "Saving question..."
                      : question.id
                        ? "Save question"
                        : "Create question"}
                  </button>
                  <button
                    type="button"
                    onClick={() => deleteQuestion(question, questionIndex)}
                    disabled={pendingKey === pendingQuestionKey}
                    className="buttonSecondary px-4 py-2"
                  >
                    {question.id ? "Delete question" : "Discard question"}
                  </button>
                </div>
              </article>
            );
          })}
        </section>
      </div>
    </div>
  );
}
