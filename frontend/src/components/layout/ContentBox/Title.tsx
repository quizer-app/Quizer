interface TitleProps {
  children: React.ReactNode;
}

export default function Title({ children }: TitleProps) {
  return (
    <h2 className="font-bold text-center text-2xl sm:text-3xl mb-3">
      {children}
    </h2>
  );
}
