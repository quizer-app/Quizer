import { Link } from "@tanstack/react-router";
import { ArrowDown, BadgeCheck, Compass, Sparkles } from "lucide-react";
import { createLazyFileRoute } from "@tanstack/react-router";

export const Route = createLazyFileRoute("/_layout/")({
  component: Home,
});

const fullViewportSectionClass =
  "relative h-[100dvh] min-h-[100dvh] w-full snap-start snap-always overflow-hidden";

export function Home() {
  return (
    <main
      data-testid="home-scroll-root"
      className="h-[100dvh] overflow-y-auto snap-y snap-mandatory overscroll-y-contain bg-primary text-white"
    >
      <section data-testid="home-section-hero" className={`${fullViewportSectionClass} bg-primary`}>
        <div className="mx-auto flex h-full w-full max-w-6xl flex-col justify-center px-5 py-8 sm:px-8 md:px-12">
          <p className="inline-flex w-fit items-center gap-2 rounded-full border border-lightBlue/60 bg-lightBlue/10 px-3 py-1 text-xs font-semibold uppercase tracking-[0.22em] text-lightBlue">
            <Sparkles size={14} aria-hidden />
            New learning experience
          </p>
          <h1 className="mt-6 max-w-4xl text-balance text-[clamp(2rem,6vw,5rem)] font-bold leading-[1.05]">
            Three focused screens built for uninterrupted learning momentum.
          </h1>
          <p className="mt-5 max-w-2xl text-[clamp(1rem,2vw,1.35rem)] text-textPrimary">
            Move through a full-screen narrative: discover, understand, then act. Every panel
            scales naturally across phones, tablets, and wide displays.
          </p>
          <div className="mt-8 flex flex-wrap items-center gap-4">
            <Link to="/quiz-search" search={{ pageNumber: 1, pageSize: 9 }} className="buttonPrimary">
              Start exploring
            </Link>
            <Link
              to="/signup"
              className="rounded-sm border border-white/40 px-6 py-3 text-sm font-semibold transition-colors hover:bg-white/10"
            >
              Create account
            </Link>
          </div>
          <div className="mt-10 inline-flex items-center gap-2 text-sm font-medium text-textPrimary">
            <ArrowDown size={16} aria-hidden />
            Scroll to continue
          </div>
        </div>
      </section>

      <section
        data-testid="home-section-features"
        className={`${fullViewportSectionClass} border-y border-white/10 bg-gradient-to-b from-primary to-primary/95`}
      >
        <div className="mx-auto grid h-full w-full max-w-6xl content-center gap-4 px-5 py-8 sm:grid-cols-2 sm:gap-5 sm:px-8 md:px-12 lg:grid-cols-3">
          {[
            {
              title: "Practice by direction",
              description: "Find quizzes by topic and creator without losing your place.",
              icon: <Compass size={26} className="text-lightBlue" aria-hidden />,
            },
            {
              title: "Visible progress",
              description: "Track where you improve and revisit weak spots with purpose.",
              icon: <BadgeCheck size={26} className="text-lightBlue" aria-hidden />,
            },
            {
              title: "Fast challenge loops",
              description: "Short sessions with immediate feedback to help retention stick.",
              icon: <Sparkles size={26} className="text-lightBlue" aria-hidden />,
            },
          ].map(feature => (
            <article
              key={feature.title}
              className="rounded-xl border border-white/10 bg-white/[0.04] p-5 shadow-sm backdrop-blur-sm"
            >
              {feature.icon}
              <h2 className="mt-4 text-[clamp(1.1rem,2.4vw,1.45rem)] font-semibold">{feature.title}</h2>
              <p className="mt-2 text-sm leading-relaxed text-textPrimary sm:text-base">
                {feature.description}
              </p>
            </article>
          ))}
        </div>
      </section>

      <section data-testid="home-section-cta" className={`${fullViewportSectionClass} bg-primary`}>
        <div className="mx-auto flex h-full w-full max-w-6xl flex-col items-center justify-center px-5 py-8 text-center sm:px-8 md:px-12">
          <p className="rounded-full border border-lightBlue/50 bg-lightBlue/10 px-4 py-1 text-xs font-semibold uppercase tracking-[0.22em] text-lightBlue">
            Build your path
          </p>
          <h2 className="mt-6 max-w-3xl text-balance text-[clamp(1.8rem,5vw,4rem)] font-bold leading-tight">
            Ready to test what you know and create your own quizzes?
          </h2>
          <p className="mt-4 max-w-2xl text-[clamp(1rem,2.2vw,1.25rem)] text-textPrimary">
            Join the community and start learning in full-screen focus mode on any device.
          </p>
          <div className="mt-8 flex flex-wrap justify-center gap-4">
            <Link to="/signup" className="buttonPrimary">
              Create your account
            </Link>
            <Link
              to="/quiz-search"
              search={{ pageNumber: 1, pageSize: 9 }}
              className="inline-flex min-h-14 items-center justify-center rounded-sm border border-white/40 px-6 py-4 text-sm font-semibold transition-colors hover:bg-white/10"
            >
              Browse quizzes
            </Link>
          </div>
        </div>
      </section>
    </main>
  );
}
