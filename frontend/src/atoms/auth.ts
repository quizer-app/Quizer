import { TokenData } from "@/api/tokenData";
import { create } from "zustand";

interface AuthState {
  accessToken: string | null;
  setAccessToken: (accessToken: string | null) => void;
}

export const useAuthStore = create<AuthState>(set => ({
  accessToken: null,
  setAccessToken: accessToken => set({ accessToken }),
}));

const parseTokenData = (accessToken: string): TokenData | null => {
  const segments = accessToken.split(".");
  if (segments.length < 2) {
    return null;
  }

  try {
    const decodedPayload = atob(segments[1]);
    return JSON.parse(decodedPayload) as TokenData;
  } catch {
    return null;
  }
};

export const selectAccessToken = (state: AuthState) => state.accessToken;
export const selectIsAuthenticated = (state: AuthState) => {
  if (state.accessToken === null) {
    return false;
  }

  return parseTokenData(state.accessToken) !== null;
};

export const selectTokenData = (state: AuthState): TokenData | null => {
  if (state.accessToken === null) {
    return null;
  }

  return parseTokenData(state.accessToken);
};