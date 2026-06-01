import {
  FieldErrors,
  FieldValues,
  Path,
  UseFormRegister,
} from "react-hook-form";

interface FormInputProps<TFieldValues extends FieldValues> {
  name: Path<TFieldValues>;
  type: string;
  placeholder: string;
  labelText: string;
  errors?: FieldErrors<TFieldValues>;
  register: UseFormRegister<TFieldValues>;
}

export function FormInput<TFieldValues extends FieldValues>({
  register,
  name,
  type,
  placeholder,
  labelText,
  errors,
}: FormInputProps<TFieldValues>) {
  const error = errors?.[name];
  const inputId = `input-${String(name).replace(/[^\w-]/g, "-")}`;
  const errorMessage = error?.message;
  const errorId = `${inputId}-error`;

  if (typeof errorMessage !== "string") {
    return (
      <div className="mb-8">
        <label htmlFor={inputId} className="block text-sm font-medium mb-3">
          {labelText}
        </label>
        <input
          id={inputId}
          type={type}
          {...register(name)}
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
        {...register(name)}
        placeholder={placeholder}
        aria-invalid
        aria-describedby={errorId}
        className="bg-input text-textPrimary rounded-sm w-full py-3 pl-6 shadow-md
                    border border-transparent focus:border-lightBlue outline-none"
      />
      <p id={errorId} className="pt-3 text-sm" role="alert">
        <span className="text-red-600">{errorMessage}</span>
      </p>
    </div>
  );
}
