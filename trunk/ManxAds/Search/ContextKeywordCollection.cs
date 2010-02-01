using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ManxAds.Search
{
    public class ContextKeywordCollection
    {
        private const string boldTags = "<b>{0}</b>";

        private float totalWeight;
        private List<ContextKeyword> keywordList;
        private bool isEnabled = false;

        /// <summary>
        /// Gets true if keywords have been added.
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
        }

        public float TotalWeight
        {
            get { return totalWeight; }
        }

        public static ContextKeywordCollection Empty
        {
            get
            {
                ContextKeywordCollection ckc =
                    new ContextKeywordCollection();
                ckc.isEnabled = false;
                return ckc;
            }
        }

        public ContextKeywordCollection()
        {
            this.totalWeight = 0;
            this.keywordList = new List<ContextKeyword>();
        }

        public void Add(ContextKeyword keyword)
        {
            keywordList.Add(keyword);
            totalWeight += keyword.Weight;
            isEnabled = true;
        }

        public List<ContextKeyword> getTop(int limit, string checkContext)
        {
            if (!isEnabled)
            {
                throw new InvalidOperationException(
                    "IsEnabled must be true to use this method.");
            }

            Comparison<ContextKeyword> sort =
                new Comparison<ContextKeyword>(compareWeights);
            this.keywordList.Sort(sort);

            int scanIndex = 0;
            List<ContextKeyword> topList = new List<ContextKeyword>();
            while ((topList.Count < limit) && (scanIndex < keywordList.Count) )
            {
                // Check keyword is in this context, and not just it's twin.
                Regex regex = getWordRegex(keywordList[scanIndex].Name);
                if (regex.Match(checkContext).Success)
                {
                    // Add and increment counter.
                    topList.Add(keywordList[scanIndex]);
                }
                scanIndex++;
            }
            return topList;
        }

        private static int compareWeights(ContextKeyword x, ContextKeyword y)
        {
            if (x.Weight < y.Weight)
            {
                return +1;
            }
            else if (x.Weight > y.Weight)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        private string getWordPattern(string word)
        {
            string matchWord = word.Replace("+", @"\+");
            return @"\b" + matchWord + @"(s|es)*\b";
        } 

        private Regex getWordRegex(string word)
        {
            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase;
            string regexPattern = getWordPattern(word);
            return new Regex(regexPattern, options);
        }

        /// <summary>
        /// Using a trial-and-error approach, this method recursively attempts to highlight the
        /// maximum number of segments given an overall length limit and number of available keywords.
        /// </summary>
        /// <param name="context">Entire context to highlight in.</param>
        /// <param name="overallLimit">Overall limit to entire context.</param>
        /// <param name="segmentCount">Target number of segments to produce.</param>
        /// <returns>Segmented and glued text.</returns>
        private string getDynamicHighlight(string context, int overallLimit, int requestSegmentCount)
        {
            if (requestSegmentCount < 1)
            {
                throw new InvalidOperationException(
                    "Cannot split context into less than one segment.");
            }

            // Target segment cound cannot exceed actual number of keywords.
            bool countTooBig = requestSegmentCount > keywordList.Count;
            int targetSegmentCount = countTooBig ? keywordList.Count : requestSegmentCount;
            List<ContextKeyword> topKeywords = this.getTop(targetSegmentCount, context);
            
            if (topKeywords.Count < 1)
            {
                // No keywords found, return default text (trimmed).
                return StringTools.TrimString(context, 0, overallLimit, true, "...", 2);
            }

            string newContext = String.Empty;
            MarginText lastMargin = MarginText.Null;
            int segmentLimit = overallLimit / targetSegmentCount;
            bool overlapOccurred = false;
            bool emptyMarginOccurred = false;
            int segmentsGenerated = 0;

            // Split the space into a number of segments.
            foreach (ContextKeyword keyword in topKeywords)
            {
                int timesToLoop = 1;
                if (topKeywords.Count == 1)
                {
                    int occurances = getWordRegex(keyword.Name).Matches(context).Count;

                    // Loop for every occurrence if only 1 keyword, but only to the maximum segment count.
                    timesToLoop = (occurances > requestSegmentCount ? requestSegmentCount : occurances);

                    // Change target to how many times to loop.
                    targetSegmentCount = timesToLoop;

                    // Update segment limit based on new target.
                    segmentLimit = overallLimit / targetSegmentCount;
                }

                for (int i = 0; i < timesToLoop; i++)
                {
                    Regex regex = getWordRegex(keyword.Name);
                    int nameLength = keyword.Name.Length;

                    // Replace last margin using last margin as a overlap avoidance machanism.
                    lastMargin = new MarginText(context, regex, nameLength, segmentLimit, lastMargin);

                    if (lastMargin.IsNull)
                    {
                        emptyMarginOccurred = true;
                        if (lastMargin.HasOverlapped)
                        {
                            overlapOccurred = true;
                        }
                        continue;
                    }

                    // Append the marginalised text.
                    newContext += lastMargin.Marginalised;
                    segmentsGenerated++;
                }
            }

            bool notEnoughSegments = segmentsGenerated < targetSegmentCount;
            if (emptyMarginOccurred | overlapOccurred | notEnoughSegments)
            {
                // Failsafe to stop stack overflow.
                if ((requestSegmentCount == 1) && (segmentsGenerated == 0))
                {
                    throw new InvalidOperationException(
                        "Cannot recurse when requested target " +
                        "is 1 and segments generated is 0. " +
                        "Context string was '" + context + "'.");
                }

                int newTargetSegmentCount = targetSegmentCount - 1;

                if (notEnoughSegments)
                {
                    // Generate new segments, but utilise more space.
                    newTargetSegmentCount = segmentsGenerated;
                }

                if (newTargetSegmentCount == 0)
                {
                    // Cannot have less than 1 segment.
                    newTargetSegmentCount = 1;
                }

                // Recurse with 1 less target segment and return.
                return getDynamicHighlight(context, overallLimit, newTargetSegmentCount);
            }

            return newContext;
        }

        /// <summary>
        /// Only highlights the current keywords in the entire context without
        /// limiting or segmenting the resulting context.
        /// </summary>
        /// <param name="context">Context in which to highlight keywords.</param>
        /// <returns>Origional context with highlighted keywords.</returns>
        public string HighlightContext(string context)
        {
            if (!isEnabled)
            {
                throw new InvalidOperationException(
                    "IsEnabled must be true to use this method.");
            }

            MatchEvaluator evaluator = new MatchEvaluator(ReplaceKeyword);
            foreach (ContextKeyword keyword in keywordList)
            {
                Regex regex = getWordRegex(keyword.Name);
                MatchCollection matches = regex.Matches(context);
                context = regex.Replace(context, evaluator);
            }

            return context;
        }

        /// <summary>
        /// Splits a context into segments focusing on the heaviest keywords, then
        /// highlights all remaining recognised whole keywords.
        /// </summary>
        /// <param name="context">Text to segment and highlight.</param>
        /// <param name="resultLimit">Maximum lenght of returned context.</param>
        /// <param name="segmentCount">Number of segments to try and split into.</param>
        /// <returns>Segmented, limited and highlighted context.</returns>
        public string HighlightContext(string context, int overallLimit, int segmentCount)
        {
            if (!isEnabled)
            {
                throw new InvalidOperationException(
                    "IsEnabled must be true to use this method.");
            }

            // First split the context into segments with focused keywords.
            context = getDynamicHighlight(context, overallLimit, segmentCount);

            // Then highlight the remaining known keywords.
            return this.HighlightContext(context);
        }

        /// <summary>
        /// Represents a section of text which has been marginalised, or "padded" at either
        /// end. If neccecary, then a 'pause' (...) annotation is prefixed and/or appended.
        /// </summary>
        protected struct MarginText
        {
            private const string pauseText = " <b>...</b> ";

            public int Start;
            public int End;
            public string Marginalised;
            public bool IsNull;
            public bool HasOverlapped;
            public Match TargetMatch;

            internal static MarginText Null
            {
                get
                {
                    MarginText mt = new MarginText();
                    mt.IsNull = true;
                    return mt;
                }
            }

            internal MarginText(string text, Regex regex, int focusLength, int limit)
                : this(text, regex, focusLength, limit, MarginText.Null) { }

            internal MarginText(string text, Regex regex, int focusLength, int limit, MarginText last)
            {
                // Split total length into two for margin on either side.
                int margin = (limit - focusLength) / 2;
                MatchCollection matches = regex.Matches(text);

                // User may have removed an indexed keyword.
                if (matches.Count == 0)
                {
                    this = MarginText.Null;
                    return;
                }

                TargetMatch = matches[0];
                int matchIndex = 0;
                int lastIndex = matches.Count;

                // Target cannot lie within the last used margin.
                while (TargetMatch.Index <= last.End)
                {
                    if (matchIndex >= lastIndex)
                    {
                        // Failsafe trigger if cannot highlight further.
                        this = MarginText.Null;
                        this.HasOverlapped = true;
                        return;
                    }
                    TargetMatch = matches[matchIndex++];
                    if (last.IsNull)
                    {
                        // Index could start at 0, where null also starts at 0.
                        break;
                    }
                }

                // Target values for marginalisation.
                int index = TargetMatch.Index - margin;
                int length = TargetMatch.Length + (margin * 2);

                if (index < 0)
                {
                    // Cannot preceed 0 index.
                    index = 0;
                }

                if ((index + length) >= text.Length)
                {
                    // Calculate distance between index and end.
                    int spaceLeft = text.Length - index;

                    // Rewind index as far as possible (without loosing match).
                    index -= length - spaceLeft;

                    // Incase index has gone before 0, trim from end.
                    if (index < 0)
                    {
                        // Subtract inverse (positive).
                        length += index;

                        // Reset to start.
                        index = 0;
                    }
                }

                this.IsNull = false;
                this.HasOverlapped = false;
                this.Start = index;
                this.End = index + length;
                this.Marginalised = String.Empty;

                string startPause = null, endPause = null;

                if ((this.Start != 0) && (last.IsNull))
                {
                    // Add pause if not at start.
                    startPause = pauseText;
                }

                if (this.End != text.Length)
                {
                    // Add pause if not at end.
                    endPause = pauseText;
                }

                string trimmed = StringTools.TrimToWords(text, ref index, ref length);
                this.Marginalised = startPause + trimmed + endPause;

                // Reassign because index and length may have changed.
                this.Start = index;
                this.End = index + length;
            }
        }

        protected static string ReplaceKeyword(Match input)
        {
            return String.Format(boldTags, input.Value);
        }
    }
}