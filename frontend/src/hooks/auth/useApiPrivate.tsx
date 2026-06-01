import { apiPrivate, setAuthorizationHeader } from "@/api/axios";
import { selectAccessToken, useAuthStore } from "@/atoms/auth";
import { AxiosError, AxiosHeaders, InternalAxiosRequestConfig } from "axios";
import { useEffect } from "react";
import useRefreshToken from "./useRefreshToken";

const withAuthorization = (
  config: InternalAxiosRequestConfig,
  accessToken: string
) => {
  const headers = AxiosHeaders.from(config.headers);
  setAuthorizationHeader(headers, accessToken);
  config.headers = headers;
};

export default function useApiPrivate() {
  const refresh = useRefreshToken();
  const accessToken = useAuthStore(selectAccessToken);

  useEffect(() => {
    const requestIntercept = apiPrivate.interceptors.request.use(
      config => {
        if (!accessToken || config.headers.Authorization) {
          return config;
        }

        withAuthorization(config, accessToken);
        return config;
      },
      (error: AxiosError) => Promise.reject(error)
    );

    const responseIntercept = apiPrivate.interceptors.response.use(
      response => response,
      async (error: AxiosError) => {
        const prevRequest = error.config;
        if (!prevRequest) {
          return Promise.reject(error);
        }

        if (error.response?.status !== 401 || prevRequest._retry) {
          return Promise.reject(error);
        }

        prevRequest._retry = true;
        const newAccessToken = await refresh();
        if (!newAccessToken) {
          return Promise.reject(error);
        }

        withAuthorization(prevRequest, newAccessToken);
        return apiPrivate(prevRequest);
      }
    );

    return () => {
      apiPrivate.interceptors.response.eject(responseIntercept);
      apiPrivate.interceptors.request.eject(requestIntercept);
    };
  }, [accessToken, refresh]);

  return apiPrivate;
}
