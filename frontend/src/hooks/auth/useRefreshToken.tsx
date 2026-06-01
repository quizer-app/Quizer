import { api } from "@/api/axios";
import { useAuthStore } from "@/atoms/auth";
import { AuthResponse } from "@/types/types/auth";

export default function useRefreshToken() {
  const setAccessToken = useAuthStore(state => state.setAccessToken);

  const refresh = async (): Promise<string | null> => {
    const response = await api.post<AuthResponse>(
      "/api/v1/Auth/refresh",
      {},
      {
        withCredentials: true,
      }
    );
    const { accessToken } = response.data;
    const nextAccessToken = accessToken ?? null;
    setAccessToken(nextAccessToken);
    return nextAccessToken;
  };

  return refresh;
}
