import { Menu, X } from "lucide-react";
import { useTranslation } from "react-i18next";

interface NavToggleProps {
  toggleNavbar: () => void;
  isOpen: boolean;
  menuId: string;
}

export default function NavToggle({ toggleNavbar, isOpen, menuId }: NavToggleProps) {
  const { t } = useTranslation();

  return (
    <div className="flex pl-3 lg:hidden z-50">
      <button
        onClick={toggleNavbar}
        aria-label={isOpen ? t("nav.closeMenu") : t("nav.openMenu")}
        aria-expanded={isOpen}
        aria-controls={menuId}
        type="button"
      >
        {isOpen ? (
          <X className="w-9 h-9" strokeWidth={1.6} />
        ) : (
          <Menu className="w-9 h-9" strokeWidth={1.6} />
        )}
      </button>
    </div>
  );
}
