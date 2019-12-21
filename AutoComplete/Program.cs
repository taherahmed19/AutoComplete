using AutoComplete.Trie;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoComplete
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            AutoComplete.Crawler.Crawler crawler = new AutoComplete.Crawler.Crawler();
          //  crawler.CrawlLinks("https://stackoverflow.com/questions/8497673/html-agility-pack-parsing-an-href-tag?noredirect=1&lq=1");
            crawler.GetDomainName("https://stackoverflow.com/questions/8497673/html-agility-pack-parsing-an-href-tag?noredirect=1&lq=1");
            crawler.PrintVisitedLinks();
            Console.WriteLine(crawler.Counter);

            List<string> test = new List<string>();

            TrieNode root = new TrieNode(' ', null);
            root.AddWord(root, "secon");
            root.AddWord(root, "second person");
            root.AddWord(root, "word");
            root.AddWord(root, "words");

            //DFS(root);

            PrintAutoSuggestions(root, "s");
        }

        public static void DFS(TrieNode root, List<TrieNode> visitedNodes)
        {
            List<TrieNode> childs = root.Children;
            foreach (var child in childs)
            {
                visitedNodes.Add(child);
                if (child.Children.Count != 0)
                {
                    DFS(child, visitedNodes);
                }

            }
        }

        //if exists get the last node the prefix exists at 
        //then carry out another search to the get the autocomplete values

        public static int PrintAutoSuggestions(TrieNode root, string prefix)
        {
            int length = prefix.Length;
            TrieNode temp = root;

            List<TrieNode> visitedNodes = new List<TrieNode>();
            List<string> suggestions = new List<string>();

            bool exists = root.CheckIfExists(root, prefix, 0, visitedNodes);

            if (!exists)
            {
                return 0;
            }
            else
            {
                Console.WriteLine("lastNode " + visitedNodes[visitedNodes.Count - 1].Value);

                DFS(visitedNodes[visitedNodes.Count - 1], visitedNodes);
                Console.WriteLine();

                string str = "";
                foreach (var visited in visitedNodes)
                {
                    str += visited.Value;
                    if (visited.IsEndOfWord)
                    {
                        suggestions.Add(str);
                    }
                }

                foreach(var suggest in suggestions)
                {
                    Console.WriteLine(suggest);
                }
            }

            return 1;

        }
    }
}
