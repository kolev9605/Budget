using Budget.Domain.Constants;
using Budget.Domain.Exceptions;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Budget.Domain.Interfaces;

namespace Budget.Infrastructure;

public class CsvParser : ICsvParser
{
    private readonly CsvConfiguration _csvConfiguration;

    public CsvParser()
    {
        _csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
        };
    }

    public IEnumerable<T> ParseCsvString<T>(string csvString)
    {
        try
        {
            using (var reader = new StringReader(csvString))
            using (var csvReader = new CsvReader(reader, _csvConfiguration))
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
        try
        {
            using (var reader = new StreamReader(path))
            using (var csvReader = new CsvReader(reader, _csvConfiguration))
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
