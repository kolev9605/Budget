import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/24/outline";

const PeriodPicker = ({ selectedDateRange, referenceDate, setReferenceDate }) => {
  const getDateRangeLabel = () => {
    switch (selectedDateRange) {
      case "day":
        return referenceDate.toLocaleDateString("en-US", { year: "numeric", month: "long", day: "numeric" });
      case "week": {
        const startOfWeek = new Date(referenceDate);
        startOfWeek.setDate(startOfWeek.getDate() - startOfWeek.getDay());
        const endOfWeek = new Date(startOfWeek);
        endOfWeek.setDate(startOfWeek.getDate() + 6);
        return `${startOfWeek.toLocaleDateString("en-US", {
          month: "short",
          day: "numeric",
        })} - ${endOfWeek.toLocaleDateString("en-US", {
          month: "short",
          day: "numeric",
        })} ${startOfWeek.getFullYear()}`;
      }
      case "month":
        return referenceDate.toLocaleDateString("en-US", { year: "numeric", month: "long" });
      case "year":
        return referenceDate.getFullYear().toString();
      default:
        return "";
    }
  };

  const handlePrevRange = () => {
    const newDate = new Date(referenceDate);
    switch (selectedDateRange) {
      case "day":
        newDate.setDate(newDate.getDate() - 1);
        break;
      case "week":
        newDate.setDate(newDate.getDate() - 7);
        break;
      case "month":
        newDate.setMonth(newDate.getMonth() - 1);
        break;
      case "year":
        newDate.setFullYear(newDate.getFullYear() - 1);
        break;
      default:
        break;
    }

    setReferenceDate(newDate);
  };

  const handleNextRange = () => {
    const newDate = new Date(referenceDate);
    const today = new Date();

    switch (selectedDateRange) {
      case "day":
        newDate.setDate(newDate.getDate() + 1);
        break;
      case "week":
        newDate.setDate(newDate.getDate() + 7);
        break;
      case "month":
        newDate.setMonth(newDate.getMonth() + 1);
        break;
      case "year":
        newDate.setFullYear(newDate.getFullYear() + 1);
        break;
      default:
        break;
    }

    if (newDate > today) {
      setReferenceDate(today);
    } else {
      setReferenceDate(newDate);
    }
  };

  return (
    <div className="flex items-center justify-center gap-4">
      <button
        onClick={handlePrevRange}
        className="bg-gray-700 text-gray-300 hover:bg-blue-500 hover:text-white transition-colors p-2 rounded-lg shadow-md"
      >
        <ChevronLeftIcon className="h-5 w-5" />
      </button>
      <div className="text-gray-100 text-sm text-center w-48">{getDateRangeLabel()}</div>
      <button
        onClick={handleNextRange}
        className="bg-gray-700 text-gray-300 hover:bg-blue-500 hover:text-white transition-colors p-2 rounded-lg shadow-md"
      >
        <ChevronRightIcon className="h-5 w-5" />
      </button>
    </div>
  );
};

export default PeriodPicker;
