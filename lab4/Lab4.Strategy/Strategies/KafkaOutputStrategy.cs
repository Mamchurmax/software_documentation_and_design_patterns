using System.Text.Json;
using Confluent.Kafka;
using Lab4.Strategy.Models;

namespace Lab4.Strategy.Strategies
{
    /// <summary>
    /// Concrete Strategy: sends crime records to Apache Kafka using Confluent.Kafka producer.
    /// </summary>
    public class KafkaOutputStrategy : IOutputStrategy
    {
        private readonly string _bootstrapServers;
        private readonly string _topicName;

        public KafkaOutputStrategy(string bootstrapServers = "localhost:9092", string topicName = "chicago-crimes")
        {
            _bootstrapServers = bootstrapServers;
            _topicName = topicName;
        }

        public void Write(CrimeRecord record)
        {
            // Single write is handled by WriteAll for efficiency
        }

        public void WriteAll(IEnumerable<CrimeRecord> records)
        {
            Console.WriteLine("=== Kafka Output Strategy ===");
            Console.WriteLine($"Connecting to Kafka at {_bootstrapServers}...");
            Console.WriteLine($"Target topic: {_topicName}\n");

            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                Acks = Acks.Leader
            };

            using var producer = new ProducerBuilder<string, string>(config).Build();

            int count = 0;
            foreach (var record in records)
            {
                var json = JsonSerializer.Serialize(record);
                var message = new Message<string, string>
                {
                    Key = record.Id,
                    Value = json
                };

                producer.Produce(_topicName, message, report =>
                {
                    if (report.Error.IsError)
                        Console.WriteLine($"  [ERROR] Failed to deliver {record.Id}: {report.Error.Reason}");
                });

                count++;

                if (count % 100 == 0)
                    Console.WriteLine($"  Sent {count} records...");
            }

            // Wait for all messages to be delivered
            producer.Flush(TimeSpan.FromSeconds(10));

            Console.WriteLine($"\n=== Done: {count} records sent to Kafka topic '{_topicName}' ===");
        }
    }
}
