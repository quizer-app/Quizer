import { Menu, X } from "lucide-react";

interface NavToggleProps {
  toggleNavbar: () => void;
  isOpen: boolean;
}

export default function NavToggle({ toggleNavbar, isOpen }: NavToggleProps) {
  return (
    <div className="flex pl-3 lg:hidden z-50">
      <button onClick={toggleNavbar}>
        {isOpen ? (
          <X className="w-9 h-9" strokeWidth={1.6} />
        ) : (
          <Menu className="w-9 h-9" strokeWidth={1.6} />
        )}
      </button>
    </div>
  );
}
