namespace US_Elections.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using US_Elections.Models;
    using US_Elections.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class ElectionController : ControllerBase
    {
        private readonly ElectionDataService _service;
        private StateService _stateService;

        public ElectionController()
        {
            _service = new ElectionDataService();
            _stateService = new StateService();
        }

        // get all election years
        [HttpGet("years")]
        public ActionResult<IEnumerable<int>> GetAllYears()
        {
            IEnumerable<int> years = _service.GetAllYears();
            return Ok(years);
        }

        // get election data by year
        [HttpGet("year/{year}")]
        public ActionResult<Election> GetElectionByYear(int year)
        {
            Election election = _service.GetElectionByYear(year);
            if (election == null)
                return NotFound();

            List<Candidate> candidates = election.Candidates;
            foreach (var candidate in candidates)
            {
                candidate.ImageFull = $"{Request.Scheme}://{Request.Host}{candidate.Image}";
            }

            List<State> states = election.States;
            foreach (var state in states)
            {
                state.StateName = _stateService.AbbreviationToName(state.S);
            }

            return Ok(election);
        }

        // get all states
        [HttpGet("states-abbreviation")]
        public ActionResult<IEnumerable<string>> GetAllStates()
        {
            IEnumerable<string> states = _service.GetAllStates();
            return Ok(states);
        }

        // get state information by abbreviation
        [HttpGet("state/{abbreviation}")]
        public ActionResult<State> GetStateByAbbreviation(string abbreviation)
        {
            State state = _service.GetStateByAbbreviation(abbreviation);
            if (state == null)
            {
                return NotFound();
            }
            state.StateName = _stateService.AbbreviationToName(state.S);
            return Ok(state);
        }

        // get the Democratic candidate by year
        [HttpGet("democratic-candidate/{year}")]
        public ActionResult<Candidate> GetDemocraticCandidateByYear(int year)
        {
            Candidate candidate = _service.GetCandidateByYearAndParty(year, "Democrat");
            if (candidate == null)
            {
                return NotFound();
            }
            candidate.ImageFull = $"{Request.Scheme}://{Request.Host}{candidate.Image}";
            return Ok(candidate);
        }

        // get the Republican candidate by year
        [HttpGet("republican-candidate/{year}")]
        public ActionResult<Candidate> GetRepublicanCandidateByYear(int year)
        {
            Candidate candidate = _service.GetCandidateByYearAndParty(year, "Republican");
            if (candidate == null)
            {
                return NotFound();
            }
            candidate.ImageFull = $"{Request.Scheme}://{Request.Host}{candidate.Image}";
            return Ok(candidate);
        }

        // Electoral votes by year as percentage
        [HttpGet("electoral-votes/{year}")]
        public ActionResult<IEnumerable<VoteResult>> GetElectoralVotesByYearForChart(int year)
        {
            (double Democratic, double Republican, double Other) voteResult = _service.GetElectoralVotesByYear(year);

            var candidateVoteResults = new List<VoteResult>();

            candidateVoteResults.Add(new VoteResult
            {
                ResultAsNumber = voteResult.Democratic,
                ResultAsString = voteResult.Democratic.ToString()
            });
            candidateVoteResults.Add(new VoteResult
            {
                ResultAsNumber = voteResult.Republican,
                ResultAsString = voteResult.Republican.ToString()
            });
            candidateVoteResults.Add(new VoteResult
            {
                ResultAsNumber = voteResult.Other,
                ResultAsString = voteResult.Other.ToString()
            });

            return Ok(candidateVoteResults);
        }

        // Popular votes by year as percentage
        [HttpGet("popular-votes/{year}")]
        public ActionResult<IEnumerable<VoteResult>> GetPopularVotesByYear(int year)
        {
            (double Democratic, double Republican, double Other) voteResult = _service.GetPopularVotesByYear(year);

            var candidateVoteResults = new List<VoteResult>();

            candidateVoteResults.Add(new VoteResult
            {
                ResultAsNumber = voteResult.Democratic,
                ResultAsString = voteResult.Democratic.ToString()
            });
            candidateVoteResults.Add(new VoteResult
            {
                ResultAsNumber = voteResult.Republican,
                ResultAsString = voteResult.Republican.ToString()
            });
            candidateVoteResults.Add(new VoteResult
            {
                ResultAsNumber = voteResult.Other,
                ResultAsString = voteResult.Other.ToString()
            });

            return Ok(candidateVoteResults);
        }

        // Electoral votes by year and party (number)
        [HttpGet("electoral-votes/{year}/{party}")]
        public ActionResult<VoteCountResult> GetElectoralVotesByYearAndParty(int year, string party)
        {
            int votes = _service.GetElectoralVotesByYearAndParty(year, party);

            var voteCountResult = new VoteCountResult
            {
                Votes = votes
            };

            return Ok(voteCountResult);
        }

        // Popular votes by year and party (number)
        [HttpGet("popular-votes/{year}/{party}")]
        public ActionResult<VoteCountResult> GetPopularVotesByYearAndParty(int year, string party)
        {
            int votes = _service.GetPopularVotesByYearAndParty(year, party);

            var voteCountResult = new VoteCountResult
            {
                Votes = votes
            };

            return Ok(voteCountResult);
        }

        // gets the votes in 3 categories (democrat, republican, other) by state
        [HttpGet("popular-votes/{year}/by-state")]
        public ActionResult<IEnumerable<StateVoteResult>> GetPopularVotesByStateForYear(int year)
        {
            var results = _service.GetPopularVotesByStateForYear(year);
            if (!results.Any())
            {
                return NotFound();
            }

            return Ok(results);
        }

        [HttpGet("votes/{year}/by-candidate")]
        public ActionResult<IEnumerable<CandidateVoteResult>> GetVotesByCandidateForYear(int year)
        {
            var results = _service.GetVotesByCandidateForYear(year);
            if (!results.Any())
            {
                return NotFound();
            }

            return Ok(results);
        }
    }
}
