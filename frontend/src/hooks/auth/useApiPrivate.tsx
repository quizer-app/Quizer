import { apiPrivate } from "@/api/axios";
import { AxiosError } from "axios";
import { useAtomValue } from "jotai";
import { useEffect } from "react";
import { accessTokenAtom } from "../../atoms/auth";
import useRefreshToken from "./useRefreshToken";

export default function useApiPrivate() {
  const refresh = useRefreshToken();
  const accessToken = useAtomValue(accessTokenAtom);

  useEffect(() => {
    const requestIntercept = apiPrivate.interceptors.request.use(
      config => {
        if (!config.headers["Authorization"]) {
          config.headers["Authorization"] = `Bearer ${accessToken}`;
        }

        return config;
      },
      (error: AxiosError) => Promise.reject(error)
    );

    const responseIntercept = apiPrivate.interceptors.response.use(
      response => response,
      async (error: AxiosError) => {
        const prevRequest = error.config;
        if (!prevRequest) return Promise.reject(error);
        if (error.response?.status === 401 && !prevRequest._retry) {
          prevRequest._retry = true;
          const newAccessToken = await refresh();
          prevRequest.headers["Authorization"] = `Bearer ${newAccessToken}`;
          return apiPrivate(prevRequest);
        }
        return Promise.reject(error);
      }
    );
    return () => {
      apiPrivate.interceptors.response.eject(responseIntercept);
      apiPrivate.interceptors.request.eject(requestIntercept);
    };
  }, [accessToken, refresh]);

  return apiPrivate;
}
