using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        public void CrawlLinks(string url)
        {
            Counter++;
            var HtmlDoc = Web.Load(url);
            string title = HtmlDoc.DocumentNode.SelectSingleNode("//head/title").InnerText.ToString();
            var links = HtmlDoc.DocumentNode.Descendants("a").Select(item => item.GetAttributeValue("href", "")).ToList();
            
            VisitedLinks.Add(url);
            Titles.Add(title);
            IEnumerable<string> titleTokens = Tokenisation(title);

            //add to tree

            foreach (var link in links)
            {
                if(!VisitedLinks.Contains(link) && link.Contains("https://" + GetDomainName(url)) && link.Contains(".com"))
                {
                    CrawlLinks(link);
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
