import { fireEvent, render, screen } from "@testing-library/react";
import { describe, expect, it, vi } from "vitest";
import NavToggle from "./NavToggle";

describe("NavToggle", () => {
  it("exposes collapsed menu accessibility metadata", () => {
    const toggleNavbar = vi.fn();

    render(
      <NavToggle toggleNavbar={toggleNavbar} isOpen={false} menuId="primary-navigation" />
    );

    const button = screen.getByRole("button", { name: "Open navigation menu" });
    expect(button).toHaveAttribute("aria-expanded", "false");
    expect(button).toHaveAttribute("aria-controls", "primary-navigation");
  });

  it("exposes expanded menu accessibility metadata and click behavior", () => {
    const toggleNavbar = vi.fn();

    render(
      <NavToggle toggleNavbar={toggleNavbar} isOpen={true} menuId="primary-navigation" />
    );

    const button = screen.getByRole("button", { name: "Close navigation menu" });
    expect(button).toHaveAttribute("aria-expanded", "true");

    fireEvent.click(button);
    expect(toggleNavbar).toHaveBeenCalledTimes(1);
  });
});
