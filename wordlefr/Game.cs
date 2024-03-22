namespace WordleFr {
    internal class Game {
        readonly string _solution;

        List<string> _candidates;
        Dictionary<string, float> _entropy;

        public Game(string word, IEnumerable<string> allWords, Dictionary<string, float> entropy) {
            _solution = word;
            _candidates = new List<string>(allWords);
            _entropy = new Dictionary<string, float>(entropy);
        }

        public bool Play() {
            if(!_entropy.Any()) {
                Console.WriteLine($"ERROR - no solution for {_solution}");
                Environment.Exit(6);
            }
            string attempt = _entropy.First().Key;
            var pattern = new Pattern(_solution, attempt);
            if(pattern.IsWin()) {
                return true;
            }

            _candidates = _candidates.Where(c => pattern.Match(attempt, c)).ToList();
            _entropy = Probabilities.ComputeEntropies(_candidates);
            return false;
        }

        public int Solve() {
            int step = 0;
            while(!Play()) {
                step++;
            }
            return step;
        }
    }
}