using System;
using System.IO;

namespace LabogCheat
{
    public class Program
    {
        public static int Main()
        {
            var dico = new Words();
            Console.Out.WriteLine("Entrez la liste de lettres de la grille (jointives, dans le sens de la lecture) : ");
            string input = Console.ReadLine();
            var grid = new Grid(input.Trim().ToLowerInvariant());

            while (true)
            {
                Console.WriteLine("entrez la lettre de départ voulue (STOP pour quitter): ");
                string letter = Console.ReadLine().Trim().ToLowerInvariant();
                if (letter.ToUpperInvariant() == "STOP")
                {
                    Console.WriteLine("au revoir");
                    return 0;
                }
                else if (letter.Length > 1)
                {
                    Console.WriteLine("entrée invalide (une seule lettre à la fois), essayez encore");
                    continue;
                }
                else
                {
                    var words = Solve(grid, dico, letter[0]);
                    Console.WriteLine($"voici les mots commençant par {letter.ToUpper()} dans votre grille : ");
                    if (!words.Any())
                    {
                        Console.WriteLine($"aucun mot trouvé commençant par {letter}");
                    }
                    else
                    {
                        foreach (var w in words)
                        {
                            Console.WriteLine($"\t{w}");
                        }
                    }
                }
            }
        }

        static IEnumerable<string> Solve(Grid grid, Words dico, char letter)
        {
            if (!grid.Contains(letter))
            {
                return new List<string>();
            }

            var result = new HashSet<string>();
            List<(int, int)> start = grid.Positions(letter);
            foreach (var (i, j) in start)
            {
                SubSolve(grid, dico, i, j, result);
            }
            return result;
        }

        static void SubSolve(Grid grid, Words dico, int i, int j, HashSet<string> foundWords)
        {
            var currentPath = new List<(int, int)> { (i, j) };
            SubSolve(grid, dico, currentPath, foundWords);
        }

        static void SubSolve(Grid grid, Words dico, List<(int, int)> currentPath, HashSet<string> foundWords)
        {
            var currentWord = grid.FromPath(currentPath);
            if (currentWord.Length > 3 && dico.Contains(currentWord))
            {
                foundWords.Add(currentWord);
            }
            var (x, y) = currentPath.Last();
            if (x > 0 && y > 0 && !currentPath.Contains((x - 1, y - 1)))
            {
                var newPath = new List<(int, int)>(currentPath) { (x - 1, y - 1) };
                SubSolve(grid, dico, newPath, foundWords);
            }
            if (x > 0 && !currentPath.Contains((x - 1, y)))
            {
                var newPath = new List<(int, int)>(currentPath) { (x - 1, y) };
                SubSolve(grid, dico, newPath, foundWords);
            }
            if (x > 0 && y < 3 && !currentPath.Contains((x - 1, y + 1)))
            {
                var newPath = new List<(int, int)>(currentPath) { (x - 1, y + 1) };
                SubSolve(grid, dico, newPath, foundWords);
            }
            if (y > 0 && !currentPath.Contains((x, y - 1)))
            {
                var newPath = new List<(int, int)>(currentPath) { (x, y - 1) };
                SubSolve(grid, dico, newPath, foundWords);
            }
            if (y > 0 && x < 3 && !currentPath.Contains((x + 1, y - 1)))
            {
                var newPath = new List<(int, int)>(currentPath) { (x + 1, y - 1) };
                SubSolve(grid, dico, newPath, foundWords);
            }
            if (y < 3 && !currentPath.Contains((x, y + 1)))
            {
                var newPath = new List<(int, int)>(currentPath) { (x, y + 1) };
                SubSolve(grid, dico, newPath, foundWords);
            }
            if (x < 3 && y < 3 && !currentPath.Contains((x + 1, y + 1)))
            {
                var newPath = new List<(int, int)>(currentPath) { (x + 1, y + 1) };
                SubSolve(grid, dico, newPath, foundWords);
            }
            if (x < 3 && !currentPath.Contains((x + 1, y)))
            {
                var newPath = new List<(int, int)>(currentPath) { (x + 1, y) };
                SubSolve(grid, dico, newPath, foundWords);
            }
        }
    }
}