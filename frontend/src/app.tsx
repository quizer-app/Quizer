import "@/index.css";
import { QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { RouterProvider, createRouter } from "@tanstack/react-router";
import { Provider as JotaiProvider } from "jotai";
import { DevTools } from "jotai-devtools";
import React from "react";
import ReactDOM from "react-dom/client";
import HydrateAtoms from "./atoms/HydrateAtoms";
import queryClient from "./utils/queryClient";

import { routeTree } from "./routeTree.gen";

const router = createRouter({ routeTree });

declare module "@tanstack/react-router" {
  interface Register {
    router: typeof router;
  }
}

if (import.meta.env.PROD) {
  disableReactDevTools();
}

const rootElement = document.getElementById("app")!;
if (!rootElement.innerHTML) {
  const root = ReactDOM.createRoot(rootElement);
  root.render(
    <React.StrictMode>
      <QueryClientProvider client={queryClient}>
        <JotaiProvider>
          {import.meta.env.DEV && <DevTools />}
          <HydrateAtoms>
            {import.meta.env.DEV && <ReactQueryDevtools />}
            <RouterProvider router={router} />
          </HydrateAtoms>
        </JotaiProvider>
      </QueryClientProvider>
    </React.StrictMode>
  );
}
function disableReactDevTools() {
  throw new Error("Function not implemented.");
}
