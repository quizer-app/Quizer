import FillHeight from "@/components/layout/Background/Background";
import { Button } from "@/components/layout/ContentBox/Button";
import ContentBox from "@/components/layout/ContentBox/ContentBox";
import CheckmarkIcon from "@/components/layout/ContentBox/Icons/CheckmarkIcon";
import Text from "@/components/layout/ContentBox/Text";
import Title from "@/components/layout/ContentBox/Title";
import { Link, createLazyFileRoute } from "@tanstack/react-router";

export const Route = createLazyFileRoute("/_layout/verified")({
  component: Verified,
});

function Verified() {
  return (
    <>
      <ContentBox>
        <CheckmarkIcon />
        <Title>Email verified successfully</Title>
        <Text className="text-lg font-bold text-textHover mb-8">
          Congratulations Username!
        </Text>
        <Text className="mb-2">Thank you, your email has been verified.</Text>
        <Text> Your account is now active. Have fun!</Text>
        <Link to={"/"}>
          <Button>Go To Home Page</Button>
        </Link>
      </ContentBox>
      <FillHeight />
    </>
  );
}
