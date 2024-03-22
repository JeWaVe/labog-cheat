namespace WordleFr {
    internal class Probabilities {
        public static float ComputeEntropy(string word, IEnumerable<string> remaining) {
            var probas = new float[243];
            var patternId = 0;
            foreach(var pattern in Pattern.All) {
                foreach(var attempt in remaining) {
                    if(pattern.Match(word, attempt)) {
                        probas[patternId] += 1;
                    }
                }
                probas[patternId] /= remaining.Count();
                patternId += 1;
            }

            float result = 0.0F;
            for(int i = 0; i < 243; ++i) {
                if(probas[i] != 0) {
                    result -= probas[i] * (float) Math.Log2(probas[i]);
                }
            }

            return result;
        }

        public static Dictionary<string, float> ComputeEntropies(List<string> remaining)
        { 
            var dict = new Dictionary<string, float>();
            foreach (var word in remaining)
            {
                dict.Add(word, ComputeEntropy(word, remaining));
            }

            return dict.OrderByDescending(kvp => kvp.Value).ToDictionary();
        }

        public static Dictionary<char, float> ComputeLetterFrequencies(List<string> words) {
            Dictionary<char, int> count = [];
            int total = 0;
            foreach(var word in words) {
                foreach(char c in word) {
                    count.TryAdd(c, 0);
                    count[c] += 1;
                    total += 1;
                }
            }

            Dictionary<char, float> result = [];
            foreach(var kvp in count) {
                result[kvp.Key] = kvp.Value / (float) total;
            }

            return result;
        }
    }
}