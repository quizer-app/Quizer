import Features from "@/components/Home/Features/Features";
import Intro from "@/components/Home/Intro/Intro";
import TileGrid from "@/components/Home/TileGrid/TileGrid";
import { createLazyFileRoute } from "@tanstack/react-router";

export const Route = createLazyFileRoute("/_layout/")({
  component: Home,
});

function Home() {
  return (
    <main className="h-[calc(100svh-80px)] snap-y snap-mandatory overflow-y-auto overscroll-y-contain">
      <div className="h-[calc(100svh-80px)] snap-start snap-always">
        <Intro />
      </div>
      <div className="h-[calc(100svh-80px)] snap-start snap-always">
        <Features />
      </div>
      <div className="h-[calc(100svh-80px)] snap-start snap-always">
        <TileGrid />
      </div>
    </main>
  );
}
