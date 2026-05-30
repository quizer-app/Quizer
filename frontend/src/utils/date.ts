export const formatDate = (date: string): string => {
  const userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;

  const dateObject = new Date(date);

  const dateStringInUserTimeZone = dateObject.toLocaleString("pl-PL", {
    timeZone: userTimeZone,
    year: "numeric",
    month: "numeric",
    day: "numeric",
  });

  return dateStringInUserTimeZone;
};
