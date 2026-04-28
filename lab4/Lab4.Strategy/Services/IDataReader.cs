using Lab4.Strategy.Models;

namespace Lab4.Strategy.Services
{
    /// <summary>
    /// Interface for reading crime data from a source.
    /// </summary>
    public interface IDataReader
    {
        IEnumerable<CrimeRecord> ReadAll(string filePath);
    }
}
