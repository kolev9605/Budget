using Budget.Core.Constants;
using Budget.Core.Exceptions;
using Budget.Core.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.CsvParser
{
    public class CsvParser : ICsvParser
    {
        public IEnumerable<T> ParseCsvString<T>(string csvString) 
        {
            try
            {
                using (var reader = new StringReader(csvString))
                using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csvReader.GetRecords<T>();

                    return records.ToList();
                }
            }
            catch (Exception)
            {
                throw new CsvParseException(ValidationMessages.CsvParser.InvalidCsv);
            }
        }

        public IEnumerable<T> ParseFromFile<T>(string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
            };

            try
            {
                using (var reader = new StreamReader(path))
                using (var csvReader = new CsvReader(reader, config))
                {
                    var records = csvReader.GetRecords<T>();

                    return records.ToList();
                }


            }
            catch (Exception)
            {
                throw new CsvParseException(ValidationMessages.CsvParser.InvalidCsv);
            }
        }
    }
}
