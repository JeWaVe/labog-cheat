namespace WordleFr {
    public class Program {
        public static void Main() {
            Console.WriteLine("Hello\n");
            var words = File.ReadAllLines("mots.txt").Select(l => l.Trim().ToUpperInvariant()).Distinct();
            var frequencies = ComputeFrequencies(words);
            var scores = Scores(words, frequencies);
            var bests = scores.OrderByDescending(kvp => kvp.Value).Take(4).ToArray();
            Console.WriteLine("Les meilleurs mots pour démarrer sont : ");
            foreach(var kvp in bests) {
                Console.WriteLine($"\t{kvp.Key}");
            }

            var candidates = new List<string>(words);
            while(true)
            {
                Console.WriteLine("Entrez le mot que vous avez tapé dans le jeu :");
                string attempt = ReadWord();
                Console.WriteLine("Entre la réponse du jeu (0 pour gris, 1 pour orange, 2 pour vert, sans espace)");
                string score = ReadScore();
                candidates = FilterCandidates(candidates, attempt, score);
                frequencies = ComputeFrequencies(candidates);
                scores = Scores(candidates, frequencies);
                candidates = candidates.OrderByDescending(candidate => scores[candidate]).ToList();
                if(candidates.Count > 1) {
                    Console.WriteLine("Les meilleurs candidats suivants sont : ");
                    Console.Write(string.Join(", ", candidates.Take(5)));
                } else if (candidates.Count == 1) {
                    Console.WriteLine($"La solution est {candidates.First()}");
                    Console.WriteLine("au revoir");
                    return;
                } else if (!candidates.Any()) {
                    Console.WriteLine("Pas de solution possible, au revoir");
                    return;
                }
            }
        }

        private static string ReadWord()
        {
            string attempt;
            while (true)
            {
                var rawInput = Console.ReadLine();
                if (string.IsNullOrEmpty(rawInput))
                {
                    Console.WriteLine("entrée invalide, recommencez");
                    continue;
                }
                attempt = rawInput.Trim().ToUpperInvariant();
                if (string.IsNullOrEmpty(attempt) || attempt.Length != 5 || attempt.Any(c => c < 'A' || c > 'Z'))
                {
                    Console.WriteLine("entrée invalide, recommencez");
                    continue;
                }
                break;
            }

            return attempt;
        }

        private static List<string> FilterCandidates(List<string> candidates, string attempt, string score)
        {
            List<string> result = [];
            foreach(var word in candidates) {
                var insert = true;
                for(int i = 0; i < 5; ++i) {
                    char letter = attempt[i];
                    char s = score[i];
                    if(s == '0') {
                        if(word.Contains(letter)) {
                            Console.WriteLine($"word {word} contains {letter} but should not");
                            insert = false;
                            break;
                        }
                    }
                    else if(s == '1') {
                        if(!word.Contains(letter)) { 
                            Console.WriteLine($"word {word} does not contain {letter} but should");
                            insert = false;
                            break;
                        }
                        if(word[i] == letter) {
                            Console.WriteLine($"word {word} has {letter} at index {i} but should not");
                            insert = false;
                            break;
                        }
                    }
                    else if (s == '2') {
                        if(word[i] != letter) {
                            Console.WriteLine($"word {word} has not {letter} at index {i} but should");
                            insert = false;
                            break;
                        }
                    }
                    else {
                        Environment.Exit(6); // abort - program inconsistent
                    }
                }
                if(insert) {
                    result.Add(word);
                }
            }

            return result;
        }

        private static string ReadScore()
        {
            string result;
            while (true)
            {
                var rawInput = Console.ReadLine();
                if (string.IsNullOrEmpty(rawInput))
                {
                    Console.WriteLine("entrée invalide, recommencez");
                    continue;
                }
                result = rawInput.Trim();
                if (string.IsNullOrEmpty(result) || result.Length != 5 || result.Any(c => c < '0' || c > '2'))
                {
                    Console.WriteLine("entrée invalide, recommencez");
                    continue;
                }
                break;
            }

            return result;
        }

        static double[] ComputeFrequencies(IEnumerable<string> words) {
            var result = new int[26];
            double total = 0.0;
            foreach(var word in words) {
                foreach(var c in word) {
                    result[c - 'A'] += 1;
                    total += 1;
                }
            }

            return result.Select(f => f / total).ToArray();;
        }

        static Dictionary<string, double> Scores(IEnumerable<string> words, double[] freqs) {
            var result = new Dictionary<string, double>();
            foreach(var w in words) {
                double score = 0.0;
                foreach(var c in w.Distinct()) {
                    score += freqs[c - 'A'];
                }
                result.Add(w, score);
            }

            return result;
        }
    }
}