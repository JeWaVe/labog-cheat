namespace WordleFr {
    internal class Game {
        readonly string _solution;
        List<string> _attempts;
        int _steps;

        List<string> _candidates;
        Dictionary<string, float> _entropy;
        readonly Dictionary<char, float> _frequencies;

        public Game(string word, IEnumerable<string> allWords, Dictionary<string, float> entropy, Dictionary<char, float> frequencies) {
            _solution = word;
            _steps = 0;
            _attempts = new List<string>();
            _candidates = new List<string>(allWords);
            _entropy = new Dictionary<string, float>(entropy);
            _frequencies = frequencies;
        }

        public bool Play() {
            _steps += 1;
            if(!_entropy.Any()) {
                Console.WriteLine($"ERROR - no solution for {_solution}");
                Environment.Exit(6);
            }
            var allBest = _entropy.Where(_e => _e.Value == _entropy.First().Value).ToList();
            string attempt = _entropy.First().Key;
            if(allBest.Count > 1) {
                float max = 0.0F;
                foreach(var a in allBest) {
                    float score = 0.0F;
                    foreach(char c in a.Key) {
                        score += _frequencies[c];
                    }
                    if(score > max) {
                        max = score;
                        attempt = a.Key;
                    }
                }
            }
            
            _attempts.Add(attempt);
            var pattern = new Pattern(_solution, attempt);
            if(pattern.IsWin()) {
                return true;
            }

            _candidates = _candidates.Where(c => pattern.Match(attempt, c)).ToList();
            _entropy = Probabilities.ComputeEntropies(_candidates);
            return false;
        }

        public override string ToString()
        {
            return String.Format($"{_solution}:{_steps}:") + string.Join(",", _attempts);
        }

        public int Solve() {
            while(!Play()) {}
            return _steps;
        }
    }
}