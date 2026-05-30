import PersistLogin from "@/components/router/PersistLogin";
import { createRootRoute, Outlet } from "@tanstack/react-router";

export const Route = createRootRoute({
  component: () => (
    <PersistLogin>
      <Outlet />
    </PersistLogin>
  ),
});
