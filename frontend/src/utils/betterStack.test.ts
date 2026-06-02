import { afterEach, beforeEach, describe, expect, it, vi } from "vitest";

const logtailInstances = vi.hoisted(() =>
  [] as Array<{
    error: ReturnType<typeof vi.fn>;
    flush: ReturnType<typeof vi.fn>;
    options: unknown;
    token: string;
  }>
);

const LogtailMock = vi.hoisted(() =>
  vi.fn(function (this: {
    error: ReturnType<typeof vi.fn>;
    flush: ReturnType<typeof vi.fn>;
    options: unknown;
    token: string;
  }, token: string, options?: unknown) {
    this.token = token;
    this.options = options;
    this.error = vi.fn(() => Promise.resolve({}));
    this.flush = vi.fn(() => Promise.resolve());
    logtailInstances.push(this);
  })
);

vi.mock("@logtail/browser", () => ({ Logtail: LogtailMock }));

beforeEach(() => {
  vi.resetModules();
  vi.unstubAllEnvs();
  LogtailMock.mockClear();
  logtailInstances.length = 0;
});

afterEach(() => {
  vi.unstubAllEnvs();
});

describe("Better Stack logger", () => {
  it("stays disabled without a source token", async () => {
    const { betterStack, installBetterStackErrorHandlers, logFrontendError } =
      await import("./betterStack");

    const removeHandlers = installBetterStackErrorHandlers();
    logFrontendError("ignored", new Error("ignored"));
    removeHandlers();

    expect(betterStack).toBeNull();
    expect(LogtailMock).not.toHaveBeenCalled();
  });

  it("creates a browser logger with the configured endpoint", async () => {
    vi.stubEnv("VITE_BETTERSTACK_SOURCE_TOKEN", " source-token ");
    vi.stubEnv("VITE_BETTERSTACK_ENDPOINT", " https://example.test ");

    const { betterStack } = await import("./betterStack");

    expect(betterStack).toBe(logtailInstances[0]);
    expect(LogtailMock).toHaveBeenCalledWith("source-token", {
      endpoint: "https://example.test",
    });
  });

  it("captures unhandled browser errors and flushes immediately", async () => {
    vi.stubEnv("VITE_BETTERSTACK_SOURCE_TOKEN", "source-token");

    const { installBetterStackErrorHandlers } = await import("./betterStack");
    const removeHandlers = installBetterStackErrorHandlers();
    const error = new Error("render failed");

    window.dispatchEvent(
      new ErrorEvent("error", {
        message: "render failed",
        filename: "app.tsx",
        lineno: 10,
        colno: 20,
        error,
      })
    );

    expect(logtailInstances[0].error).toHaveBeenCalledWith(
      "Unhandled browser error",
      expect.objectContaining({
        message: "render failed",
        filename: "app.tsx",
        lineno: 10,
        colno: 20,
        error: expect.objectContaining({
          name: "Error",
          message: "render failed",
        }),
      })
    );
    expect(logtailInstances[0].flush).toHaveBeenCalledTimes(1);

    removeHandlers();
  });

  it("captures unhandled promise rejections", async () => {
    vi.stubEnv("VITE_BETTERSTACK_SOURCE_TOKEN", "source-token");

    const { installBetterStackErrorHandlers } = await import("./betterStack");
    const removeHandlers = installBetterStackErrorHandlers();
    const rejection = new Error("request failed");
    const event = new Event("unhandledrejection") as PromiseRejectionEvent;
    Object.defineProperty(event, "reason", { value: rejection });

    window.dispatchEvent(event);

    expect(logtailInstances[0].error).toHaveBeenCalledWith(
      "Unhandled promise rejection",
      {
        reason: expect.objectContaining({
          name: "Error",
          message: "request failed",
        }),
      }
    );
    expect(logtailInstances[0].flush).toHaveBeenCalledTimes(1);

    removeHandlers();
  });
});
