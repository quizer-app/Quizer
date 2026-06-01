import { selectIsAuthenticated, useAuthStore } from "@/atoms/auth";
import { Navigate, Outlet } from "@tanstack/react-router";

export default function RequireAuth() {
  // const location = useLocation();

  const isAuthenticated = useAuthStore(selectIsAuthenticated);

  return isAuthenticated ? (
    <Outlet />
  ) : (
    // <Navigate to="/signin" state={{ from: location }} replace />
    <Navigate to="/signin" replace />
  );
}
