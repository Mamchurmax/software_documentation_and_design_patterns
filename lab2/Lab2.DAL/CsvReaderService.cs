using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Lab2.Domain.DTOs;
using Lab2.Domain.Interfaces;

namespace Lab2.DAL
{
    public class CsvReaderService : ICsvReader
    {
        public IEnumerable<CsvHotelRecord> ReadRecords(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);
            return csv.GetRecords<CsvHotelRecord>().ToList();
        }
    }
}
