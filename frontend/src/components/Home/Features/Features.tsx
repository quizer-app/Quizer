import FeatureBlock from "./FeatureBlock";
import { Bike, BookOpenCheck, Lock, Map, Route, Users } from "lucide-react";

export default function Features() {
  const data = [
    {
      title: "Education for Every Mind",
      desc: "Our platform offers a diverse range of quizzes designed to cater to learners of all ages and backgrounds, ensuring that education is accessible and enjoyable for everyone.",
      icon: (
        <BookOpenCheck className="text-lightBlue" size={40} strokeWidth={1.6} />
      ),
    },
    {
      title: "Knowledge Without Boundaries",
      desc: "Dive into a treasure trove of quizzes covering various topics, all accessible without any registration or fees. Explore, learn, and expand your mind at your own pace, hassle-free.",
      icon: <Lock className="text-lightBlue" size={40} strokeWidth={1.6} />,
    },
    {
      title: "Shape Your Journey",
      desc: "Create an account to track your progress across quizzes, monitor your strengths, and identify areas for improvement. Plus, unleash your creativity by crafting your own quizzes to share with the community.",
      icon: <Route className="text-lightBlue" size={40} strokeWidth={1.6} />,
    },
    {
      title: "Community of Curiosity",
      desc: "Join our vibrant community to explore, learn, and share knowledge through quizzes. Connect with like-minded learners, dive into diverse topics, and enjoy the journey of discovery together.",
      icon: <Users className="text-lightBlue" size={40} strokeWidth={1.6} />,
    },
    {
      title: "Chasing Dreams in Every Moment",
      desc: "Our community celebrates the pursuit of passions, whether it's through art, travel, cooking, or simply enjoying the beauty of nature. Let your passions guide you to new heights of joy and fulfillment.",
      icon: <Bike className="text-lightBlue" size={40} strokeWidth={1.6} />,
    },
    {
      title: "Navigate Your Adventures",
      desc: "Navigate the exciting world of leisure with LeisureLane! Join our community to discover new hobbies, share your passions, and make the most of your free time. ",
      icon: <Map className="text-lightBlue" size={40} strokeWidth={1.6} />,
    },
  ];

  return (
    <section className="bg-primary/2 w-full text-white">
      <div className="flex flex-col gap-28 py-16 md:py-20 lg:py-28 px-4 mainContainer mx-auto">
        <div className="max-w-md sm:max-w-lg mx-auto">
          <h2 className="font-bold mb-5 text-center text-3xl md:text-4xl">
            Main Features
          </h2>
          <p className="text-textPrimary text-center font-medium md:text-lg">
            Lorem ipsum dolor sit amet consectetur adipisicing elit. Velit eos
            ut architecto consectetur quod autem.
          </p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-x-8 gap-y-14">
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
