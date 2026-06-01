import { fireEvent, render, screen } from "@testing-library/react";
import { describe, expect, it, vi } from "vitest";
import { FormInput } from "./FormInput";

describe("FormInput", () => {
  it("links label and input and reports no error semantics by default", () => {
    const onBlur = vi.fn();
    const onChange = vi.fn();

    render(
      <FormInput
        name="email"
        type="text"
        placeholder="Enter your email"
        labelText="Email"
        value=""
        onBlur={onBlur}
        onChange={onChange}
      />
    );

    const input = screen.getByLabelText("Email");
    expect(input).toHaveAttribute("id", "input-email");
    expect(input).toHaveAttribute("aria-invalid", "false");
    expect(screen.queryByRole("alert")).toBeNull();

    fireEvent.blur(input);
    expect(onBlur).toHaveBeenCalledTimes(1);
  });

  it("exposes accessible error semantics when error is present", () => {
    const onBlur = vi.fn();
    const onChange = vi.fn();

    render(
      <FormInput
        name="password"
        type="password"
        placeholder="Enter password"
        labelText="Password"
        value="bad"
        error="Password is required"
        onBlur={onBlur}
        onChange={onChange}
      />
    );

    const input = screen.getByLabelText("Password");
    const alert = screen.getByRole("alert");

    expect(input).toHaveAttribute("aria-invalid", "true");
    expect(input).toHaveAttribute("aria-describedby", "input-password-error");
    expect(alert).toHaveAttribute("id", "input-password-error");
    expect(alert).toHaveTextContent("Password is required");
  });

  it("emits changed value through callback", () => {
    const onBlur = vi.fn();
    const onChange = vi.fn();

    render(
      <FormInput
        name="username"
        type="text"
        placeholder="Enter username"
        labelText="Username"
        value=""
        onBlur={onBlur}
        onChange={onChange}
      />
    );

    fireEvent.change(screen.getByLabelText("Username"), {
      target: { value: "alice" },
    });

    expect(onChange).toHaveBeenCalledWith("alice");
  });
});
