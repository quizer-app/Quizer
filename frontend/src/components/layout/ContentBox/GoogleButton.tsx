import GoogleLogo from "@/assets/images/GoogleIcon.svg";

interface GoogleButtonProps {
  children: React.ReactNode;
}

export default function GoogleButton({ children }: GoogleButtonProps) {
  return (
    <button
      className="text-textPrimary bg-input shadow-md rounded-sm font-medium w-full py-3
                  flex items-center justify-center gap-3 hover:text-white mb-6"
    >
      <span>
        <img src={GoogleLogo} alt="logo" className="w-5 h-5"></img>
      </span>
      {children}
    </button>
  );
}
