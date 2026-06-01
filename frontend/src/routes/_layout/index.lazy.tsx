import Features from "@/components/Home/Features/Features";
import Intro from "@/components/Home/Intro/Intro";
import TileGrid from "@/components/Home/TileGrid/TileGrid";
import { createLazyFileRoute } from "@tanstack/react-router";

export const Route = createLazyFileRoute("/_layout/")({
  component: Home,
});

function Home() {
  return (
    <main className="snap-y snap-mandatory">
      <div className="snap-start">
        <Intro />
      </div>
      <div className="snap-start">
        <Features />
      </div>
      <div className="snap-start">
        <TileGrid />
      </div>
    </main>
  );
}
