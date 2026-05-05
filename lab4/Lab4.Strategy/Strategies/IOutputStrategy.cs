using Lab4.Strategy.Models;

namespace Lab4.Strategy.Strategies
{
    /// <summary>
    /// Strategy interface — defines the contract for outputting crime data.
    /// Implementations can write to Console, Kafka, Redis, or any other storage.
    /// </summary>
    public interface IOutputStrategy
    {
        void Write(CrimeRecord record);
        void WriteAll(IEnumerable<CrimeRecord> records);
    }
}
