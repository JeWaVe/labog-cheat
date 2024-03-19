using System;
using System.Collections.Generic;

namespace LabogCheat
{
    public class Dictionary
    {

        public enum ContainsType
        {
            No,
            AsPrefix,
            AsWord
        }

        class Node
        {
            public char letter;
            public bool terminator;

            public Dictionary<char, Node> childs;

            public Node()
            {
                childs = new Dictionary<char, Node>();
                terminator = false;
            }

            public Node(char a)
            {
                letter = a;
                childs = new Dictionary<char, Node>();
                terminator = false;
            }
        }

        Node root;

        public Dictionary()
        {
            root = new Node();
        }

        public void Feed(IEnumerable<string> words)
        {
            foreach (string w in words)
            {
                Insert(w);
            }
        }

        public void Insert(string word)
        {
            var node = root;
            foreach (char c in word)
            {
                if (!node.childs.ContainsKey(c))
                {
                    node.childs.Add(c, new Node(c));
                }

                node = node.childs[c];
            }

            node.terminator = true;
        }

        public ContainsType Contains(string word)
        {
            var node = root;
            foreach (char c in word)
            {
                if (!node.childs.ContainsKey(c))
                {
                    return ContainsType.No;
                }

                node = node.childs[c];
            }

            return node.terminator ? ContainsType.AsWord : ContainsType.AsPrefix;
        }
    }
}