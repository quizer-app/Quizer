import { Link } from "@tanstack/react-router";

export default function Logo() {
  return (
    <Link to="/" className="cursor-pointer">
      <div className="flex items-center justify-center gap-2 lg:gap-3 lg:pb-1 z-50">
        <div className="relative z-50">
          <div className="bg-lightBlue absolute w-6 h-6 lg:w-7 lg:h-7 rounded-[10%_90%_100%_0%_/_0%_100%_0%_100%]" />
          <div className="bg-red-600 dark:bg-darkBlue ml-[7px] w-6 h-6 lg:w-7 lg:h-7 rounded-[90%_10%_0%_100%_/_100%_0%_100%_0%]" />
        </div>
        <p className="font-bold text-2xl sm:text-[1.7rem] lg:text-3xl z-50">
          Quizer
        </p>
      </div>
    </Link>
  );
}
