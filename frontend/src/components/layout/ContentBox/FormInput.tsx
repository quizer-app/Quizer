interface FormInputProps {
  name: string;
  type: string;
  placeholder: string;
  labelText: string;
  value: string;
  error?: string;
  onBlur: () => void;
  onChange: (value: string) => void;
}

export function FormInput({
  name,
  type,
  placeholder,
  labelText,
  value,
  error,
  onBlur,
  onChange,
}: FormInputProps) {
  const inputId = `input-${name.replace(/[^\w-]/g, "-")}`;
  const hasError = typeof error === "string" && error.length > 0;
  const errorId = `${inputId}-error`;

  if (!hasError) {
    return (
      <div className="mb-8">
        <label htmlFor={inputId} className="block text-sm font-medium mb-3">
          {labelText}
        </label>
        <input
          id={inputId}
          type={type}
          value={value}
          onChange={event => onChange(event.target.value)}
          onBlur={onBlur}
          placeholder={placeholder}
          aria-invalid={false}
          className="bg-input text-textPrimary rounded-sm w-full py-3 pl-6 shadow-md
                    border border-transparent focus:border-lightBlue outline-none"
        />
      </div>
    );
  }

  return (
    <div className="mb-8">
      <label htmlFor={inputId} className="block text-sm font-medium mb-3">
        {labelText}
      </label>
      <input
        id={inputId}
        type={type}
        value={value}
        onChange={event => onChange(event.target.value)}
        onBlur={onBlur}
        placeholder={placeholder}
        aria-invalid
        aria-describedby={errorId}
        className="bg-input text-textPrimary rounded-sm w-full py-3 pl-6 shadow-md
                    border border-transparent focus:border-lightBlue outline-none"
      />
      <p id={errorId} className="pt-3 text-sm" role="alert">
        <span className="text-red-600">{error}</span>
      </p>
    </div>
  );
}
