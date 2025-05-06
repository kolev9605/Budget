import { InformationCircleIcon } from '@heroicons/react/24/outline';

export default function ErrorSection({ errors, sectionTitle = "Please fix the following issues:" }) {
  return (
    Object.keys(errors).length > 0 && (
      <div className="bg-red-500/20 p-4 rounded-xl mb-6">
        <p className="text-red-400 text-sm flex items-center gap-2">
          <InformationCircleIcon className="h-5 w-5" />
            {sectionTitle}
        </p>
        <ul className="list-disc list-inside mt-2 ml-5">
          {Object.values(errors).map((error, index) => (
            <li key={index} className="text-red-300 text-sm">
              {error}
            </li>
          ))}
        </ul>
      </div>
    )
  );
}
