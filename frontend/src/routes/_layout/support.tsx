import ContactForm from "@/components/Support/ContactForm";
import SubscriptionForm from "@/components/Support/SubscriptionForm";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_layout/support")({
  component: Support,
});

export default function Support() {
  return (
    <div className="w-full bg-primary">
      <div className="mainContainer mx-auto py-12">
        <div className="max-w-xl">
          <h2 className="text-white font-bold text-2xl sm:text-3xl mb-6">
            Support Page
          </h2>
          <p className="text-textPrimary mb-12">
            Lorem ipsum dolor sit amet consectetur adipisicing elit. Cumque
            minima cupiditate dignissimos ipsum consectetur architecto.
          </p>
        </div>
        <div className="flex flex-col lg:flex-row gap-10">
          <ContactForm />
          <SubscriptionForm />
        </div>
      </div>
    </div>
  );
}
