import { describe, expect, it } from "vitest";
import { selectIsAuthenticated, selectTokenData } from "./auth";

const createToken = (payload: Record<string, unknown>) => {
  const header = btoa(JSON.stringify({ alg: "none", typ: "JWT" }));
  const body = btoa(JSON.stringify(payload));
  return `${header}.${body}.sig`;
};

describe("auth selectors", () => {
  it("treats null token as unauthenticated", () => {
    const state = { accessToken: null, setAccessToken: () => undefined };

    expect(selectIsAuthenticated(state)).toBe(false);
    expect(selectTokenData(state)).toBeNull();
  });

  it("fails closed on malformed token", () => {
    const state = { accessToken: "invalid.token", setAccessToken: () => undefined };

    expect(selectIsAuthenticated(state)).toBe(false);
    expect(selectTokenData(state)).toBeNull();
  });

  it("returns parsed payload for valid token", () => {
    const token = createToken({
      jti: "1",
      sub: "2",
      given_name: "alice",
      email: "alice@example.com",
      permissions: "user",
      exp: 123,
      iss: "issuer",
      aud: "audience",
    });

    const state = { accessToken: token, setAccessToken: () => undefined };

    expect(selectIsAuthenticated(state)).toBe(true);
    expect(selectTokenData(state)).toMatchObject({
      given_name: "alice",
      email: "alice@example.com",
      permissions: "user",
    });
  });
});
