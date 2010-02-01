using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Data.SqlTypes;
using System.IO;

namespace ManxAds.Search
{
    public class WordList
    {
        public const float WordWeightMax = 10f;

        private List<ContextKeyword> list;
        
        /// <summary>
        /// Whole words to exclude from catalogue index. These can be added as part of words.
        /// </summary>
        private static string[] ignoreWords = new string[] {
            "and", "or", "-", "/", @"\"
        };

        /// <summary>
        /// Split strings by these characters. None of these will be added to the database.
        /// </summary>
        public static char[] SplitChars = new char[] {
            ' ', ',', ':', '?', '!', '*', '(', ')', '\n', '\r', '\'', '"', '’', '<', '>'
        };

        /// <summary>
        /// Characters which join words together, like foo-bar.
        /// </summary>
        public static char[] WordJoinChars = new char[] {
            '-', '+', '.', '/', '\\', '_', '@', '&'
        };

        public List<ContextKeyword> InnerList
        {
            get { return list; }
        }

        public WordList()
        {
            list = new List<ContextKeyword>();
        }

        public WordList(string rawText, bool allowRepeatWords)
            : this(rawText, allowRepeatWords, 0) { }

        public WordList(string rawText, bool allowRepeatWords, int imposeWordLimit)
            : this(rawText, allowRepeatWords, imposeWordLimit, false, false, false) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawText"></param>
        /// <param name="allowRepeatWords"></param>
        /// <param name="imposeWordLimit"></param>
        /// <param name="useOperators"></param>
        /// <param name="anyWords"></param>
        /// <param name="useWordSegments">
        /// Adds each segment of a segmented word as well as the segmented word it's self.
        /// </param>
        public WordList(
            string context, bool allowRepeatWords,
            int imposeWordLimit, bool useOperators,
            bool anyWords, bool useWordSegments)
            : this()
        {
            bool operatorPrevious = false;
            List<string> cleanList = new List<string>(), finalList = new List<string>();
            string[] rawList = context.Split(SplitChars, StringSplitOptions.RemoveEmptyEntries);
            List<char> wordJoinCharList = new List<char>(WordJoinChars);

            // Split each segment word, recording individual segments and whole words.
            for (int i = 0; i < rawList.Length; i++)
            {
                string word = rawList[i].ToLower();
                string[] segments = word.Split(WordJoinChars, StringSplitOptions.RemoveEmptyEntries);

                if (segments.Length > 1)
                {
                    foreach (string innerWord in segments)
                    {
                        // Add individual segments.
                        cleanList.Add(innerWord);
                    }
                }

                // Check for invalid chars.
                foreach (char join in wordJoinCharList)
                {
                    if (word[0] == join)
                    {
                        // Remove first if invalid char.
                        word = word.Remove(0, 1);
                    }

                    if ((word.Length > 1) && (word[word.Length - 1] == join))
                    {
                        // Remove last if invalid char.
                        word = word.Remove(word.Length - 1, 1);
                    }

                    if (String.IsNullOrEmpty(word))
                    {
                        // Give up on empty words.
                        break;
                    }
                }

                // May have been deleted in treatment.
                if (!String.IsNullOrEmpty(word))
                {
                    cleanList.Add(word);
                }
            }

            // Add operators and keywords to final list.
            for (int i = 0; i < cleanList.Count; i++)
            {
                bool singularAdded = false;
                string word = cleanList[i];

                // Check to see if next word is an operator.
                if (useOperators & !operatorPrevious & ((i + 1) < rawList.Length))
                {
                    if (rawList[i + 1] == "or")
                    {
                        // Prepend "or" to make next operator valid.
                        finalList.Add("or");
                        operatorPrevious = true;
                    }
                }

                WordStemResult stemResult = WordStem.GetStem(word);
                if (stemResult.Stemmed)
                {
                    // Add all stems separated by or if neccecary.
                    foreach (string stem in stemResult.StemVariations)
                    {
                        // Can be overriden by and.
                        if (useOperators & !operatorPrevious)
                        {
                            finalList.Add("or");
                        }

                        finalList.Add(stem);
                    }

                    operatorPrevious = false;
                    singularAdded = true;
                }

                // Singular needs origional to be or.
                if (useOperators & !operatorPrevious & (singularAdded | anyWords))
                {
                    finalList.Add("or");
                }

                // Always add origional after, incase stem is wrong.
                finalList.Add(word);

                // Remember if last word was operator.
                if ((word == "and") | (word == "or"))
                {
                    operatorPrevious = true;
                }
                else
                {
                    operatorPrevious = false;
                }
            }

            int wordsAdded = 0;
            List<string> ignoreList = new List<string>(ignoreWords);

            foreach (string word in finalList)
            {
                // Check if the word is an operator.
                bool isOperator = (word == "and") | (word == "or");
                
                ContextKeyword contextKeyword = new ContextKeyword(word, 0);
                bool isRepeated = list.Contains(contextKeyword);

                // Ignore blocked and repeated words (if specified in parameter).
                if (isOperator || (allowRepeatWords || !isRepeated) && !ignoreList.Contains(word))
                {
                    if (!isOperator && ((imposeWordLimit > 0) && (wordsAdded > imposeWordLimit)))
                    {
                        // Break if word limit reached.
                        break;
                    }

                    // Calculate word weight as it's percentage of location.
                    int wordsRemaining = finalList.Count - wordsAdded;
                    contextKeyword.Weight = ((float)wordsRemaining / (float)finalList.Count) * 10;

                    list.Add(contextKeyword);
                    wordsAdded++;
                }
            }
        }

        public string HighlightContext(string context, int limit)
        {
            Regex search;
            Match match;
            string replace;
            string format = LocalSettings.WordHighlightFormat;
            int index, start, length;
            int highlightMargin = limit / 4;
            List<string> segments = new List<string>();

            // For each word, highlight in context.
            foreach (ContextKeyword contextKeyword in list)
            {
                string word = contextKeyword.Name;
                search = new Regex(word, RegexOptions.IgnoreCase);
                match = search.Match(context);

                // Only continue if something to replace.
                if (!match.Success) continue;

                replace = String.Format(format, match.Value);
                context = search.Replace(context, replace);

                // Continue if result should not be limited.
                if (limit == -1) continue;

                // Repeat until index not found.
                index = 0;
                while (index != -1)
                {
                    index = context.IndexOf(match.Value, index);
                    if (index == -1) continue;
                    
                    start = index - highlightMargin;
                    length = match.Value.Length +
                        (highlightMargin * 2);


                    if (length < context.Length)
                    {
                        if ((start + length) > context.Length)
                        {
                            // Shift over to the left.
                            start = context.Length - length;
                        }

                        // Shift over to the right.
                        if (start < 0) start = 0;
                    }
                    else
                    {
                        start = 0;
                        length = context.Length;
                    }

                    segments.Add(context.Substring(start, length));

                    // Push over to next.
                    index = start + length;

                    if (list.Count > 1) break;
                }
            }

            if (limit != -1)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string segment in segments)
                {
                    builder.Append(
                        segment.Trim(LocalSettings.ShortenTrimChars) + "... ");

                    if (builder.Length > limit) break;
                }

                if (segments.Count == 0)
                {
                    return String.Empty;
                }

                return "..." + builder.ToString();
            }

            return context;
        }

        public void AddWord(string word)
        {
            list.Add(new ContextKeyword(word, 0));
        }

        public override string ToString()
        {
            throw new NotSupportedException();
        }

        public SqlXml ToXml()
        {
            XmlDocument wordList = new XmlDocument();
            XmlNode root = wordList.CreateElement("Root");
            wordList.AppendChild(root);

            foreach (ContextKeyword contextKeyword in this.list)
            {
                string word = contextKeyword.Name;
                XmlNode wordNode = wordList.CreateElement("Word");
                XmlText wordText = wordList.CreateTextNode(word);
                wordNode.AppendChild(wordText);
                root.AppendChild(wordNode);
            }

            StringReader reader = new StringReader(wordList.InnerXml);
            return new SqlXml(new XmlTextReader(reader));
        }
    }
}
