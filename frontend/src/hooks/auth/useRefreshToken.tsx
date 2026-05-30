import { api } from "@/api/axios";
import { accessTokenAtom } from "@/atoms/auth";
import { AuthResponse } from "@/types/types/auth";
import { useSetAtom } from "jotai";

export default function useRefreshToken() {
  const setAccessToken = useSetAtom(accessTokenAtom);

  const refresh = async () => {
    const response = await api.post<AuthResponse>(
      "/api/v1/Auth/refresh",
      {},
      {
        withCredentials: true,
      }
    );
    const { accessToken } = response.data;
    setAccessToken(accessToken ?? null);
    return accessToken;
  };

  return refresh;
}
