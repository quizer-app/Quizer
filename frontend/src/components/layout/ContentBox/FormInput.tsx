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

  return (
    <div className="mb-8">
      <label htmlFor={name} className="block text-sm font-medium mb-3">
        {labelText}
      </label>
      <input
        type={type}
        {...register(name)}
        placeholder={placeholder}
        className="bg-input text-textPrimary rounded-sm w-full py-3 pl-6 shadow-md
                    border border-transparent focus:border-lightBlue outline-none"
      />
      {error && (
        <p className="pt-3 text-sm">
          <span className="text-red-600">{error.message as string}</span>
        </p>
      )}
    </div>
  );
}
