export const getCurrencyLabel = (currencyCode, amount) => {
  const currencyFormats = {
    USD: { symbol: "$", position: "before" },
    EUR: { symbol: "€", position: "after" },
    BGN: { symbol: "лв.", position: "after" },
    GBP: { symbol: "£", position: "before" },
    JPY: { symbol: "¥", position: "before" },
    AUD: { symbol: "A$", position: "before" },
    CAD: { symbol: "C$", position: "before" },
  };

  const format = currencyFormats[currencyCode] || { symbol: currencyCode, position: "after" };
  const amountFormat = parseFloat(amount).toLocaleString(undefined, {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });

  return format.position === "before"
    ? `${format.symbol}${amountFormat}`
    : `${amountFormat}${format.symbol}`;
};