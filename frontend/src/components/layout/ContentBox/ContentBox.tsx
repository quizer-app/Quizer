interface ContentBoxProps {
  children: React.ReactNode;
}

export default function ContentBox({ children }: ContentBoxProps) {
  return (
    <div className="bg-primary w-full text-white px-4 py-14 md:py-16 lg:py-20">
      <div className="bg-secondary mx-auto rounded-sm max-w-[420px] sm:max-w-[500px] py-10 px-6 sm:p-[60px]">
        {children}
      </div>
    </div>
  );
}
