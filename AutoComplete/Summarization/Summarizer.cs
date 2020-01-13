using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using AutoComplete.FileLogger;

namespace AutoComplete.Summarization
{
    class Summarizer
    {
        public int Counter { get; set; }
        public List<string> SummarizedText { get; set; }
        public Summarizer()
        {
            Counter = 0;
            SummarizedText = new List<string>();
        }

        public void Run(string input)
        {
            List<string> tokenizeText = Tokenizer.Split(input);
            Matrix matrix = new Matrix(tokenizeText.Count);
            Sentence sentence = new Sentence();

            for (int i = 0; i < tokenizeText.Count; i++)
            {
                int counter = 0;
                for (int j = 0; j < tokenizeText.Count; j++)
                {
                    if(i != j)
                    {
                        double score = Intersection(tokenizeText[i], tokenizeText[j]);
                        if(!matrix.Score[i,0].HasValue)
                        {
                            matrix.Score[i, 0] = score;
                         //   Console.WriteLine(i + " " + 0 + " : " + tokenizeText[i] + " ::: " + tokenizeText[j] + " = " + score);
                        }
                        else
                        {
                            matrix.Score[i, counter] = score;
                         //   Console.WriteLine(i + " " + (counter) + " : " + tokenizeText[i] + " ::: " + tokenizeText[j] + " = " + score);
                        }
                        counter++;
                    }
                }
                sentence.ScoreSentence(tokenizeText[i], matrix.Score, i, tokenizeText.Count);
            }

            double chosenIndex = Convert.ToDouble(sentence.IndScore.Values.Max());
            string chosenSentence = sentence.IndScore.FirstOrDefault(x => Convert.ToDouble(x.Value) == chosenIndex).Key;
            Console.WriteLine("Start " + chosenSentence + " end ");
            SummarizedText.Add(chosenSentence);
         //   Logger.Log(chosenSentence, "Sum");
            Counter += chosenSentence.Split(" ").Count();
        }

        double Intersection(string s1, string s2)
        {
            List<string> s1Tokenized = Tokenizer.Tokenize(s1);
            List<string> s2Tokenized = Tokenizer.Tokenize(s2);
            double commonTokens = 0;

            for(int i = 0; i < s1Tokenized.Count; i++)
            {
                for (int j = 0; j < s2Tokenized.Count; j++)
                {
                    if (s1Tokenized[i].Equals(s2Tokenized[j]))
                    {
                        commonTokens++;
                    }
                }
            }
            return commonTokens / ((s1.Length + s2.Length) / 2);
        }
    }

    public class Sentence
    {
        public Dictionary<string,double?> IndScore { get; set; }

        public Sentence()
        {
            IndScore = new Dictionary<string, double?>();
        }

        public void ScoreSentence(string sentence, double?[,] matrix, int index, int length)
        {
            double score = 0;
            for (int i = 0; i < length; i++)
            {
                score += Convert.ToDouble(matrix[index, i]);
            }
            IndScore.Add(sentence, score);
        }
    }

    public class Matrix
    {
        public double?[,] Score { get; set; }

        public Matrix(int size)
        {
            Score = new double?[size,size];
        }
    }

    public class Tokenizer
    {

        public static List<string> Split(string para)
        {
            return new List<string>(Regex.Split(para, @"(?<=[\.!\?])\s+"));
        }

        public static List<string> Tokenize(string sentence)
        {
            return new List<string>(sentence.Split(" "));
        }
    }
}
