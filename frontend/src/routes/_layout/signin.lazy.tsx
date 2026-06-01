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
import toast from "react-hot-toast";
import { z } from "zod";

export const Route = createLazyFileRoute("/_layout/signin")({
  component: SignIn,
});

const schema = z.object({
  email: z.string().min(1, "Email is required"),
  password: z.string().min(1, "Password is required"),
  rememberMe: z.boolean().optional(),
});

type Form = z.infer<typeof schema>;

const getAuthErrorMessage = (error: AxiosError) => {
  if (!error.response) {
    return "Network error. Check your connection and try again.";
  }

  if (error.response.status === 400) {
    return "Please check the entered values and try again.";
  }

  if (error.response.status === 401) {
    return "Invalid email or password.";
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

  return "Unable to sign in right now. Please try again.";
};

function SignIn() {
  const isAuthenticated = useAuthStore(selectIsAuthenticated);
  const navigate = useNavigate();
  const setAccessToken = useAuthStore(state => state.setAccessToken);
  const [formError, setFormError] = useState<string | null>(null);

  const loginMutation = useMutation({
    mutationFn: (data: Form) => {
      return api.post<AuthResponse>("/api/v1/Auth/login", data, {
        withCredentials: true,
      });
    },
    onMutate: () => {
      setFormError(null);
    },
    onSuccess: data => {
      const { accessToken, message } = data.data;
      toast.success(message);
      setAccessToken(accessToken ?? null);
      form.reset();
    },
    onError: error => {
      if (!axios.isAxiosError(error)) {
        const fallbackError = "Unable to sign in right now. Please try again.";
        setFormError(fallbackError);
        toast.error(fallbackError);
        return;
      }

      const message = getAuthErrorMessage(error);
      setFormError(message);
      toast.error(message);
    },
  });

  const form = useForm({
    defaultValues: {
      email: "",
      password: "",
      rememberMe: false,
    } as Form,
    onSubmit: async ({ value }) => {
      const parsed = schema.safeParse(value);
      if (!parsed.success) {
        return;
      }

      loginMutation.mutate(parsed.data);
    },
  });

  useEffect(() => {
    const emailInput = document.getElementById("input-email");
    if (emailInput instanceof HTMLInputElement) {
      emailInput.focus();
    }
  }, []);

  const redirectParam = new URLSearchParams(window.location.search).get("redirect");
  const redirectTo =
    redirectParam && redirectParam.startsWith("/") ? redirectParam : "/";

  useEffect(() => {
    if (!isAuthenticated) {
      return;
    }

    navigate({ to: redirectTo, replace: true });
  }, [isAuthenticated, navigate, redirectTo]);

  return (
    <ContentBox>
      <Title>Sign in to your account</Title>
      <Text>Login to your account for better experience</Text>
      <GoogleButton>Sign in with Google</GoogleButton>
      <TextWithLines>Or, sign in with your email</TextWithLines>
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
      >
        <form.Field
          name="email"
          validators={{
            onChange: ({ value }) => {
              if (schema.shape.email.safeParse(value).success) {
                return undefined;
              }

              return "Email is required";
            },
          }}
          children={field => (
            <FormInput
              name={field.name}
              type="text"
              placeholder="Enter your email"
              labelText="Email or Username"
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

              return "Password is required";
            },
          }}
          children={field => (
            <FormInput
              name={field.name}
              type="password"
              placeholder="Enter your password"
              labelText="Password"
              value={field.state.value}
              onChange={field.handleChange}
              onBlur={field.handleBlur}
              error={field.state.meta.errors[0]}
            />
          )}
        />
        <form.Field
          name="rememberMe"
          children={field => (
            <div className="mt-8 flex justify-center mb-5">
              <input
                type="checkbox"
                checked={field.state.value}
                onChange={event => field.handleChange(event.target.checked)}
                className="bg-secondary"
                id="rememberMe"
              />
              <label
                htmlFor="rememberMe"
                className="text-textPrimary text-sm font-medium grow pl-3"
              >
                Keep me signed in
              </label>
              <a href="#" className="text-lightBlue text-sm font-medium">
                Forgot Password?
              </a>
            </div>
          )}
        />
        <Button>Sign in</Button>
      </form>
      <TextWithLink link="Sign up" path="/signup">
        Don't have an account?
      </TextWithLink>
    </ContentBox>
  );
}
