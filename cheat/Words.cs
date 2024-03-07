using System.IO;
using System.Collections.Generic;
using System;

namespace LabogCheat {
    public class Words : HashSet<string> {
        public Words() {
            var grammalecte = File.ReadAllLines("grammalecte.txt");
            foreach(var w in grammalecte) {
                this.Add(w.ToLowerInvariant());
            }
            var gutenberg = File.ReadAllLines("gutenberg.txt");
            foreach(var w in gutenberg) {
                this.Add(w.ToLowerInvariant());
            }
        }
    }
}