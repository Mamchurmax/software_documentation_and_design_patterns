using Lab4.Strategy.Models;
using Lab4.Strategy.Strategies;

namespace Lab4.Strategy.Services
{
    /// <summary>
    /// Context class in the Strategy pattern.
    /// Reads data using IDataReader and outputs using IOutputStrategy.
    /// The context does NOT know which strategy is being used — it works through the interface.
    /// </summary>
    public class DataProcessor
    {
        private readonly IDataReader _dataReader;
        private readonly IOutputStrategy _outputStrategy;

        public DataProcessor(IDataReader dataReader, IOutputStrategy outputStrategy)
        {
            _dataReader = dataReader;
            _outputStrategy = outputStrategy;
        }

        public void Process(string filePath)
        {
            Console.WriteLine($"Reading data from: {filePath}");

            var records = _dataReader.ReadAll(filePath);
            var recordList = records.ToList();

            Console.WriteLine($"Read {recordList.Count} records.\n");

            // Delegate output to the strategy — the processor doesn't care WHERE data goes
            _outputStrategy.WriteAll(recordList);
        }
    }
}
