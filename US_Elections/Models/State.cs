namespace US_Elections.Models
{
    public class State
    {
        public string S { get; set; } // State abbreviation
        public List<StateResult> R { get; set; } // Results
        public string StateName { get; set; } // State name by abbreviation
    }
}
