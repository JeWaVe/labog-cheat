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
            public bool isEndOfWord;

            public Dictionary<char, Node> childs;

            public Node()
            {
                childs = [];
                isEndOfWord = false;
            }

            public Node(char a)
            {
                letter = a;
                childs = [];
                isEndOfWord = false;
            }
        }

        readonly Node root;

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
                if (!node.childs.TryGetValue(c, out Node? next))
                {
                    next = new Node(c);
                    node.childs.Add(c, next);
                }

                node = next;
            }

            node.isEndOfWord = true;
        }

        public ContainsType Contains(string word)
        {
            var node = root;
            foreach (char c in word)
            {
                if (!node.childs.TryGetValue(c, out Node? next))
                {
                    return ContainsType.No;
                }

                node = next;
            }

            return node.isEndOfWord ? ContainsType.AsWord : ContainsType.AsPrefix;
        }
    }
}