import { selectIsAuthenticated, selectTokenData, useAuthStore } from "@/atoms/auth";
import useLogout from "@/hooks/auth/useLogout";
import { Link } from "@tanstack/react-router";
import { useTranslation } from "react-i18next";

export default function NavButtons() {
  const { t } = useTranslation();
  const isAuthenticated = useAuthStore(selectIsAuthenticated);
  const tokenData = useAuthStore(selectTokenData);
  const logout = useLogout();

  const username = tokenData?.given_name ? tokenData.given_name : "";

  return (
    <div className="hidden sm:block font-bold z-50">
      {isAuthenticated ? (
        <>
          <Link
            to="/$username"
            params={{
              username: username,
            }}
            search={{
              pageNumber: 1,
              pageSize: 6,
              userName: username,
              tab: 0,
            }}
            className="py-4 px-6 hover:text-textHover"
          >
            {t("nav.dashboard", { name: tokenData?.given_name })}
          </Link>
          <button className="buttonPrimary" onClick={logout} type="button">
            {t("nav.logOut")}
          </button>
        </>
      ) : (
        <>
          <Link to="/signin" className="py-4 px-6 textHover">
            {t("nav.signIn")}
          </Link>
          <Link to="/signup" className="buttonPrimary">
            {t("nav.signUp")}
          </Link>
        </>
      )}
    </div>
  );
}
