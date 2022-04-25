using Budget.Core.Interfaces.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace Budget.Infrastructure.Services
{
    public class ExportService : IExportService
    {
        public byte[] SerializeToByteArray<T>(IEnumerable<T> items, JsonSerializerSettings jsonSerializerSettings)
        {
            var result = JsonConvert.SerializeObject(items, jsonSerializerSettings);

            return Encoding.UTF8.GetBytes(result);
        }
    }
}
