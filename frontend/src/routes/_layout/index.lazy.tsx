import Features from "@/components/Home/Features/Features";
import Intro from "@/components/Home/Intro/Intro";
import TileGrid from "@/components/Home/TileGrid/TileGrid";
import { createLazyFileRoute } from "@tanstack/react-router";

export const Route = createLazyFileRoute("/_layout/")({
  component: Home,
});

function Home() {
  return (
    <main className="h-[calc(100dvh-var(--header-height,80px))] overflow-y-auto snap-y snap-mandatory overscroll-y-contain">
      <section className="h-[calc(100dvh-var(--header-height,80px))] snap-start snap-always">
        <Intro />
      </section>
      <section className="h-[calc(100dvh-var(--header-height,80px))] snap-start snap-always">
        <Features />
      </section>
      <section className="h-[calc(100dvh-var(--header-height,80px))] snap-start snap-always">
        <TileGrid />
      </section>
    </main>
  );
}
