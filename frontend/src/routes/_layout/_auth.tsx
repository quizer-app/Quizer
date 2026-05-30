import RequireAuth from "@/components/router/RequireAuth";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_layout/_auth")({
  component: RequireAuth,
});
