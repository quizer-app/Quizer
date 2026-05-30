import { api } from "@/api/axios";
import { UserResponse } from "@/types/types/user";
import { useQuery } from "@tanstack/react-query";

export default function useUserData(username: string) {
  const { isLoading, isError, data } = useQuery({
    queryKey: ["user", username],
    queryFn: () => api.get<UserResponse>(`/api/v1/User/${username}`),
  });
  const user = data?.data;

  return { isLoading, isError, user };
}
