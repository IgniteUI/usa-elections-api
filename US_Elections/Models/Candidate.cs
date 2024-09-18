namespace US_Elections.Models
{
    public class Candidate
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Party { get; set; }
        public string TermStart { get; set; }
        public string TermEnd { get; set; }
        public string Image { get; set; }
        public string ImageFull { get; set; }
    }
}
