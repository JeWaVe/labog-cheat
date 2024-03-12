using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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
            while(true) {
                Console.WriteLine("entrez le mot que vous avez tapé dans le jeu :");
                var attempt = Console.ReadLine().Trim().ToUpperInvariant();
                Console.WriteLine("entre la réponse du jeu (0 pour gris, 1 pour orange, 2 pour vert, sans espace)");
                var result = Console.ReadLine().Trim();
                for(int i = 0; i < 5; ++i) {
                    var letter = attempt[i];
                    var s = result[i];
                    switch(s) {
                        // letter is not in solution
                        case '0':
                            candidates = candidates.Where(candidate => !candidate.Contains(letter)).ToList();
                        break;
                        // letter is in the solution
                        case '1':
                        case '2':
                            candidates = candidates.Where(candidate => candidate.Contains(letter)).ToList();
                        break;
                    }
                }

                candidates = candidates.OrderByDescending(candidate => scores[candidate]).ToList();
                Console.WriteLine("best next candidates are : ");
                Console.Write(string.Join(", ", candidates.Take(5)));
            }
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