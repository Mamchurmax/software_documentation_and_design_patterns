using Lab4.Strategy.Models;

namespace Lab4.Strategy.Strategies
{
    /// <summary>
    /// Concrete Strategy: outputs crime records to Apache Kafka (stub implementation).
    /// In a real application, this would use a Kafka producer client (e.g., Confluent.Kafka).
    /// </summary>
    public class KafkaOutputStrategy : IOutputStrategy
    {
        private readonly string _topicName = "chicago-crimes";

        public void Write(CrimeRecord record)
        {
            Console.WriteLine($"[Kafka -> {_topicName}] Sending: {record}");
        }

        public void WriteAll(IEnumerable<CrimeRecord> records)
        {
            Console.WriteLine("=== Kafka Output Strategy ===");
            Console.WriteLine($"Sending {records.Count()} records to Kafka topic '{_topicName}'...\n");

            int count = 0;
            foreach (var record in records)
            {
                Write(record);
                count++;
            }

            Console.WriteLine($"\n=== Done: {count} records sent to Kafka topic '{_topicName}' ===");
        }
    }
}
