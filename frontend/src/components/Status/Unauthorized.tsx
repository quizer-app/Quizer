import Background from "@/components/layout/Background/Background";
import HomePageButton from "./HomePageButton";

export default function Unauthorized() {
  return (
    <>
      <Background />
      <div className="text-white w-full h-[80vh] top-0 left-0 flex flex-col gap-6 items-center justify-center font-bold caret-transparent">
        <p className="text-4xl text-center">401</p>
        <p className="text-xl sm:text-2xl text-textPrimary text-center mb-8 px-4">
          You're not authorized to reach this page
        </p>
        <HomePageButton />
      </div>
    </>
  );
}
