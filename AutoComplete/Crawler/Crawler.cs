using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AutoComplete.Trie;

namespace AutoComplete.Crawler
{
    class Crawler 
    {
        public HtmlWeb Web { get; set; }
        public List<string> VisitedLinks { get; set; }
        public List<string> Titles { get; set; }
        public int Counter = 0;

        public Crawler()
        {
            Web = new HtmlWeb();
            VisitedLinks = new List<string>();
            Titles = new List<string>();
        }

        public void CrawlLinks(string url, TrieNode root)
        {
            if (Counter > 10) return;
            Counter++;
            var HtmlDoc = Web.Load(url);
            string title = HtmlDoc.DocumentNode.SelectSingleNode("//head/title").InnerText.ToString();
            string col = "";

            if(HtmlDoc.DocumentNode.SelectSingleNode("//h1") == null)
            {
                col = HtmlDoc.DocumentNode.SelectSingleNode("//head/title").InnerText;
            }
            else
            {
                col = HtmlDoc.DocumentNode.SelectSingleNode("//h1").InnerText;
            }

            var links = HtmlDoc.DocumentNode.Descendants("a").Select(item => item.GetAttributeValue("href", "")).ToList();
            
            VisitedLinks.Add(url);

            IEnumerable<string> titleTokens = Tokenisation(col);
            if (!Titles.Contains(col)) Titles.Add(col); root.AddWord(root, col);
            //add to tree

            foreach (var link in links)
            {
                if(!VisitedLinks.Contains(link) && link.Contains("https://" + GetDomainName(url)) && link.Contains(".com"))
                {
                    CrawlLinks(link, root);
                }
            }
        }

        public string GetDomainName(string url)
        {
            int pFrom = url.IndexOf("https://") + "https://".Length;
            int pTo = url.LastIndexOf(".");

            string result = url.Substring(pFrom, pTo - pFrom);

            return result;
        }

        public void PrintVisitedLinks()
        {
            foreach(var title in Titles)
            {
                Console.WriteLine(title);
            }
        }


        public IEnumerable<string> Tokenisation(string title)
        {
            var regex = "\\w+";
            IEnumerable<string> words = Regex.Matches(title, regex).Cast<Match>().Select(m => m.Value);

            return words;
        }

        public IEnumerable<string> ExtractLetter(string word)
        {
            var regex = "\\w{1}";
            IEnumerable<string> letters = Regex.Matches(word, regex).Cast<Match>().Select(m => m.Value);

            return letters;
        }
    }
}
