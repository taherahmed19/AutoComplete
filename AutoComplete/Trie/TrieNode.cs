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
            TrieNode temp = root;

            for (int i = 0; i < word.Length; i++)
            {

                int index = word[i];

                if ((temp.Children.Select(item => item.Value.ToString().Equals(word[i].ToString())).ToList().FirstOrDefault() == false))
                {

                    temp.Children.Add(new TrieNode(word[i], this));
                    Console.WriteLine("Added under root " + temp.Children.ElementAt(temp.Children.Count - 1).Value);
                }

                temp = temp.Children[temp.Children.Count - 1];

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
    }

}
