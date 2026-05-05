using Google.Cloud.Firestore;
using Lab4.Strategy.Models;

namespace Lab4.Strategy.Strategies
{
    /// <summary>
    /// Concrete Strategy: stores crime records in Google Cloud Firestore.
    /// Each record is stored as a document in the "crimes" collection.
    /// </summary>
    public class FirestoreOutputStrategy : IOutputStrategy
    {
        private readonly string _projectId;
        private readonly string _collectionName;

        public FirestoreOutputStrategy(string projectId, string collectionName = "crimes")
        {
            _projectId = projectId;
            _collectionName = collectionName;
        }

        public void Write(CrimeRecord record)
        {
            // Handled in WriteAll for efficiency
        }

        public void WriteAll(IEnumerable<CrimeRecord> records)
        {
            Console.WriteLine("=== Firestore Output Strategy ===");
            Console.WriteLine($"Connecting to Firestore project '{_projectId}'...");
            Console.WriteLine($"Target collection: {_collectionName}\n");

            var db = FirestoreDb.Create(_projectId);

            Console.WriteLine("Connected!\n");

            var collection = db.Collection(_collectionName);
            int count = 0;

            foreach (var record in records)
            {
                var docData = new Dictionary<string, object>
                {
                    { "id", record.Id },
                    { "case_number", record.CaseNumber },
                    { "date", record.Date },
                    { "block", record.Block },
                    { "primary_type", record.PrimaryType },
                    { "description", record.Description },
                    { "location_description", record.LocationDescription },
                    { "arrest", record.Arrest },
                    { "domestic", record.Domestic },
                    { "district", record.District },
                    { "ward", record.Ward },
                    { "year", record.Year }
                };

                // Use crime ID as document ID for easy lookup
                collection.Document(record.Id).SetAsync(docData).GetAwaiter().GetResult();
                count++;

                if (count % 50 == 0)
                    Console.WriteLine($"  Stored {count} records...");
            }

            Console.WriteLine($"\n=== Done: {count} records stored in Firestore collection '{_collectionName}' ===");
            Console.WriteLine("Check your Firebase Console → Firestore to see the data!");
        }
    }
}
