using System;
using System.IO;
using System.Text;
using System.Globalization;

namespace LabogCheat.Import
{
    public class Program
    {
        public static void Main()
        {
            Grammalecte();
            Gutenberg();
        }

        private static void Gutenberg() {
            var lines = File.ReadAllLines("gutenberg_raw.txt");
            File.WriteAllLines("../cheat/gutenberg.txt", lines.Select(l => RemoveDiacritics(l)));
        }

        private static void Grammalecte()
        {
            var lines = File.ReadAllLines("./grammalecte_raw.txt");
            var result = new List<string>();
            foreach (var line in lines)
            {
                List<string> toInsert = new List<string>();
                var lowerLine = line.Trim().ToLowerInvariant();
                if (lowerLine[0] < 'z' && lowerLine[0] >= 'a')
                {
                    var last = lowerLine.IndexOf('/');
                    if (last != -1)
                    {
                        toInsert.Add(lowerLine[0..last]);
                        if (lowerLine.Contains("/s"))
                        {
                            toInsert.Add(lowerLine[0..last] + "s");
                        }
                    }
                    else
                    {
                        toInsert.Add(lowerLine);
                    }
                }
                result.AddRange(toInsert.Where(s => s.Length > 3).Select(s => RemoveDiacritics(s)));
            }

            File.WriteAllLines("../cheat/grammalecte.txt", result);
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }
    }
}