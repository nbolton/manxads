using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ManxAds.Search;

namespace ManxAds
{
    /// <summary>
    /// Consider renaming to StringTools.
    /// </summary>
    public class StringTools
    {
        const int upperCaseWeight = 12;
        const int lowerCaseWeight = 8;
        const int miscCharWeight = 5;
        const int numberWeight = 7;

        public static string MakeTextPublicSafe(string text, bool removeMatches)
        {
            Regex emailSearch = new Regex(LocalSettings.EmailAddressRegex, RegexOptions.Multiline);
            if (removeMatches)
            {
                text = emailSearch.Replace(text, String.Empty);
            }
            else
            {
                text = emailSearch.Replace(text, LocalSettings.ListingEmailReplaceString);
            }

            return text;
        }

        public static string StripHtmlTags(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }

            return Regex.Replace(text, @"<(.|\n)*?>", String.Empty);
        }

        public static string InsertHtmlBreaks(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }

            return Regex.Replace(text, @"\r\n|\r|\n", "<br />");
        }

        /// <summary>
        /// Trims the text to a specific length (or closest available).
        /// </summary>
        /// <param name="text">Text to trim and return.</param>
        /// <param name="index">Start position in string.</param>
        /// <param name="length">Length of trim to achieve.</param>
        /// <param name="toWords">Trim inward toward word endings.</param>
        /// <param name="appendString">String to append if trimmed.</param>
        /// <param name="extraTrimForAppend">Extra characters to remove if trimmed.</param>
        /// <returns>A trimmed string altered by parameters.</returns>
        public static string TrimString(
            string text,
            int index,
            int length,
            bool toWords,
            string appendString,
            int extraTrimForAppend)
        {
            if (index < 0)
            {
                index = 0;
            }

            if ((length + index) > text.Length)
            {
                length = text.Length - index;
            }

            if (text.Length <= (index + length))
            {
                // Append nothing if target is smaller that max size.
                appendString = String.Empty;
            }
            else
            {
                length -= extraTrimForAppend;
            }

            if (toWords)
            {
                return TrimToWords(text, ref index, ref length) + appendString;
            }

            return text.Substring(index, length).Trim(WordList.SplitChars) + appendString;
        }

        public static string TrimToWords(string text, ref int index, ref int length)
        {
            // Check that text actually contains whitespace.
            bool containsWhitespace = false;
            for (int i = index; i < (index + length); i++)
            {
                if (Char.IsWhiteSpace(text[i]))
                {
                    containsWhitespace = true;
                    break;
                }
            }

            if (containsWhitespace)
            {
                // Unwind until a space or end of line is reached.
                while ((index < text.Length) && (index > 0)
                    && !Char.IsWhiteSpace(text[index]))
                {
                    index++;
                    length--;
                }

                // Rewind until a space or start is reached.
                while (((index + length) < text.Length)
                    && ((length * 2) > 0)
                    && !Char.IsWhiteSpace(text[index + length]))
                {
                    length--;
                }
            }

            // Return with whitespace trimmed.
            string trimmed = text.Substring(index, length).TrimStart(WordList.SplitChars);

            if ((index + length) != text.Length)
            {
                // Trim punctuation and whitespace if not end.
                return trimmed.TrimEnd(WordList.SplitChars);
            }

            return trimmed;
        }

        /// <summary>
        /// Trims a string to its estimated pixel length.
        /// </summary>
        /// <param name="text">Text to trim to pixels.</param>
        /// <param name="index">Character index to trim from.</param>
        /// <param name="pixels">Number of pixels to trim to.</param>
        /// <param name="sizeEm">Font size of characters.</param>
        /// <returns>String trimmed to pixel length.</returns>
        public static string TrimToPixels(
            string text, int index, float pixels, float sizeEm, string appendString)
        {
            float appendWeight = GetStringWeight(appendString, sizeEm);
            string result = String.Empty;
            float accumulatedLength = 0f;
            float maximumPixels = pixels - appendWeight;

            for (int i = index; i < text.Length; i++)
            {
                // Calculate the length after the append operation.
                accumulatedLength += GetCharWeight(text[i], sizeEm);

                if (accumulatedLength <= maximumPixels)
                {
                    // Only apeend if pixel quota not exceeded.
                    result += text[i];
                }
                else
                {
                    // Go ahead with append.
                    return result.Trim() + appendString;
                }
            }

            // Unaltered result.
            return result;
        }

        public static float GetStringWeight(string text, float sizeEm)
        {
            float accumulatedWeight = 0f;
            foreach (char testChar in text)
            {
                accumulatedWeight += GetCharWeight(testChar, sizeEm);
            }
            return accumulatedWeight;
        }

        public static float GetCharWeight(char test, float sizeEm)
        {
            if (Char.IsUpper(test))
            {
                return upperCaseWeight * sizeEm;
            }

            if (Char.IsLower(test))
            {
                return lowerCaseWeight * sizeEm;
            }

            if (Char.IsNumber(test))
            {
                return numberWeight * sizeEm;
            }

            return miscCharWeight * sizeEm;
        }
    }
}
