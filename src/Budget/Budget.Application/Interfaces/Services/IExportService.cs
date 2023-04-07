using Newtonsoft.Json;
using System.Collections.Generic;

namespace Budget.Application.Interfaces.Services
{
    public interface IExportService
    {
        byte[] SerializeToByteArray<T>(IEnumerable<T> items, JsonSerializerSettings jsonSerializerSettings);
    }
}
