import type { ReactNode } from "react";
import { fireEvent, render, screen } from "@testing-library/react";
import { beforeEach, describe, expect, it, vi } from "vitest";
import SearchInput from "./SearchInput";

const navigateMock = vi.fn();

vi.mock("@tanstack/react-router", () => ({
  Link: ({ children }: { children: ReactNode }) => <a>{children}</a>,
  useNavigate: () => navigateMock,
}));

describe("SearchInput", () => {
  beforeEach(() => {
    navigateMock.mockReset();
  });

  it("debounces quiz search navigation", () => {
    vi.useFakeTimers();

    render(
      <SearchInput
        fullPath="/quiz-search"
        searchParams={{ pageNumber: 5, pageSize: 9, searchTerm: "" }}
      />
    );

    const input = screen.getByPlaceholderText("Search for quizes...");
    fireEvent.change(input, { target: { value: "history" } });

    expect(navigateMock).not.toHaveBeenCalled();

    vi.advanceTimersByTime(299);
    expect(navigateMock).not.toHaveBeenCalled();

    vi.advanceTimersByTime(1);
    expect(navigateMock).toHaveBeenCalledTimes(1);
    expect(navigateMock).toHaveBeenCalledWith({
      from: "/quiz-search",
      search: { pageNumber: 1, pageSize: 9, searchTerm: "history" },
      replace: true,
    });

    vi.useRealTimers();
  });

  it("keeps username search immediate", () => {
    render(<SearchInput fullPath="/$username" searchParams={{ searchTerm: "" }} />);

    const input = screen.getByPlaceholderText("Search for quizes...");
    fireEvent.change(input, { target: { value: "math" } });

    expect(navigateMock).toHaveBeenCalledWith({
      from: "/$username",
      search: { searchTerm: "math" },
      replace: true,
    });
  });
});
