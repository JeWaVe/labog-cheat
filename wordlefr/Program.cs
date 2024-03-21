using System.Runtime.InteropServices;

namespace WordleFr
{
    public class Program {
        public static void Main()
        {
            List<string> candidates = File.ReadAllLines("mots.txt").ToHashSet().ToList();
            var entropy = ComputeInitialEntropies(candidates);
            while(true) {
                Console.WriteLine("Les meilleurs mots sont à ce stade : ");
                Console.WriteLine(string.Join("\n", entropy.Take(5).Select(kvp => kvp.Key + ", " + kvp.Value)));
                Console.WriteLine("Entrez le mot que vous avez joué: ");
                string word = ReadAttempt();
                Console.WriteLine("Entrez le score indiqué par le jeu (jointif), 0 pour gris, 1 pour orange, 2 pour vert : ");
                Pattern score = ReadPattern();
                candidates = candidates.Where(tempt => score.Match(word, tempt)).ToList();
                entropy = ComputeEntropies(candidates);
            }
        }

        private static Dictionary<string, float> ComputeEntropies(List<string> remaining)
        { 
            var dict = new Dictionary<string, float>();
            foreach (var word in remaining)
            {
                dict.Add(word, Probabilities.ComputeEntropy(word, remaining));
            }

            return dict;
        }

        private static Pattern ReadPattern()
        {
            string? rawInput;
            while(true) {
                rawInput = Console.ReadLine();
                if(string.IsNullOrEmpty(rawInput)) {
                    Console.WriteLine("Entrée invalide, recommencez");
                    continue;
                }
                rawInput = rawInput.Trim().ToLowerInvariant();
                if(string.IsNullOrEmpty(rawInput) || rawInput.Length != 5 || rawInput.Any(c => c < '0' || c > '2')) {
                    Console.WriteLine("Entrée invalide, recommencez");
                    continue;
                }
                
                return new Pattern(rawInput.Select(c => (byte) (c - '0')).ToArray());
            }
        }

        private static string ReadAttempt()
        {
            string? rawInput;
            while(true) {
                rawInput = Console.ReadLine();
                if(string.IsNullOrEmpty(rawInput)) {
                    Console.WriteLine("Entrée invalide, recommencez");
                    continue;
                }
                rawInput = rawInput.Trim().ToLowerInvariant();
                if(string.IsNullOrEmpty(rawInput) || rawInput.Length != 5 || rawInput.Any(c => c < 'a' || c > 'z')) {
                    Console.WriteLine("Entrée invalide, recommencez");
                    continue;
                }

                return rawInput.ToUpperInvariant();
            }
        }

        private static Dictionary<string, float> ComputeInitialEntropies(IEnumerable<string> allWords)
        {
            var dict = new Dictionary<string, float>();
            // caching
            if(!File.Exists("entropy.txt")) {
                Console.WriteLine("computing initial scores, this might take some time and deserve optimization, please wait...");
                foreach (var word in allWords)
                {
                    dict.Add(word, Probabilities.ComputeEntropy(word, allWords));
                }

                File.WriteAllLines("entropy.txt", dict.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key + ":" + kvp.Value));
            } 
            else {
                var lines = File.ReadAllLines("entropy.txt");
                foreach(var line in lines) {
                    string[] split = line.Split(':');
                    dict.Add(split[0], float.Parse(split[1]));
                }
            }

            return dict;
        }
    }
}