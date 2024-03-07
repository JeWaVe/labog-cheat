namespace LabogCheat {
    public class Grid {
        string chars {get; set; }

        public Grid(string input) {
            chars = input;
        }

        public override string ToString() {
            String result = "";
            for(int i = 0; i < 4; ++i) {
                for(int j = 0; j < 4; ++j) {
                    result += chars[4*i + j];
                }
                result += "\n";
            }
            return result;
        }

        public char this[int i, int j] => chars[4*i + j];

        public bool Contains(char c) {
            return chars.Contains(c);
        }

        public List<(int, int)> Positions(char c) {
            var result = new List<(int, int)> ();
            int start = 0;
            while(start != -1) {
                int index = chars.IndexOf(c, start);
                if(index == -1) {
                    break;
                }
                result.Add((index / 4, index % 4));
                start = index + 1;
            }

            return result;
        }

        public string FromPath(List<(int, int)> path) {
            var result = "";
            foreach(var (i, j) in path) {
                result  += this[i,j];
            }
            return result;
        }
    }
}