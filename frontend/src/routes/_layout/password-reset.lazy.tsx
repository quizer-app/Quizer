import FillHeight from "@/components/layout/Background/Background";
import { Button } from "@/components/layout/ContentBox/Button";
import ContentBox from "@/components/layout/ContentBox/ContentBox";
import LockIcon from "@/components/layout/ContentBox/Icons/LockIcon";
import Text from "@/components/layout/ContentBox/Text";
import Title from "@/components/layout/ContentBox/Title";
import { createLazyFileRoute } from "@tanstack/react-router";

export const Route = createLazyFileRoute("/_layout/password-reset")({
  component: PasswordReset,
});

function PasswordReset() {
  return (
    <>
      <ContentBox>
        <LockIcon />
        <Title>Reset Password</Title>

        <form>
          <Text>2 Inputs here. New password and confirm new password</Text>
          <Button>Reset Password</Button>
        </form>
      </ContentBox>
      <FillHeight />
    </>
  );
}
