import { Link } from "@tanstack/react-router";
import Bottom from "./Bottom";
import Top from "./Top";

export default function Intro() {
  return (
    <section className="relative flex min-h-[calc(100svh-80px)] w-full items-center overflow-hidden bg-primary">
      <div className="mx-auto flex w-full max-w-xl flex-col gap-8 px-4 py-20 text-white md:max-w-3xl">
        <h1 className="z-20 text-center text-3xl font-bold sm:text-4xl lg:text-5xl">
          Ignite Your Knowledge with Fun
        </h1>
        <p className="z-20 text-center text-lg font-semibold text-textPrimary lg:text-xl">
          Dive into Quizer for an immersive blend of entertainment and
          education! Explore captivating quizzes across diverse subjects, from
          history to science and beyond. Challenge your mind, expand your
          knowledge, and embark on an exciting journey of discovery.
        </p>
        <div className="z-20 flex flex-col items-center justify-center gap-4 font-bold sm:flex-row">
          <Link
            to="/quiz-search"
            search={{ pageNumber: 1, pageSize: 9 }}
            className="buttonPrimary"
          >
            Explore Quizzes
          </Link>
          <Link
            to="/signup"
            className="rounded-sm border border-white/40 px-8 py-4 text-white transition-colors hover:bg-white/10"
          >
            Create Account
          </Link>
        </div>
      </div>

      <div className="pointer-events-none absolute bottom-0 left-0 opacity-30 lg:opacity-100">
        <Bottom />
      </div>
      <div className="pointer-events-none absolute right-0 top-0 z-10 opacity-30 lg:opacity-100">
        <Top />
      </div>
    </section>
  );
}
