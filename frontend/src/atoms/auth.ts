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

export const selectAccessToken = (state: AuthState) => state.accessToken;
export const selectIsAuthenticated = (state: AuthState) => state.accessToken !== null;
export const selectTokenData = (state: AuthState): TokenData | null => {
  if (state.accessToken === null) {
    return null;
  }

  const tokenData = JSON.parse(atob(state.accessToken.split(".")[1])) as TokenData;
  return tokenData;
};