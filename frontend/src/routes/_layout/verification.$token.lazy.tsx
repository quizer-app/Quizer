import FillHeight from "@/components/layout/Background/Background";
import { Button } from "@/components/layout/ContentBox/Button";
import ContentBox from "@/components/layout/ContentBox/ContentBox";
import EmailIcon from "@/components/layout/ContentBox/Icons/EmailIcon";
import Text from "@/components/layout/ContentBox/Text";
import Title from "@/components/layout/ContentBox/Title";
import { createLazyFileRoute } from "@tanstack/react-router";

export const Route = createLazyFileRoute("/_layout/verification/$token")({
  component: Verification,
});

function Verification() {
  // const { token } = useParams();

  return (
    <>
      <ContentBox>
        <EmailIcon />
        <Title>Email Verification</Title>
        <Text className="mb-3">You're almost there! We sent an email to</Text>
        <Text className="text-lg font-bold text-textHover">
          szymonbudziakj@gmail.com
        </Text>
        <Text>
          Just click on the link in that email to complete your signup. If you
          don't see it, make sure to check the spam folder.
        </Text>
        <Text className="mb-8">Still can't find the email?</Text>
        <Button>Resend Verification Email</Button>
      </ContentBox>
      <FillHeight />
    </>
  );
}
