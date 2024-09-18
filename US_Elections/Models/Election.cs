namespace US_Elections.Models
{
    public class Election
    {
        public int Year { get; set; }
        public bool HasVotes { get; set; }
        public List<Candidate> Candidates { get; set; }
        public List<State> States { get; set; }
    }
}
