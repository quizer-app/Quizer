import axios, { AxiosHeaders } from "axios";

export const baseURL =
  (import.meta.env.VITE_API_URL as string) ?? "https://api.local.elotoja.com";

export const api = axios.create({
  baseURL,
  headers: {
    "Content-Type": "application/json",
  },
});
export const apiPrivate = axios.create({
  baseURL,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});
export const setAuthorizationHeader = (
  headers: AxiosHeaders,
  accessToken: string
) => {
  headers.set("Authorization", `Bearer ${accessToken}`);
};