import type { ReactNode } from "react";
import { render, screen } from "@testing-library/react";
import { describe, expect, it, vi } from "vitest";

vi.mock("@tanstack/react-router", async () => {
  const actual = await vi.importActual<object>("@tanstack/react-router");

  return {
    ...actual,
    Link: ({ children, className }: { children: ReactNode; className?: string }) => (
      <a className={className}>{children}</a>
    ),
    createLazyFileRoute: () => (routeOptions: object) => routeOptions,
  };
});

import { Home } from "./index.lazy";

describe("Home route", () => {
  it("renders exactly three full-screen sections", () => {
    render(<Home />);

    const sectionIds = ["home-section-hero", "home-section-features", "home-section-cta"];

    sectionIds.forEach(sectionId => {
      const section = screen.getByTestId(sectionId);
      expect(section.className).toContain("h-[100dvh]");
      expect(section.className).toContain("min-h-[100dvh]");
      expect(section.className).toContain("snap-start");
    });
  });

  it("uses viewport snap scrolling container", () => {
    render(<Home />);

    const scrollRoot = screen.getByTestId("home-scroll-root");
    expect(scrollRoot.className).toContain("h-[100dvh]");
    expect(scrollRoot.className).toContain("snap-y");
    expect(scrollRoot.className).toContain("snap-mandatory");
  });

  it("contains responsive typography and layout classes", () => {
    render(<Home />);

    const heroHeading = screen.getByRole("heading", {
      level: 1,
      name: /three focused screens built for uninterrupted learning momentum/i,
    });
    expect(heroHeading.className).toContain("text-[clamp(2rem,6vw,5rem)]");

    const featureSection = screen.getByTestId("home-section-features").firstElementChild;
    expect(featureSection).not.toBeNull();
    expect(featureSection?.className).toContain("sm:grid-cols-2");
    expect(featureSection?.className).toContain("lg:grid-cols-3");
  });
});
