interface ButtonProps {
  children?: React.ReactNode;
  disabled?: boolean;
}

export function Button({ children, disabled }: ButtonProps) {
  return (
    <button
      type="submit"
      className="w-full bg-lightBlue hover:bg-opacity-95 rounded-sm font-medium py-4 mb-6 disabled:cursor-not-allowed disabled:opacity-60"
      disabled={disabled}
      aria-disabled={disabled}
    >
      {children}
    </button>
  );
}
