using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces
{
    public interface ICsvParser
    {
        IEnumerable<T> ParseCsvString<T>(string csvString);

        IEnumerable<T> ParseFromFile<T>(string path);
    }
}
