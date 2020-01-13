using AutoComplete.Crawler;
using AutoComplete.Summarization;
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
            Options options = new Options();
            AutoComplete.Crawler.Crawler crawler = new AutoComplete.Crawler.Crawler();

            Console.WriteLine("Running...");
           // crawler.CrawlLinks("https://stackoverflow.com/questions/8497673/html-agility-pack-parsing-an-href-tag?noredirect=1&lq=1", root);
            crawler.GetDomainName("https://stackoverflow.com/questions/8497673/html-agility-pack-parsing-an-href-tag?noredirect=1&lq=1");
            crawler.PrintVisitedLinks();

            root.AddWord(root, "secon");
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
            root.AddWord(root, "I hear that Nancy is very pretty.");

            Paragraph _Paragraph = crawler.CrawlParagraphs("https://thenextweb.com/apps/2013/03/21/swayy-discover-curate-content/");
            Summarizer summarizer = new Summarizer();

            foreach(var paragraph in _Paragraph._Paragraph.ToList())
            {
                summarizer.Run(paragraph);
            }

            foreach(var item in summarizer.SummarizedText.ToList())
            {
                root.AddWord(root,item);
            }


            Console.WriteLine();
            Console.WriteLine("Origianl Length: " + _Paragraph.Counter);
            Console.WriteLine("Summary Length: " + summarizer.Counter);
            Console.WriteLine("Summary Ratio: " + Math.Round((Convert.ToDouble(_Paragraph.Counter) / Convert.ToDouble(summarizer.Counter)),2));

            //DFS(root);
            AutoComplete.Trie.Trie trie = new AutoComplete.Trie.Trie();

            string input = "";
            do
            {
                input = Console.ReadLine();
                Console.WriteLine("Printing Suggestions");
                trie.PrintAutoSuggestions(root, input);
                options.AddAutoCompleteOptions(trie.suggestions);
                new JsonObject(options.Values).Serialize();
            } while (input != "exit");

            


        }

        public string FilterRegex(string filter)
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
