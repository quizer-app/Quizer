import Background from "@/components/layout/Background/Background";
import { FadeLoader } from "react-spinners";

export default function Loading() {
  return (
    <>
      <Background />
      <div className="w-full h-[80vh] top-0 left-0 flex items-center justify-center">
        <FadeLoader color="white" />
      </div>
    </>
  );
}
