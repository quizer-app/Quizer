import { useEffect, useState } from "react";
import DarkModeButton from "./DarkModeButton";
import Logo from "./Logo";
import NavButtons from "./NavButtons";
import NavLinks from "./NavLinks";
import NavToggle from "./NavToggle";

export function Header() {
  const [isOpen, setIsOpen] = useState<boolean>(false);
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

  return (
    <nav className="bg-primary text-white sticky top-0 w-full font-primary z-40 border-b border-white/10">
      <div className="flex justify-between items-center gap-2 mx-auto mainContainer py-2 sm:py-3 lg:py-4">
        <div>
          <Logo />
        </div>
        <div className="flex justify-between items-center grow">
          <NavLinks isOpen={isOpen} onClick={closeNavbar} menuId={menuId} />
          <NavButtons />
        </div>
        <div className="flex justify-between items-center">
          <DarkModeButton />
          <NavToggle toggleNavbar={toggleNavbar} isOpen={isOpen} menuId={menuId} />
        </div>
      </div>
    </nav>
  );
}
