using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AutoComplete.Trie;
using System.Threading;
using Newtonsoft.Json;

namespace AutoComplete.Crawler
{

    public class Paragraph
    {
        public List<string> _Paragraph { get; set; }
        public int Counter { get; set; }

        public Paragraph(List<string> _Paragraph, int Counter)
        {
            this._Paragraph = _Paragraph;
            this.Counter = Counter;
        }
    }

    class Crawler 
    {
        public HtmlWeb Web { get; set; }
        public List<string> VisitedLinks { get; set; }
        public List<string> Titles { get; set; }
        public int Counter = 0;
        HtmlDocument HtmlDoc;

        public Crawler()
        {
            Web = new HtmlWeb();
            VisitedLinks = new List<string>();
            Titles = new List<string>();
        }

        public void CrawlLinks(string url, TrieNode root)
        {
            if ((Counter % 10) == 0 && Counter != 0)
            {
                Console.WriteLine("Paused");
                Thread.Sleep(10000);
            }
            Counter++;
            HtmlDoc = Web.Load(url);

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

            Console.WriteLine("Visited " + url);

            VisitedLinks.Add(url);

            IEnumerable<string> titleTokens = Tokenisation(col);
            if (!Titles.Contains(col)) Titles.Add(col); root.AddWord(root, col);

            foreach (var link in links)
            {
                if(!VisitedLinks.Contains(link) && link.Contains("https://" + GetDomainName(url)) && link.Contains(".com"))
                {
                    CrawlLinks(link, root);
                }
            }
        }

        public Paragraph CrawlParagraphs(string url)
        {
            int counter = 0;
            List<string> paragraph = new List<string>();
            HtmlDoc = Web.Load(url);

            HtmlNodeCollection _p = HtmlDoc.DocumentNode.SelectNodes("//p");

            foreach(var p in _p.ToList())
            {
                if (p.InnerText.Length != 0)
                {
                    string normText = p.InnerText.Replace("  ", "");
                    normText = normText.Trim();
                    if (new Regex(@"[.!?\\-]").Match(normText).Success == false)
                    {
                        normText += ".";
                    }
                    paragraph.Add(normText);
                    counter += normText.Split(" ").Count();
                }
            }
          //  Console.WriteLine("Start " + counter);
            return new Paragraph(paragraph,counter);
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

    public class JsonObject
    {
        public string ObjectName { get; set; }
        public List<string> Results { get; set; }

        public JsonObject(List<string> Results)
        {
            this.ObjectName = "AutoComplete Results";
            this.Results = Results;
        }

        public void Serialize()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            string str = JsonConvert.SerializeObject(this);

            Console.WriteLine(str);
        }

    }

    public class Options
    {
        public List<string> Values { get; set; }

        public Options()
        {
            this.Values = new List<string>();
        }

        public void AddAutoCompleteOptions(List<string> list)
        {
            foreach(var item in list)
            {
                this.Values.Add(item);
            }
        }

       
    }
}
