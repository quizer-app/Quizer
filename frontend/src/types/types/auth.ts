export type AuthResponse = {
  message: string;
  errors?: ValidationErrors;
  accessToken?: string;
}

type ValidationErrors = {
  [key: string]: ValidationError;
}

type ValidationError = {
  tag: string;
  value: string;
}
