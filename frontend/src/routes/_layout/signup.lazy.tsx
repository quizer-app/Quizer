import { api } from "@/api/axios";
import { isAuthenticatedAtom } from "@/atoms/auth";
import { Button } from "@/components/layout/ContentBox/Button";
import ContentBox from "@/components/layout/ContentBox/ContentBox";
import { FormInput } from "@/components/layout/ContentBox/FormInput";
import GoogleButton from "@/components/layout/ContentBox/GoogleButton";
import Text from "@/components/layout/ContentBox/Text";
import TextWithLines from "@/components/layout/ContentBox/TextWithLines";
import TextWithLink from "@/components/layout/ContentBox/TextWithLink";
import Title from "@/components/layout/ContentBox/Title";
import { AuthResponse } from "@/types/types/auth";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation } from "@tanstack/react-query";
import { createLazyFileRoute, useNavigate } from "@tanstack/react-router";
import { AxiosError } from "axios";
import { useAtomValue } from "jotai";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { toast } from "react-hot-toast/headless";
import { z } from "zod";

export const Route = createLazyFileRoute("/_layout/signup")({
  component: SignUp,
});

const schema = z.object({
  username: z.string().min(3).max(32),
  email: z.string().email().min(3).max(64),
  password: z.string().min(8).max(64),
  confirmPassword: z.string().min(8).max(64),
});

type Form = z.infer<typeof schema>;

function SignUp() {
  const {
    register,
    handleSubmit,
    setFocus,
    formState: { errors },
  } = useForm({
    resolver: zodResolver(schema),
    mode: "onChange",
  });

  const registerMutation = useMutation({
    mutationFn: (data: Form) => {
      return api.post<AuthResponse>("/api/v1/Auth/register", data, {
        withCredentials: true,
      });
    },
    onSuccess: data => {
      const { message } = data.data;
      toast.success(message);
    },
    onError: (error: AxiosError) => {
      toast.error(error.message);
    },
  });

  const isAuthenticated = useAtomValue(isAuthenticatedAtom);
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated && !registerMutation.isSuccess) {
      navigate({ to: "/" });
    }
  }, [isAuthenticated, navigate, registerMutation.isSuccess]);

  useEffect(() => {
    setFocus("username");
  }, [setFocus]);

  return (
    <ContentBox>
      <Title>Create your account</Title>
      <Text>It's totally free and super easy</Text>
      <GoogleButton>Sign up with Google</GoogleButton>
      <TextWithLines>Or, register with your email</TextWithLines>
      <form
        onSubmit={handleSubmit(data => {
          registerMutation.mutate(data as Form);
        })}
        autoComplete="off"
      >
        <FormInput
          name="username"
          type="text"
          placeholder="Enter your username"
          labelText="Username"
          register={register}
          errors={errors}
        />
        <FormInput
          name="email"
          type="text"
          placeholder="Enter your email"
          labelText="Email"
          register={register}
          errors={errors}
        />
        <FormInput
          name="password"
          type="password"
          placeholder="Enter your password"
          labelText="Password"
          register={register}
          errors={errors}
        />
        <FormInput
          name="confirmPassword"
          type="password"
          placeholder="Confirm your password"
          labelText="Confirm Password"
          register={register}
          errors={errors}
        />
        <Button>Sign up</Button>
        <TextWithLink link="Sign in" path="/signin">
          Already using Quizer?
        </TextWithLink>
      </form>
    </ContentBox>
  );
}
