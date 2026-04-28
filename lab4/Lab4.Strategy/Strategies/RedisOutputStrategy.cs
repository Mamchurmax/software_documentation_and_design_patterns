using Lab4.Strategy.Models;

namespace Lab4.Strategy.Strategies
{
    /// <summary>
    /// Concrete Strategy: outputs crime records to Redis (stub implementation).
    /// In a real application, this would use a Redis client (e.g., StackExchange.Redis).
    /// </summary>
    public class RedisOutputStrategy : IOutputStrategy
    {
        private readonly string _keyPrefix = "crime";

        public void Write(CrimeRecord record)
        {
            Console.WriteLine($"[Redis SET] {_keyPrefix}:{record.Id} -> {record}");
        }

        public void WriteAll(IEnumerable<CrimeRecord> records)
        {
            Console.WriteLine("=== Redis Output Strategy ===");
            Console.WriteLine($"Writing {records.Count()} records to Redis...\n");

            int count = 0;
            foreach (var record in records)
            {
                Write(record);
                count++;
            }

            Console.WriteLine($"\n=== Done: {count} records written to Redis ===");
        }
    }
}
