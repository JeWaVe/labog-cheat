namespace WordleFr
{
    internal class Pattern
    {
        static Pattern[] _patterns = new Pattern[243];
        static Pattern()
        {
            int pIndex = 0;
            byte[] array = new byte[5];
            for (array[0] = 0; array[0] <= 2; array[0]++)
            {
                for (array[1] = 0; array[1] <= 2; array[1]++)
                {
                    for (array[2] = 0; array[2] <= 2; array[2]++)
                    {
                        for (array[3] = 0; array[3] <= 2; array[3]++)
                        {
                            for (array[4] = 0; array[4] <= 2; array[4]++)
                            {
                                _patterns[pIndex++] = new Pattern(array);
                            }
                        }
                    }
                }
            }
        }

        public static Pattern[] All => _patterns;

        readonly byte[] bits;

        public Pattern(byte[] bits)
        {
            this.bits = new byte[5];
            Array.Copy(bits, this.bits, 5);
        }

        // returns true if 'attempt' matches current pattern for 'word'
        public bool Match(string word, string attempt)
        {
            for (int i = 0; i < 5; ++i)
            {
                byte s = bits[i];
                char wx = word[i];
                char ax = attempt[i];
                // letter not present
                if (s == 0)
                {
                    if (attempt.Contains(wx))
                    {
                        return false;
                    }
                }
                // letter present but wrong position
                else if (s == 1)
                {
                    if (ax == wx || !attempt.Contains(wx))
                    {
                        return false;
                    }
                }
                // letter must be at this place
                else if (s == 2)
                {
                    if (ax != wx)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}