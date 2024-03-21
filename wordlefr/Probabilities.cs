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
    }
}