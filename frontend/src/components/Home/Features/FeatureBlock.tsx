interface FeatureBlockProps {
  title: string;
  desc: string;
  children: React.ReactNode;
}

export default function FeatureBlock({
  title,
  desc,
  children,
}: FeatureBlockProps) {
  return (
    <div className="group flex h-full flex-col rounded-sm border border-white/10 bg-white/[0.03] p-7 transition-all duration-300 hover:-translate-y-1 hover:border-lightBlue/40 hover:bg-white/[0.05]">
      <div className="mb-7 flex h-[4.25rem] w-[4.25rem] items-center justify-center rounded-sm border border-lightBlue/20 bg-lightBlue/10 transition-colors group-hover:border-lightBlue/40 group-hover:bg-lightBlue/20">
        {children}
      </div>
      <h3 className="mb-3 text-xl font-bold sm:text-2xl">{title}</h3>
      <p className="font-medium leading-7 text-textPrimary">{desc}</p>
    </div>
  );
}
