interface TextWithLinesProps {
  children: React.ReactNode;
}

export default function TextWithLines({ children }: TextWithLinesProps) {
  return (
    <div className="flex items-center justify-center gap-5 mb-8">
      <span className="h-[1px] grow bg-textPrimary font-medium hidden sm:block" />
      <p className="text-textPrimary text-center font-medium">{children}</p>
      <span className="h-[1px] grow bg-textPrimary font-medium hidden sm:block" />
    </div>
  );
}
