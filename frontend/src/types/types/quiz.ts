export type PaginatedQuizResponse = {
  items: ShortQuizResponse[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
};

export type ShortQuizResponse = {
  id: string;
  userName: string;
  name: string;
  slug: string;
  location: string;
  description: string;
  averageRating: number;
  numberOfRatings: number;
  timeLimitInSeconds: number | null;
  autoSubmitWhenTimeExpires: boolean;
  maxAttemptsPerUser: number | null;
  numberOfQuestions: number;
  imageUrl: string;
  imageId: string;
  createdBy: string;
  updatedBy: string;
  createdAt: string;
  updatedAt: string;
};

export type QuizResponse = ShortQuizResponse & {
  questions: QuestionResponse[];
};

export type QuestionResponse = {
  id: string;
  question: string;
  answers: Answer[];
};

export type Answer = {
  text: string;
  isCorrect: boolean;
};

export type UpdateQuizRequest = {
  name: string;
  description: string;
  imageId: string;
  timeLimitInSeconds: number | null;
  autoSubmitWhenTimeExpires: boolean;
  maxAttemptsPerUser: number | null;
};

export type UpdateQuestionRequest = {
  question: string;
  answers: Answer[];
};

export type CreateQuestionRequest = UpdateQuestionRequest;
