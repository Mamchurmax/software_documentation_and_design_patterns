using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Lab4.Strategy.Models;

namespace Lab4.Strategy.Services
{
    /// <summary>
    /// Reads crime records from a CSV file using CsvHelper.
    /// </summary>
    public class CsvDataReader : IDataReader
    {
        public IEnumerable<CrimeRecord> ReadAll(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                BadDataFound = null
            });

            var records = new List<CrimeRecord>();

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var record = new CrimeRecord
                {
                    Id = csv.GetField("ID") ?? "",
                    CaseNumber = csv.GetField("Case Number") ?? "",
                    Date = csv.GetField("Date") ?? "",
                    Block = csv.GetField("Block") ?? "",
                    PrimaryType = csv.GetField("Primary Type") ?? "",
                    Description = csv.GetField("Description") ?? "",
                    LocationDescription = csv.GetField("Location Description") ?? "",
                    Arrest = csv.GetField("Arrest")?.ToLower() == "true",
                    Domestic = csv.GetField("Domestic")?.ToLower() == "true",
                    District = csv.GetField("District") ?? "",
                    Ward = csv.GetField("Ward") ?? "",
                    Year = int.TryParse(csv.GetField("Year"), out var y) ? y : 0
                };
                records.Add(record);
            }

            return records;
        }
    }
}
