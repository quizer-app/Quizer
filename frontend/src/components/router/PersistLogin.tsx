import { selectIsAuthenticated, useAuthStore } from "@/atoms/auth";
import useRefreshToken from "@/hooks/auth/useRefreshToken";
import { ReactNode, useEffect, useState } from "react";
import Loading from "../Status/Loading";

export default function PersistLogin({ children }: { children: ReactNode }) {
  const [isLoading, setIsLoading] = useState(true);
  const refresh = useRefreshToken();
  const isAuthenticated = useAuthStore(selectIsAuthenticated);

  useEffect(() => {
    const verifyRefreshToken = async () => {
      try {
        await refresh();
      } finally {
        setIsLoading(false);
      }
    };

    if (isAuthenticated) {
      setIsLoading(false);
      return;
    }

    void verifyRefreshToken();
  }, [isAuthenticated, refresh]);

  if (isLoading) {
    return <Loading />;
  }

  return <>{children}</>;
}
