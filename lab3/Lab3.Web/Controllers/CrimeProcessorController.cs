using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Lab4.Strategy.Services;
using Lab4.Strategy.Strategies;

namespace Lab3.Web.Controllers
{
    public class CrimeProcessorController : Controller
    {
        // GET: /CrimeProcessor
        public IActionResult Index()
        {
            return View();
        }

        // POST: /CrimeProcessor/Run
        [HttpPost]
        public async Task<IActionResult> Run(string strategy)
        {
            var result = new CrimeProcessorResult();
            var sw = Stopwatch.StartNew();

            try
            {
                // Download CSV if not present
                var csvPath = Path.Combine(AppContext.BaseDirectory, "crimes_data.csv");
                if (!System.IO.File.Exists(csvPath))
                {
                    result.Log += "Downloading Chicago Crimes dataset (1000 rows)...\n";
                    await DownloadCsv(csvPath);
                    result.Log += "Download complete.\n";
                }

                // Create data reader
                var dataReader = new CsvDataReader();

                // Create the strategy based on selection
                IOutputStrategy outputStrategy = strategy?.ToLowerInvariant() switch
                {
                    "kafka" => new KafkaOutputStrategy(),
                    "redis" => new RedisOutputStrategy(),
                    "firestore" => CreateFirestoreStrategy(),
                    _ => new ConsoleOutputStrategy()
                };

                // Process
                var processor = new DataProcessor(dataReader, outputStrategy);
                var records = dataReader.ReadAll(csvPath).ToList();

                outputStrategy.WriteAll(records);

                sw.Stop();

                result.Success = true;
                result.Strategy = strategy ?? "Console";
                result.RecordCount = records.Count;
                result.ElapsedMs = sw.ElapsedMilliseconds;
                result.Log += $"Successfully processed {records.Count} records using {strategy} strategy in {sw.ElapsedMilliseconds}ms.";
            }
            catch (Exception ex)
            {
                sw.Stop();
                result.Success = false;
                result.Strategy = strategy ?? "Console";
                result.ElapsedMs = sw.ElapsedMilliseconds;
                result.Log += $"Error: {ex.Message}";
            }

            return View("Result", result);
        }

        private FirestoreOutputStrategy CreateFirestoreStrategy()
        {
            var keyPath = Path.Combine(AppContext.BaseDirectory, "serviceAccountKey.json");
            if (System.IO.File.Exists(keyPath))
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", keyPath);
            }
            return new FirestoreOutputStrategy("documentation-f0d9d");
        }

        private async Task DownloadCsv(string outputPath)
        {
            var url = "https://data.cityofchicago.org/api/views/ijzp-q8t2/rows.csv?accessType=DOWNLOAD";
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);
            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            using var writer = new StreamWriter(outputPath, false, System.Text.Encoding.UTF8);
            int lineCount = 0;
            while (!reader.EndOfStream && lineCount < 1001)
            {
                var line = await reader.ReadLineAsync();
                if (line != null) { await writer.WriteLineAsync(line); lineCount++; }
            }
        }
    }

    public class CrimeProcessorResult
    {
        public bool Success { get; set; }
        public string Strategy { get; set; } = "";
        public int RecordCount { get; set; }
        public long ElapsedMs { get; set; }
        public string Log { get; set; } = "";
    }
}
