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
                if ((temp.Children.Where(item => item.Value.ToString() == (word[i].ToString())).ToList().Count == 0))
                {
                    temp.Children.Add(new TrieNode(word[i], temp));
                    Console.WriteLine("Added under root " + temp.Children.ElementAt(temp.Children.Count - 1).Value + " Parent " + temp.Children[temp.Children.Count - 1].Parent.Value
                    + " End of word " + temp.Children[temp.Children.Count - 1].IsEndOfWord);
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

        public bool CheckIfExists(TrieNode root, string prefix, int index, List<TrieNode> visitedNodes)
        {
            List<TrieNode> childs = root.Children;

            foreach (var child in childs)
            {

                if ((index >=0  && index < prefix.Length) && child.Value == prefix[index])
                {
                    Console.WriteLine(child.Value + " - " + prefix[index]);
                    index++;
                    visitedNodes.Add(child);
                    CheckIfExists(child, prefix, index, visitedNodes);
                }
            }

            return index > 0;
        }
    }

}
