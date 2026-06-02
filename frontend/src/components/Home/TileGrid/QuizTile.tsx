import { useState } from "react";
import img from "@/assets/images/office.jpg";
import { ShortQuizResponse } from "@/types/types/quiz";
import { formatDate } from "@/utils/date";
import { Link } from "@tanstack/react-router";

type QuizTileProps = {
  quiz: ShortQuizResponse;
};

export default function QuizTile({ quiz }: QuizTileProps) {
  const [bannerSrc, setBannerSrc] = useState(quiz.imageUrl || img);

  return (
    <div className="bg-secondary rounded-sm w-full h-full flex flex-col">
      <Link
        to={`/$username/$quizSlug`}
        params={{
          username: quiz.userName,
          quizSlug: quiz.slug,
        }}
        search={{
          term: 1,
        }}
        className="relative block overflow-hidden rounded-t-sm"
      >
        <img
          src={bannerSrc}
          loading="lazy"
          decoding="async"
          alt="quiz banner"
          onError={() => setBannerSrc(img)}
          className="h-[clamp(7.5rem,16vh,12rem)] w-full object-cover"
        />
        <div className="text-white text-sm font-bold absolute top-4 right-4 px-4 py-2 bg-lightBlue rounded-3xl">
          {`${quiz.numberOfQuestions} Questions`}
        </div>
      </Link>
      <div className="p-6 flex flex-1 flex-col">
        <Link
          to={`/$username/$quizSlug`}
          params={{
            username: quiz.userName,
            quizSlug: quiz.slug,
          }}
          search={{
            term: 1,
          }}
          className="relative"
        >
          <h3 className="text-white text-2xl font-bold mb-3">{quiz.name}</h3>
          <p className="text-textPrimary font-medium">{quiz.description}</p>
        </Link>
        <div className="mt-auto">
          <span className="block h-[1px] bg-white bg-opacity-10 mt-8 mb-4"></span>
          <div className="flex items-center gap-4">
          <img
            src={img}
            loading="lazy"
            decoding="async"
            alt="user avatar"
            className="w-10 h-10 rounded-3xl"
          />
          <div className="border-r-[1px] border-white border-opacity-10 pr-4">
            <h4 className="text-white font-semibold text-sm">By {quiz.userName}</h4>
            <p className="text-textPrimary text-xs font-medium">Graphic Designer</p>
          </div>
          <div>
            <h4 className="text-white font-semibold text-sm">Date</h4>
            <p className="text-textPrimary text-xs font-medium">
              {formatDate(quiz.createdAt)}
            </p>
          </div>
          </div>
        </div>
      </div>
    </div>
  );
}
