export type PaginatedQuizResponse = {
  items: QuizResponse[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
};

export type QuizResponse = {
  id: string;
  userName: string;
  name: string;
  slug: string;
  location: string;
  description: string;
  averageRating: number;
  numberOfRatings: number;
  numberOfQuestions: number;
  imageUrl: string;
  imageId: string;
  userId: string;
  questions: QuestionResponse[];
  createdAt: string;
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
