import { Logtail } from "@logtail/browser";

type BetterStackContext = Record<string, unknown>;

const sourceToken = readOptionalEnv(import.meta.env.VITE_BETTERSTACK_SOURCE_TOKEN);
const endpoint = readOptionalEnv(import.meta.env.VITE_BETTERSTACK_ENDPOINT);

export const betterStack = sourceToken
  ? new Logtail(sourceToken, endpoint ? { endpoint } : undefined)
  : null;

let installed = false;

export const installBetterStackErrorHandlers = () => {
  if (!betterStack || installed) {
    return () => undefined;
  }

  installed = true;

  const handleError = (event: ErrorEvent) => {
    void betterStack.error("Unhandled browser error", {
      message: event.message,
      filename: event.filename,
      lineno: event.lineno,
      colno: event.colno,
      error: serializeError(event.error),
    });
    void betterStack.flush();
  };

  const handleUnhandledRejection = (event: PromiseRejectionEvent) => {
    void betterStack.error("Unhandled promise rejection", {
      reason: serializeError(event.reason),
    });
    void betterStack.flush();
  };

  window.addEventListener("error", handleError);
  window.addEventListener("unhandledrejection", handleUnhandledRejection);

  return () => {
    window.removeEventListener("error", handleError);
    window.removeEventListener("unhandledrejection", handleUnhandledRejection);
    installed = false;
  };
};

export const logFrontendError = (
  message: string,
  error: unknown,
  context: BetterStackContext = {}
) => {
  if (!betterStack) {
    return;
  }

  void betterStack.error(message, {
    ...context,
    error: serializeError(error),
  });
};

function readOptionalEnv(value: string | undefined) {
  const trimmed = value?.trim();
  return trimmed ? trimmed : undefined;
}

function serializeError(error: unknown) {
  if (!(error instanceof Error)) {
    return error;
  }

  return {
    name: error.name,
    message: error.message,
    stack: error.stack,
  };
}
