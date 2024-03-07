using System.IO;
using System.Collections.Generic;
using System;

namespace LabogCheat {
    public class Words : HashSet<string> {
        public Words() {
            var all = File.ReadAllLines("gutenberg.txt");
            foreach(var w in all) {
                this.Add(w.ToLowerInvariant());
            }
        }
    }
}