import { api } from "@/api/axios";
import {
  selectIsAuthenticated,
  useAuthStore,
} from "@/atoms/auth";
import { useMutation } from "@tanstack/react-query";

export default function useLogout() {
  const isAuthenticated = useAuthStore(selectIsAuthenticated);
  const setAccessToken = useAuthStore(state => state.setAccessToken);

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
