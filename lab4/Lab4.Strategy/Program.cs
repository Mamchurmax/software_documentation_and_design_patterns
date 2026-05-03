using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lab4.Strategy.Models;
using Lab4.Strategy.Services;
using Lab4.Strategy.Strategies;

namespace Lab4.Strategy
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // 1. Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var strategyName = configuration["OutputStrategy"] ?? "Console";

            // 2. Setup Dependency Injection with strategy from config
            var services = new ServiceCollection();

            services.AddTransient<IDataReader, CsvDataReader>();

            // Register the output strategy based on configuration — no code changes needed!
            switch (strategyName.ToLowerInvariant())
            {
                case "kafka":
                    services.AddTransient<IOutputStrategy, KafkaOutputStrategy>();
                    break;
                case "redis":
                    services.AddTransient<IOutputStrategy, RedisOutputStrategy>();
                    break;
                case "console":
                default:
                    services.AddTransient<IOutputStrategy, ConsoleOutputStrategy>();
                    break;
            }

            services.AddTransient<DataProcessor>();

            var serviceProvider = services.BuildServiceProvider();

            // 3. Download dataset if not present
            var csvPath = "crimes_data.csv";
            if (!File.Exists(csvPath))
            {
                Console.WriteLine("Downloading Chicago Crimes dataset (1000 rows)...");
                await DownloadDataset(csvPath);
                Console.WriteLine($"Dataset saved to {csvPath}\n");
            }

            // 4. Process data using the Strategy pattern
            Console.WriteLine($"Active strategy: {strategyName}\n");

            var processor = serviceProvider.GetRequiredService<DataProcessor>();
            processor.Process(csvPath);
        }

        static async Task DownloadDataset(string outputPath)
        {
            // Download 1000 records from Chicago Crimes API as CSV
            var url = "https://data.cityofchicago.org/api/views/ijzp-q8t2/rows.csv?accessType=DOWNLOAD";

            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);

            // Use streaming to get a limited amount of data
            // The full dataset is huge (8M+ rows), so we'll read just 1001 lines (header + 1000 rows)
            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);

            int lineCount = 0;
            int maxLines = 1001; // header + 1000 data rows

            while (!reader.EndOfStream && lineCount < maxLines)
            {
                var line = await reader.ReadLineAsync();
                if (line != null)
                {
                    await writer.WriteLineAsync(line);
                    lineCount++;
                }
            }

            Console.WriteLine($"Downloaded {lineCount - 1} data rows.");
        }
    }
}
