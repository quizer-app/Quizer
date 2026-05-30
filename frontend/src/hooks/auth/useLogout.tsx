import { api } from "@/api/axios";
import { useMutation } from "@tanstack/react-query";
import { useAtomValue, useSetAtom } from "jotai";
import { accessTokenAtom, isAuthenticatedAtom } from "../../atoms/auth";

export default function useLogout() {
  const isAuthenticated = useAtomValue(isAuthenticatedAtom);
  const setAccessToken = useSetAtom(accessTokenAtom);

  const logoutMutation = useMutation({
    mutationFn: () => {
      return api.delete("/api/v1/Auth/logout", { withCredentials: true });
    },
    onSuccess: () => {
      setAccessToken(null);
    },
  });

  const logout = async () => {
    if (!isAuthenticated) return;
    await logoutMutation.mutateAsync();
  };

  return logout;
}
