import { useState } from "react";
import DarkModeButton from "./DarkModeButton";
import Logo from "./Logo";
import NavButtons from "./NavButtons";
import NavLinks from "./NavLinks";
import NavToggle from "./NavToggle";

export function Header() {
  const [isOpen, setIsOpen] = useState<boolean>(false);

  const toggleNavbar = () => {
    setIsOpen(!isOpen);
  };

  const closeNavbar = () => {
    setIsOpen(false);
  };

  return (
    <nav className="bg-primary text-white top-0 w-full font-primary -z-30">
      <div className="flex justify-between items-center gap-2 mx-auto mainContainer py-2 sm:py-3 lg:py-4">
        <div>
          <Logo />
        </div>
        <div className="flex justify-between items-center grow">
          <NavLinks isOpen={isOpen} onClick={closeNavbar} />
          <NavButtons />
        </div>
        <div className="flex justify-between items-center">
          <DarkModeButton />
          <NavToggle toggleNavbar={toggleNavbar} isOpen={isOpen} />
        </div>
      </div>
    </nav>
  );
}
