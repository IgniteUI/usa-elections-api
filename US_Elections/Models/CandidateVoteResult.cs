namespace US_Elections.Models
{
    public class CandidateVoteResult
    {
        public string Party { get; set; }
        public string CandidateName { get; set; }
        public int ElectoralVotesNumber { get; set; }
        public double ElectoralVotesPercentage { get; set; }
        public int PopularVotesNumber { get; set; }
        public double PopularVotesPercentage { get; set; }
    }
}
