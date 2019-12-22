using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoComplete.Trie
{
    class TrieNode
    {
        public char Value { get; set; }
        public List<TrieNode> Children { get; set; }
        public TrieNode Parent { get; set; }
        public bool IsEndOfWord { get; set; }

        public TrieNode(char value, TrieNode parent)
        {
            Value = value;
            Children = new List<TrieNode>();
            Parent = parent;
            IsEndOfWord = false;
        }

        public void AddWord(TrieNode root, string word)
        {
            word = word.ToLower();

            TrieNode temp = root;
            for (int i = 0; i < word.Length; i++)
            {
                if ((temp.Children.Where(item => item.Value.ToString().Equals(word[i].ToString(), StringComparison.OrdinalIgnoreCase)).ToList().Count == 0))
                {
                    temp.Children.Add(new TrieNode(word[i], temp));
                    temp = temp.Children[temp.Children.Count - 1];
                }
                else
                {
                    temp = temp.Children.Where(item => item.Value == word[i]).FirstOrDefault();
                }


                if (i == word.Length - 1)
                {
                    temp.IsEndOfWord = true;
                }
            }
        }

        public bool IsLeaf()
        {
            return this.Children.Count == 0;
        }

        public static bool found = false;

        public bool CheckIfExists(TrieNode root, string prefix, int index, List<TrieNode> visitedNodes)
        {
            List<TrieNode> childs = root.Children;
            foreach (var child in childs)
            {

                if ((index >= 0 && index < prefix.Length) && child.Value.ToString().Equals(prefix[index].ToString(), StringComparison.OrdinalIgnoreCase) && !visitedNodes.Contains(child))
                {
                    if (!visitedNodes.Contains(child) && !found)
                    {
                        index++;
                        visitedNodes.Add(child);
                        Console.WriteLine("Last Node " + visitedNodes[visitedNodes.Count - 1].Value);
                        if (index == prefix.Length)
                        {
                            found = true;
                        }
                        CheckIfExists(child, prefix, index, visitedNodes);
                    }
                }
            }

            return visitedNodes[visitedNodes.Count - 1].Value.ToString() == prefix[prefix.Length - 1].ToString();
        }
    }

}
