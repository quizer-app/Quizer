export default function SubscriptionForm() {
  return (
    <div className="w-full bg-secondary rounded-sm p-8 text-white flex flex-col lg:flex-1">
      <h3 className="text-white text-2xl font-bold mb-4">
        Subscribe to receive future updates
      </h3>
      <p className="text-textPrimary">
        Stay in the loop with our website updates delivered directly to your
        email inbox.
      </p>
      <span className="h-[0.5px] bg-textPrimary/50 my-10" />
      <form>
        <input
          type="text"
          placeholder="Enter your name"
          className="bg-input text-textPrimary rounded-sm w-full py-3 pl-6 shadow-md
                border border-transparent focus:border-lightBlue outline-none mb-5"
        />
        <input
          type="text"
          placeholder="Enter your name"
          className="bg-input text-textPrimary rounded-sm w-full py-3 pl-6 shadow-md
              border border-transparent focus:border-lightBlue outline-none mb-5"
        />
        <button
          type="submit"
          className="w-full bg-lightBlue hover:bg-opacity-95 rounded-sm font-medium py-4"
        >
          Subscribe
        </button>
      </form>
      <p className="text-textPrimary text-center mt-6">
        No spam guaranteeded, so please don't send any spam mail.
      </p>
    </div>
  );
}
