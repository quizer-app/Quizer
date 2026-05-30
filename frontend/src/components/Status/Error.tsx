import Background from "@/components/layout/Background/Background";
import HomePageButton from "./HomePageButton";

interface ErrorProps {
  msg?: string;
}

export default function Error({ msg }: ErrorProps) {
  return (
    <>
      <Background />
      <div className="text-white w-full h-[80vh] top-0 left-0 flex flex-col gap-16 items-center justify-center font-bold caret-transparent">
        <p className="text-4xl text-center">
          {msg ? msg : "Error has occurred!"}
        </p>
        <HomePageButton />
      </div>
    </>
  );
}
