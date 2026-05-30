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
    <div className="flex flex-col w-full">
      <div className="w-[4.5rem] h-[4.5rem] bg-lightBlue/10 rounded-sm mb-8 flex items-center justify-center">
        {children}
      </div>
      <h3 className="text-xl sm:text-2xl font-bold mb-4">{title}</h3>
      <p className="text-textPrimary font-medium">{desc}</p>
    </div>
  );
}
