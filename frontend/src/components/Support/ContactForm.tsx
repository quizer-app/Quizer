export default function ContactForm() {
  return (
    <div className="w-full bg-secondary rounded-sm p-8 lg:basis-2/3">
      <h3 className="text-white text-2xl sm:text-3xl font-bold mb-4">
        Need Help? Open a Ticket
      </h3>
      <p className="text-textPrimary mb-10">
        Our support team will get back to you via email.
      </p>
      <form className="flex flex-col text-white gap-3">
        <div className="flex flex-col md:flex-row md:justify-between md:gap-10">
          <div className="flex flex-1 flex-col gap-2">
            <label className="font-semibold">Your name</label>
            <input
              type="text"
              placeholder="Enter your name"
              className="bg-input text-textPrimary rounded-sm w-full py-3 pl-6 shadow-md
                border border-transparent focus:border-lightBlue outline-none mb-5"
            />
          </div>
          <div className="flex flex-1 flex-col gap-2">
            <label className="font-semibold">Your email</label>
            <input
              type="text"
              placeholder="Enter your email"
              className="bg-input text-textPrimary rounded-sm w-full py-3 pl-6 shadow-md
                border border-transparent focus:border-lightBlue outline-none mb-5"
            />
          </div>
        </div>

        <label className="font-semibold">Your message</label>
        <textarea
          placeholder="Enter your message"
          rows={4}
          className="bg-input text-textPrimary rounded-sm w-full py-3 pl-6 shadow-md
                border border-transparent focus:border-lightBlue outline-none mb-5 resize-none"
        />

        <div>
          <button
            type="submit"
            className="w-full sm:w-auto bg-lightBlue hover:bg-opacity-95 rounded-sm font-medium py-4 px-10"
          >
            Report
          </button>
        </div>
      </form>
    </div>
  );
}
