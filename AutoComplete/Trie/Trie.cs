using System;
using System.Collections.Generic;
using System.Text;

namespace AutoComplete.Trie
{
    class Trie
    {

        public string str { get; set; }
        public List<string> suggestions { get; set; }

        public Trie()
        {
            this.str = "";
            this.suggestions = new List<string>();
        }

        public int PrintAutoSuggestions(TrieNode root, string prefix)
        {
            List<TrieNode> visitedNodes = new List<TrieNode>();
            TrieNode.found = false;

            bool exists = root.CheckIfExists(root, prefix, 0, visitedNodes);
            if (!exists)
            {
                return 0;
            }
            else
            {
                str = "";
                FindWords(visitedNodes[visitedNodes.Count - 1], suggestions, prefix);

                foreach (var suggest in suggestions)
                {
                    Console.WriteLine(suggest);
                }
            }
            return 1;
        }

        public void FindWords(TrieNode root, List<string> suggestions, string prefix)
        {
            List<TrieNode> childs = root.Children;

            if (root.IsEndOfWord && !suggestions.Contains(str) && !string.IsNullOrEmpty(str))
            {
                suggestions.Add(str);
            }

            foreach (var child in childs)
            {
                if (child.IsEndOfWord)
                {
                    str += child.Value;
                    GoToRoot(child, suggestions);
                }

                if (child.Children.Count != 0)
                {
                    FindWords(child, suggestions, prefix);
                }
            }
        }

        public void GoToRoot(TrieNode leaf, List<string> suggestions)
        {
            if (leaf.Parent != null)
            {
                str += leaf.Parent.Value;
                GoToRoot(leaf.Parent, suggestions);
            }
            else
            {
                AddWord(suggestions, str);
                str = "";
            }

        }

        public void AddWord(List<string> suggestions, string str)
        {
            var sb = new StringBuilder(str.Length);
            for (var i = str.Length - 1; i >= 0; --i)
            {
                sb.Append(str[i]);
            }
            string newSb = sb.ToString().TrimStart().TrimEnd();
            suggestions.Add(newSb);
        }
    }
}
