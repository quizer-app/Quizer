import { api } from "@/api/axios";
import { selectIsAuthenticated, useAuthStore } from "@/atoms/auth";
import { Button } from "@/components/layout/ContentBox/Button";
import ContentBox from "@/components/layout/ContentBox/ContentBox";
import { FormInput } from "@/components/layout/ContentBox/FormInput";
import GoogleButton from "@/components/layout/ContentBox/GoogleButton";
import Text from "@/components/layout/ContentBox/Text";
import TextWithLines from "@/components/layout/ContentBox/TextWithLines";
import TextWithLink from "@/components/layout/ContentBox/TextWithLink";
import Title from "@/components/layout/ContentBox/Title";
import { AuthResponse } from "@/types/types/auth";
import { useForm } from "@tanstack/react-form";
import { useMutation } from "@tanstack/react-query";
import { createLazyFileRoute, useNavigate } from "@tanstack/react-router";
import axios, { AxiosError } from "axios";
import { useEffect, useState } from "react";
import { toast } from "react-hot-toast/headless";
import { useTranslation } from "react-i18next";
import { z } from "zod";

export const Route = createLazyFileRoute("/_layout/signup")({
  component: SignUp,
});

const schema = z
  .object({
    username: z.string().min(3, "Username must be at least 3 characters").max(32),
    email: z.string().email("Enter a valid email").min(3).max(64),
    password: z.string().min(8, "Password must be at least 8 characters").max(64),
    confirmPassword: z
      .string()
      .min(8, "Password confirmation must be at least 8 characters")
      .max(64),
  })
  .refine(data => data.password === data.confirmPassword, {
    message: "Passwords do not match",
    path: ["confirmPassword"],
  });

type Form = z.infer<typeof schema>;

const getRegistrationErrorMessage = (error: AxiosError) => {
  if (!error.response) {
    return "Network error. Check your connection and try again.";
  }

  if (error.response.status === 400) {
    return "Please check the entered values and try again.";
  }

  if (error.response.status === 409) {
    return "That account already exists. Try signing in instead.";
  }

  if (error.response.status >= 500) {
    return "Server error. Please try again in a moment.";
  }

  const data = error.response.data;
  if (
    typeof data === "object" &&
    data !== null &&
    "message" in data &&
    typeof data.message === "string"
  ) {
    return data.message;
  }

  return "Unable to sign up right now. Please try again.";
};

function SignUp() {
  const [formError, setFormError] = useState<string | null>(null);
  const { t } = useTranslation();

  const registerMutation = useMutation({
    mutationFn: (data: Form) => {
      return api.post<AuthResponse>("/api/v1/Auth/register", data, {
        withCredentials: true,
      });
    },
    onMutate: () => {
      setFormError(null);
    },
    onSuccess: data => {
      const { message } = data.data;
      toast.success(message);
      form.reset();
    },
    onError: error => {
      if (!axios.isAxiosError(error)) {
        const fallbackError = "Unable to sign up right now. Please try again.";
        setFormError(fallbackError);
        toast.error(fallbackError);
        return;
      }

      const message = getRegistrationErrorMessage(error);
      setFormError(message);
      toast.error(message);
    },
  });

  const form = useForm({
    defaultValues: {
      username: "",
      email: "",
      password: "",
      confirmPassword: "",
    } as Form,
    onSubmit: async ({ value }) => {
      const parsed = schema.safeParse(value);
      if (!parsed.success) {
        return;
      }

      registerMutation.mutate(parsed.data);
    },
  });

  const isAuthenticated = useAuthStore(selectIsAuthenticated);
  const navigate = useNavigate();

  useEffect(() => {
    if (!isAuthenticated) {
      return;
    }

    navigate({ to: "/", replace: true });
  }, [isAuthenticated, navigate]);

  useEffect(() => {
    const usernameInput = document.getElementById("input-username");
    if (usernameInput instanceof HTMLInputElement) {
      usernameInput.focus();
    }
  }, []);

  return (
    <ContentBox>
      <Title>{t("auth.createAccountTitle")}</Title>
      <Text>{t("auth.createAccountSubtitle")}</Text>
      <GoogleButton>{t("auth.signUpGoogle")}</GoogleButton>
      <TextWithLines>{t("auth.signUpWithEmail")}</TextWithLines>
      {formError && (
        <p className="mb-4 text-sm text-red-600" role="alert">
          {formError}
        </p>
      )}
      <form
        onSubmit={event => {
          event.preventDefault();
          event.stopPropagation();
          void form.handleSubmit();
        }}
        autoComplete="off"
        aria-busy={registerMutation.isPending}
      >
        <form.Field
          name="username"
          validators={{
            onChange: ({ value }) => {
              if (schema.shape.username.safeParse(value).success) {
                return undefined;
              }

              return "Username must be at least 3 characters";
            },
          }}
          children={field => (
            <FormInput
              name={field.name}
              type="text"
              placeholder={t("auth.usernamePlaceholder")}
              labelText={t("auth.username")}
              value={field.state.value}
              onChange={field.handleChange}
              onBlur={field.handleBlur}
              error={field.state.meta.errors[0]}
            />
          )}
        />
        <form.Field
          name="email"
          validators={{
            onChange: ({ value }) => {
              if (schema.shape.email.safeParse(value).success) {
                return undefined;
              }

              return "Enter a valid email";
            },
          }}
          children={field => (
            <FormInput
              name={field.name}
              type="text"
              placeholder={t("auth.emailPlaceholder")}
              labelText={t("auth.email")}
              value={field.state.value}
              onChange={field.handleChange}
              onBlur={field.handleBlur}
              error={field.state.meta.errors[0]}
            />
          )}
        />
        <form.Field
          name="password"
          validators={{
            onChange: ({ value }) => {
              if (schema.shape.password.safeParse(value).success) {
                return undefined;
              }

              return "Password must be at least 8 characters";
            },
          }}
          children={field => (
            <FormInput
              name={field.name}
              type="password"
              placeholder={t("auth.passwordPlaceholder")}
              labelText={t("auth.password")}
              value={field.state.value}
              onChange={field.handleChange}
              onBlur={field.handleBlur}
              error={field.state.meta.errors[0]}
            />
          )}
        />
        <form.Field
          name="confirmPassword"
          validators={{
            onChange: ({ value, fieldApi }) => {
              const password = fieldApi.form.getFieldValue("password");
              if (value !== password) {
                return "Passwords do not match";
              }

              if (schema.shape.confirmPassword.safeParse(value).success) {
                return undefined;
              }

              return "Password confirmation must be at least 8 characters";
            },
          }}
          children={field => (
            <FormInput
              name={field.name}
              type="password"
              placeholder={t("auth.confirmPasswordPlaceholder")}
              labelText={t("auth.confirmPassword")}
              value={field.state.value}
              onChange={field.handleChange}
              onBlur={field.handleBlur}
              error={field.state.meta.errors[0]}
            />
          )}
        />
        <Button disabled={registerMutation.isPending}>
          {registerMutation.isPending ? t("auth.creatingAccount") : t("auth.signUp")}
        </Button>
        <TextWithLink link={t("nav.signIn")} path="/signin">
          {t("auth.alreadyUsing")}
        </TextWithLink>
      </form>
    </ContentBox>
  );
}
