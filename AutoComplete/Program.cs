using AutoComplete.Trie;
using System;
using System.Collections.Generic;

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
            root.AddWord(root, "second");

            root.AddWord(root, "word");

            foreach(var letter in root.Children)
            {
                Console.WriteLine(letter.Value);
            }

        }
    }
}
