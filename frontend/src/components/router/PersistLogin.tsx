import { selectAccessToken, useAuthStore } from "@/atoms/auth";
import useRefreshToken from "@/hooks/auth/useRefreshToken";
import { ReactNode, useEffect, useState } from "react";
import Loading from "../Status/Loading";

export default function PersistLogin({ children }: { children: ReactNode }) {
  const [isLoading, setIsLoading] = useState(true);
  const refresh = useRefreshToken();
  const accessToken = useAuthStore(selectAccessToken);

  useEffect(() => {
    const verifyRefreshToken = async () => {
      try {
        await refresh();
      } catch (err) {
        if (err instanceof Error) {
          console.log(err.message);
        }
      } finally {
        setIsLoading(false);
      }
    };

    if (accessToken) {
      setIsLoading(false);
      return;
    }

    void verifyRefreshToken();
  }, [refresh, accessToken]);

  return <>{isLoading ? <Loading /> : <>{children}</>}</>;
}
