import { selectIsAuthenticated, useAuthStore } from "@/atoms/auth";
import { Navigate, Outlet, useLocation } from "@tanstack/react-router";

export default function RequireAuth() {
  const location = useLocation();
  const isAuthenticated = useAuthStore(selectIsAuthenticated);
  const redirectTo = location.pathname;

  if (isAuthenticated) {
    return <Outlet />;
  }

  return <Navigate to="/signin" search={{ redirect: redirectTo }} replace />;
}
