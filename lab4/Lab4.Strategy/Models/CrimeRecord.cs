namespace Lab4.Strategy.Models
{
    public class CrimeRecord
    {
        public string Id { get; set; } = "";
        public string CaseNumber { get; set; } = "";
        public string Date { get; set; } = "";
        public string Block { get; set; } = "";
        public string PrimaryType { get; set; } = "";
        public string Description { get; set; } = "";
        public string LocationDescription { get; set; } = "";
        public bool Arrest { get; set; }
        public bool Domestic { get; set; }
        public string District { get; set; } = "";
        public string Ward { get; set; } = "";
        public int Year { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Date} | {PrimaryType}: {Description} | {Block} | Arrest: {Arrest}";
        }
    }
}
