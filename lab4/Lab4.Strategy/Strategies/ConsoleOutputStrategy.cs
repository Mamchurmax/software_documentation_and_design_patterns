using Lab4.Strategy.Models;

namespace Lab4.Strategy.Strategies
{
    /// <summary>
    /// Concrete Strategy: outputs crime records to the console.
    /// </summary>
    public class ConsoleOutputStrategy : IOutputStrategy
    {
        public void Write(CrimeRecord record)
        {
            Console.WriteLine(record.ToString());
        }

        public void WriteAll(IEnumerable<CrimeRecord> records)
        {
            Console.WriteLine("=== Console Output Strategy ===");
            Console.WriteLine($"Writing {records.Count()} records to Console...\n");

            int count = 0;
            foreach (var record in records)
            {
                Write(record);
                count++;
            }

            Console.WriteLine($"\n=== Done: {count} records written to Console ===");
        }
    }
}
