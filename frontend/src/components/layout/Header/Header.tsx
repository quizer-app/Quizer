import { useEffect, useRef, useState } from "react";
import DarkModeButton from "./DarkModeButton";
import Logo from "./Logo";
import NavButtons from "./NavButtons";
import NavLinks from "./NavLinks";
import LanguageSwitcher from "./LanguageSwitcher";
import NavToggle from "./NavToggle";

export function Header() {
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const headerRef = useRef<HTMLElement | null>(null);
  const menuId = "primary-navigation";

  const toggleNavbar = () => {
    setIsOpen(previousState => !previousState);
  };

  const closeNavbar = () => {
    setIsOpen(false);
  };

  useEffect(() => {
    const handleEscape = (event: KeyboardEvent) => {
      if (event.key !== "Escape") {
        return;
      }

      closeNavbar();
    };

    window.addEventListener("keydown", handleEscape);
    return () => window.removeEventListener("keydown", handleEscape);
  }, []);

  useEffect(() => {
    const headerElement = headerRef.current;
    if (headerElement === null) {
      return;
    }

    const setHeaderHeight = () => {
      document.documentElement.style.setProperty(
        "--header-height",
        `${headerElement.offsetHeight}px`
      );
    };

    setHeaderHeight();

    const resizeObserver = new ResizeObserver(() => {
      setHeaderHeight();
    });

    resizeObserver.observe(headerElement);
    window.addEventListener("resize", setHeaderHeight);

    return () => {
      resizeObserver.disconnect();
      window.removeEventListener("resize", setHeaderHeight);
    };
  }, []);

  return (
    <nav
      ref={headerRef}
      className="bg-primary text-white sticky top-0 w-full font-primary z-40 border-b border-white/10"
    >
      <div className="flex justify-between items-center gap-2 mx-auto mainContainer py-2 sm:py-3 lg:py-4">
        <div>
          <Logo />
        </div>
        <div className="flex justify-between items-center grow">
          <NavLinks isOpen={isOpen} onClick={closeNavbar} menuId={menuId} />
          <NavButtons />
        </div>
        <div className="flex items-center gap-3 sm:gap-4 lg:gap-5 pl-3">
          <DarkModeButton />
          <LanguageSwitcher />
          <NavToggle toggleNavbar={toggleNavbar} isOpen={isOpen} menuId={menuId} />
        </div>
      </div>
    </nav>
  );
}
