namespace US_Elections.Services
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using US_Elections.Models;

    public class ElectionDataService
    {
        private readonly List<Election> _elections;
        private StateService _stateService;

        public ElectionDataService()
        {
            var json = File.ReadAllText("Data/electionData.json");
            _elections = JsonConvert.DeserializeObject<List<Election>>(json);
            _stateService = new StateService();
        }

        public IEnumerable<int> GetAllYears() => _elections.Select(e => e.Year);

        public Election GetElectionByYear(int year) => _elections.FirstOrDefault(e => e.Year == year);

        public IEnumerable<string> GetAllStates() => _elections.SelectMany(e => e.States).Select(s => s.S).Distinct();

        public State GetStateByAbbreviation(string abbreviation) =>
            _elections.SelectMany(e => e.States).FirstOrDefault(s => s.S.Equals(abbreviation, StringComparison.OrdinalIgnoreCase));

        public Candidate GetCandidateByYearAndParty(int year, string party)
        {
            Election election = GetElectionByYear(year);
            return election?.Candidates.FirstOrDefault(c => c.Party.Equals(party, StringComparison.OrdinalIgnoreCase));
        }

        private Candidate GetCandidateByParty(Election election, string party)
        {
            return election.Candidates.FirstOrDefault(c => c.Party.Equals(party, StringComparison.OrdinalIgnoreCase));
        }

        public (double Democratic, double Republican, double Other) GetElectoralVotesByYear(int year)
        {
            var election = GetElectionByYear(year);
            if (election == null) return (0, 0, 0);

            double totalElectoralVotes = election.States.Sum(s => s.R.Sum(r => r.E));
            if (totalElectoralVotes == 0) return (0, 0, 0);

            double democraticVotes = 0;
            double republicanVotes = 0;
            double otherVotes = 0;

            var democraticCandidate = GetCandidateByParty(election, "Democrat");
            var republicanCandidate = GetCandidateByParty(election, "Republican");

            foreach (var state in election.States)
            {
                foreach (var result in state.R)
                {
                    if (result.ID == democraticCandidate?.ID)
                        democraticVotes += result.E;
                    else if (result.ID == republicanCandidate?.ID)
                        republicanVotes += result.E;
                    else
                        otherVotes += result.E;
                }
            }

            return (
                Democratic: Math.Round((democraticVotes / totalElectoralVotes) * 100, 2),
                Republican: Math.Round((republicanVotes / totalElectoralVotes) * 100, 2),
                Other: Math.Round((otherVotes / totalElectoralVotes) * 100, 2)
            );
        }

        public (double Democratic, double Republican, double Other) GetPopularVotesByYear(int year)
        {
            var election = GetElectionByYear(year);
            if (election == null) return (0, 0, 0);

            double totalPopularVotes = election.States.Sum(s => s.R.Sum(r => r.V));
            if (totalPopularVotes == 0) return (0, 0, 0);

            double democraticVotes = 0;
            double republicanVotes = 0;
            double otherVotes = 0;

            var democraticCandidate = GetCandidateByParty(election, "Democrat");
            var republicanCandidate = GetCandidateByParty(election, "Republican");

            foreach (var state in election.States)
            {
                foreach (var result in state.R)
                {
                    if (result.ID == democraticCandidate?.ID)
                        democraticVotes += result.V;
                    else if (result.ID == republicanCandidate?.ID)
                        republicanVotes += result.V;
                    else
                        otherVotes += result.V;
                }
            }

            return (
                Democratic: Math.Round((democraticVotes / totalPopularVotes) * 100, 2),
                Republican: Math.Round((republicanVotes / totalPopularVotes) * 100, 2),
                Other: Math.Round((otherVotes / totalPopularVotes) * 100, 2)
            );
        }

        public int GetElectoralVotesByYearAndParty(int year, string party)
        {
            var election = GetElectionByYear(year);
            if (election == null) return 0;

            var candidate = GetCandidateByParty(election, party);
            if (candidate == null) return 0;

            return election.States.Sum(s => s.R.Where(r => r.ID == candidate.ID).Sum(r => r.E));
        }

        public int GetPopularVotesByYearAndParty(int year, string party)
        {
            var election = GetElectionByYear(year);
            if (election == null) return 0;

            var candidate = GetCandidateByParty(election, party);
            if (candidate == null) return 0;

            return election.States.Sum(s => s.R.Where(r => r.ID == candidate.ID).Sum(r => r.V));
        }

        public IEnumerable<StateVoteResult> GetPopularVotesByStateForYear(int year)
        {
            var election = GetElectionByYear(year);
            if (election == null) return Enumerable.Empty<StateVoteResult>();

            var democraticCandidate = GetCandidateByParty(election, "Democrat");
            var republicanCandidate = GetCandidateByParty(election, "Republican");

            var stateVoteResults = new List<StateVoteResult>();

            foreach (var state in election.States)
            {
                var stateName = _stateService.AbbreviationToName(state.S);
                var democraticVotes = state.R.FirstOrDefault(r => r.ID == democraticCandidate?.ID)?.V ?? 0;
                var republicanVotes = state.R.FirstOrDefault(r => r.ID == republicanCandidate?.ID)?.V ?? 0;
                //var otherVotes = state.R.Where(r => r.ID != democraticCandidate?.ID && r.ID != republicanCandidate?.ID).Sum(r => r.V);

                stateVoteResults.Add(new StateVoteResult
                {
                    StateName = stateName,
                    DemocraticVotes = democraticVotes,
                    RepublicanVotes = republicanVotes
                });
            }

            return stateVoteResults;
        }

        public IEnumerable<CandidateVoteResult> GetVotesByCandidateForYear(int year)
        {
            var election = GetElectionByYear(year);
            if (election == null) return Enumerable.Empty<CandidateVoteResult>();

            double totalElectoralVotes = election.States.Sum(s => s.R.Sum(r => r.E));
            double totalPopularVotes = election.States.Sum(s => s.R.Sum(r => r.V));

            var candidateVoteResults = new List<CandidateVoteResult>();

            foreach (var candidate in election.Candidates)
            {
                int electoralVotes = election.States.Sum(s => s.R.FirstOrDefault(r => r.ID == candidate.ID)?.E ?? 0);
                int popularVotes = election.States.Sum(s => s.R.FirstOrDefault(r => r.ID == candidate.ID)?.V ?? 0);

                double electoralPercentage = (totalElectoralVotes > 0) ? (electoralVotes / totalElectoralVotes) * 100 : 0;
                double popularPercentage = (totalPopularVotes > 0) ? (popularVotes / totalPopularVotes) * 100 : 0;

                candidateVoteResults.Add(new CandidateVoteResult
                {
                    Party = candidate.Party,
                    CandidateName = candidate.Name,
                    ElectoralVotesNumber = electoralVotes,
                    ElectoralVotesPercentage = Math.Round(electoralPercentage, 2),
                    PopularVotesNumber = popularVotes,
                    PopularVotesPercentage = Math.Round(popularPercentage, 2)
                });
            }

            return candidateVoteResults;
        }
    }
}
