interface ButtonProps {
  children?: React.ReactNode;
  disabled?: boolean;
}

export function Button({ children, disabled }: ButtonProps) {
  return (
    <button
      className="w-full bg-lightBlue hover:bg-opacity-95 rounded-sm font-medium py-4 mb-6"
      disabled={disabled}
    >
      {children}
    </button>
  );
}
