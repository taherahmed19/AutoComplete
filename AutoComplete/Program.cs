using AutoComplete.Trie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AutoComplete
{
    class Program
    {
        static void Main(string[] args)
        {
            TrieNode root = new TrieNode(' ', null);

            Console.WriteLine("Running...");
            AutoComplete.Crawler.Crawler crawler = new AutoComplete.Crawler.Crawler();
            crawler.CrawlLinks("https://stackoverflow.com/questions/8497673/html-agility-pack-parsing-an-href-tag?noredirect=1&lq=1", root);
            crawler.GetDomainName("https://stackoverflow.com/questions/8497673/html-agility-pack-parsing-an-href-tag?noredirect=1&lq=1");
            crawler.PrintVisitedLinks();

           /* root.AddWord(root, "secon");
            root.AddWord(root, "second person");
            root.AddWord(root, "word");
            root.AddWord(root, "Organize and share knowledge across your company");
            root.AddWord(root, "developer job");
            root.AddWord(root, "Log In Stack Overflow");
            root.AddWord(root, "Developer jobs");
            root.AddWord(root, "Where");
            root.AddWord(root, "who");
            root.AddWord(root, "HTML Agility pack: parsing an href tag");
            root.AddWord(root, "Convert uppercase characters in strings to lowercase");
            root.AddWord(root, "Convert uppercase ");
            root.AddWord(root, "I am counting my calories, yet I really want dessert.");
            root.AddWord(root, "It was getting dark, and we weren’t there yet.");
            root.AddWord(root, "The shooter says goodbye to his love.");
            root.AddWord(root, "I hear that Nancy is very pretty.");*/
            //root.AddWord(root, FilterRegex("we people who code"));


            //DFS(root);
            AutoComplete.Trie.Trie trie = new AutoComplete.Trie.Trie();

            string input = "";
            do
            {
                input = Console.ReadLine();
                Console.WriteLine("Printing Suggestions");
                trie.PrintAutoSuggestions(root, input);
            } while (input != "exit");

        }

        public static string FilterRegex(string filter)
        {
            var regex = @"[a-zA-Z]+";
            IEnumerable<string> words = Regex.Matches(filter, regex).Cast<Match>().Select(m => m.Value);
            string str = "";
            foreach(var word in words)
            {
                str += word + " ";
            }
            Console.WriteLine(str.ToLower() + "...");

            return str;
        }

        public static void DFS(TrieNode root)
        {
            List<TrieNode> childs = root.Children;
            foreach (var child in childs)
            {
                Console.WriteLine(child.Value);
                if (child.Children.Count != 0)
                {
                    DFS(child);
                }
            }
        }


    }
}
