using System.Text.Json;
using StackExchange.Redis;
using Lab4.Strategy.Models;

namespace Lab4.Strategy.Strategies
{
    /// <summary>
    /// Concrete Strategy: stores crime records in Redis using StackExchange.Redis.
    /// Each record is stored as a Redis hash with key "crime:{id}".
    /// </summary>
    public class RedisOutputStrategy : IOutputStrategy
    {
        private readonly string _connectionString;
        private readonly string _keyPrefix;

        public RedisOutputStrategy(string connectionString = "localhost:6379", string keyPrefix = "crime")
        {
            _connectionString = connectionString;
            _keyPrefix = keyPrefix;
        }

        public void Write(CrimeRecord record)
        {
            // Single write is handled by WriteAll for efficiency
        }

        public void WriteAll(IEnumerable<CrimeRecord> records)
        {
            Console.WriteLine("=== Redis Output Strategy ===");
            Console.WriteLine($"Connecting to Redis at {_connectionString}...");

            var connection = ConnectionMultiplexer.Connect(_connectionString);
            var db = connection.GetDatabase();

            Console.WriteLine("Connected!\n");

            int count = 0;
            foreach (var record in records)
            {
                var key = $"{_keyPrefix}:{record.Id}";
                var hashEntries = new HashEntry[]
                {
                    new("id", record.Id),
                    new("case_number", record.CaseNumber),
                    new("date", record.Date),
                    new("block", record.Block),
                    new("primary_type", record.PrimaryType),
                    new("description", record.Description),
                    new("location_description", record.LocationDescription),
                    new("arrest", record.Arrest.ToString()),
                    new("domestic", record.Domestic.ToString()),
                    new("district", record.District),
                    new("ward", record.Ward),
                    new("year", record.Year.ToString())
                };

                db.HashSet(key, hashEntries);
                count++;

                if (count % 100 == 0)
                    Console.WriteLine($"  Stored {count} records...");
            }

            // Store the total count
            db.StringSet($"{_keyPrefix}:total_count", count.ToString());

            Console.WriteLine($"\n=== Done: {count} records stored in Redis (keys: {_keyPrefix}:*) ===");

            // Verify by reading one back
            var sampleKey = $"{_keyPrefix}:{records.First().Id}";
            var sample = db.HashGetAll(sampleKey);
            Console.WriteLine($"\nVerification — reading back {sampleKey}:");
            foreach (var entry in sample)
            {
                Console.WriteLine($"  {entry.Name}: {entry.Value}");
            }

            connection.Close();
        }
    }
}
