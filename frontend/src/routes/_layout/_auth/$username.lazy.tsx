import UserInfo from "@/components/Dashboard/UserInfo";
import UserQuizes from "@/components/Dashboard/UserQuizes";
import { quizSearchSchema } from "@/types/schema/quizSearchSchema";
import { Link, createFileRoute } from "@tanstack/react-router";
import { twMerge } from "tailwind-merge";

export const Route = createFileRoute("/_layout/_auth/$username")({
  component: Dashboard,
  validateSearch: quizSearchSchema,
});

export default function Dashboard() {
  const searchParams = Route.useSearch();
  const { username } = Route.useParams();
  const tabs = ["Information", "Quizes", "Activity"];

  return (
    <>
      <div className="bg-primary w-full text-white">
        <div className="flex flex-col gap-14 py-16 md:py-20 lg:py-28 px-4 items-end mainContainer mx-auto justify-between md:flex-row">
          <div className="flex gap-8 items-center">
            <div className="w-16 h-16 rounded-full bg-lightBlue"></div>
            <p className="text-3xl font-bold">{username}</p>
          </div>

          <div className="flex flex-col gap-5 sm:flex-row">
            {tabs.map((tab, id) => {
              return (
                <Link
                  to="/$username"
                  params={{ username: username }}
                  search={prev => ({
                    ...prev,
                    tab: id,
                  })}
                  key={id}
                  className={twMerge(
                    "buttonSecondary bg-secondary px-5 py-3",
                    searchParams.tab === id && "bg-lightBlue"
                  )}
                  replace
                >
                  {tab}
                </Link>
              );
            })}
          </div>
        </div>
        {searchParams.tab === 0 && (
          <div className="mainContainer mx-auto">
            <UserInfo username={username} />
          </div>
        )}
        {searchParams.tab === 1 && (
          <div className="flex flex-col gap-10 mainContainer mx-auto">
            <UserQuizes fullPath={Route.fullPath} searchParams={searchParams} />
          </div>
        )}
      </div>
    </>
  );
}
