export interface TermProps {
  term: string;
  desc: string;
}

export default function Term({ term, desc }: TermProps) {
  return (
    <div className="bg-secondary flex flex-col gap-3 sm:gap-6 p-5 rounded-sm sm:flex-row">
      <div className="sm:w-1/4 sm:min-w-[25%]">
        <p className="overflow-auto">{term}</p>
      </div>
      <span className="bg-white bg-opacity-20 h-[1px] w-full sm:w-[1px] sm:top-0 sm:bottom-0 sm:h-auto"></span>
      <p className="overflow-auto">{desc}</p>
    </div>
  );
}
