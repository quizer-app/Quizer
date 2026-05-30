import useRefreshToken from "@/hooks/auth/useRefreshToken";
import { useAtomValue } from "jotai";
import { ReactNode, useEffect, useState } from "react";
import { accessTokenAtom } from "../../atoms/auth";
import Loading from "../Status/Loading";

export default function PersistLogin({ children }: { children: ReactNode }) {
  const [isLoading, setIsLoading] = useState(true);
  const refresh = useRefreshToken();
  const accessToken = useAtomValue(accessTokenAtom);

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

    !accessToken ? verifyRefreshToken() : setIsLoading(false);
  }, [refresh, accessToken]);

  return <>{isLoading ? <Loading /> : <>{children}</>}</>;
}
