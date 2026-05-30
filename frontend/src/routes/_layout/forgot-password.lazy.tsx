import FillHeight from "@/components/layout/Background/Background";
import { Button } from "@/components/layout/ContentBox/Button";
import ContentBox from "@/components/layout/ContentBox/ContentBox";
import WarningIcon from "@/components/layout/ContentBox/Icons/WarningIcon";
import Text from "@/components/layout/ContentBox/Text";
import Title from "@/components/layout/ContentBox/Title";
import { Link, createLazyFileRoute } from "@tanstack/react-router";

export const Route = createLazyFileRoute("/_layout/forgot-password")({
  component: ForgotPassword,
});

function ForgotPassword() {
  return (
    <>
      <ContentBox>
        <WarningIcon />
        <Title>Reset Password</Title>
        <Text className="mb-2">Forgot your password? No worries...</Text>
        <Text>
          Enter your email and we'll send you a link to reset your password
        </Text>
        {/* <form><InputForm /> here</form> */}
        <Link to={"/"}>
          <Button>Request Password Change</Button>
        </Link>
      </ContentBox>
      <FillHeight />
    </>
  );
}
