import { Bike, BookOpenCheck, Lock, Map, Route, Users } from "lucide-react";
import FeatureBlock from "./FeatureBlock";

export default function Features() {
  const data = [
    {
      title: "Education for Every Mind",
      desc: "Curated quizzes for beginners and experts, with clear progression paths that reward consistency over guessing.",
      icon: (
        <BookOpenCheck className="text-lightBlue" size={40} strokeWidth={1.6} />
      ),
    },
    {
      title: "Knowledge Without Boundaries",
      desc: "Browse public quizzes instantly and discover new domains without registration walls or paid unlocks.",
      icon: <Lock className="text-lightBlue" size={40} strokeWidth={1.6} />,
    },
    {
      title: "Shape Your Journey",
      desc: "Track your progress by topic, revisit weak spots, and build momentum with measurable improvement.",
      icon: <Route className="text-lightBlue" size={40} strokeWidth={1.6} />,
    },
    {
      title: "Community of Curiosity",
      desc: "Create your own quizzes, publish them, and learn from a community that values shared discovery.",
      icon: <Users className="text-lightBlue" size={40} strokeWidth={1.6} />,
    },
    {
      title: "Learning That Sticks",
      desc: "Short challenge loops and immediate feedback help you retain knowledge instead of skimming content.",
      icon: <Bike className="text-lightBlue" size={40} strokeWidth={1.6} />,
    },
    {
      title: "Explore by Direction",
      desc: "Jump between topics and creators with smart filtering, then continue exactly where you left off.",
      icon: <Map className="text-lightBlue" size={40} strokeWidth={1.6} />,
    },
  ];

  return (
    <section className="bg-primary/95 h-full w-full overflow-y-auto text-white">
      <div className="mainContainer mx-auto flex h-full flex-col justify-center gap-10 px-4 py-10 md:py-12 lg:py-14">
        <div className="max-w-2xl mx-auto text-center">
          <p className="inline-block rounded-sm border border-lightBlue/50 bg-lightBlue/10 px-3 py-1 text-xs uppercase tracking-[0.24em] text-lightBlue">
            Why Quizer
          </p>
          <h2 className="mt-5 text-3xl font-bold md:text-4xl">Built for focused learning, not passive scrolling</h2>
          <p className="mt-4 text-textPrimary text-base md:text-lg">
            Every part of Quizer is designed to help you practice deliberately, see your growth, and keep learning enjoyable.
          </p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-x-8 gap-y-10">
          {data.map((el, id) => {
            return (
              <FeatureBlock title={el.title} desc={el.desc} key={id}>
                {el.icon}
              </FeatureBlock>
            );
          })}
        </div>
      </div>
    </section>
  );
}
