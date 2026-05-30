import { Navigate, Outlet } from "@tanstack/react-router";
import { useAtomValue } from "jotai";
import { isAuthenticatedAtom } from "../../atoms/auth";

export default function RequireAuth() {
  // const location = useLocation();

  const isAuthenticated = useAtomValue(isAuthenticatedAtom);

  return isAuthenticated ? (
    <Outlet />
  ) : (
    // <Navigate to="/signin" state={{ from: location }} replace />
    <Navigate to="/signin" replace />
  );
}
