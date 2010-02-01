using System;
using System.Collections.Generic;
using System.Text;

namespace ManxAds.Search
{
    public class WordStem
    {
        public static WordStemResult GetStem(string word)
        {
            // Default result is that the word cannot be stemmed.
            WordStemResult result = new WordStemResult(word);

            // Look for suffixes in a strict order.
            foreach (WordStemRule rule in stemRules)
            {
                if (word.EndsWith(rule.Suffix, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Word is already a stem.
                    if (rule.NonPlural) return result;

                    // Apply suffix stripping word stemming.
                    string stemPure = rule.GetPureStem(word);

                    foreach (string replaceWithString in rule.ReplaceWith)
                    {
                        string stemFinal = stemPure + replaceWithString;

                        // Stem pure can be 1 char, but stem final must be more than 1.
                        if (!string.IsNullOrEmpty(stemPure) && (stemFinal.Length > 1))
                        {
                            result.Stemmed = true;
                            result.AddStem(stemFinal);
                            result.RuleUsed = rule;
                        }
                    }

                    break;
                }
            }

            // May not have been stemmed.
            return result;
        }

        /// <summary>
        /// Hard coded suffix-strip stemming rules (could be scripted).
        /// </summary>
        private static WordStemRule[] stemRules = new WordStemRule[] {
            
            new WordStemRule("ious", "e"), // spacious -> space
            new WordStemRule("ies", "y"), // monies -> money
            new WordStemRule("es", null, "e"), // bikes -> bike | scratches -> scratch
            new WordStemRule("s"), // computers -> computer

            new WordStemRule("bbing", "b"), // bobbing -> bob
            new WordStemRule("dding", "d", "dd"), // budding -> bud | adding -> add
            new WordStemRule("ffing", "f"), // faffing -> faf
            new WordStemRule("gging", "g"), // gagging -> gag
            new WordStemRule("tting", "t"), // matting -> mat
            new WordStemRule("lling", "l", "ll"), // travelling -> travel | filling -> fill
            new WordStemRule("nning", "n"), // scanning -> scan
            new WordStemRule("pping", "p"), // skipping -> skip
            new WordStemRule("tting", "t"), // sitting -> sit
            new WordStemRule("vving", "v"), // revving -> rev
            new WordStemRule("thing", true), // something != someth
            new WordStemRule("ing", null, "e"), // fishing -> fish | taking -> take

            new WordStemRule("bbed", "b"), // dabbed -> dab
            new WordStemRule("dded", "d", "dd"), // added -> add || padded -> pad
            new WordStemRule("ffed", "f"), // faffed -> faf
            new WordStemRule("gged", "g"), // bagged -> bag
            new WordStemRule("tted", "t"), // fitted -> fit
            new WordStemRule("nned", "n"), // sequinned -> sequin
            new WordStemRule("pped", "p"), // tipped -> tip
            new WordStemRule("lled", "l", "ll"), // fuelled -> fuel | killed -> kill
            new WordStemRule("vved", "v"), // revved -> rev
            new WordStemRule("ied", "y"), // storied -> story
            new WordStemRule("eed", true), // proceed != proce
            new WordStemRule("ed", null, "e"), // treated -> treat | created -> create

            new WordStemRule("fully", "ful"), // hopefully -> hopeful
            new WordStemRule("ghtly", "ght"), // brightly -> bright
            new WordStemRule("ally", "al"), // originally -> original
            new WordStemRule("ntly", "nt"), // presently -> present
            new WordStemRule("lly", "ll"), // fully -> full
            new WordStemRule("tly", "tle"), // gently -> gentle
            new WordStemRule("bly", "ble"), // incredibly -> incredible
            new WordStemRule("ply", "ple"), // simply -> simple
            new WordStemRule("oly", "ol"), // wooly -> wool
            new WordStemRule("fly", true), // butterfly != butterf
            new WordStemRule("nly", true), // only != on
            new WordStemRule("ily", true), // family != fami
            new WordStemRule("ly"), // lovely -> love

            new WordStemRule("fication", "fy"), // specification -> specify
            new WordStemRule("llation", "ll"), // installation -> install
            new WordStemRule("duction", "duce"), // introduction -> introduce
            new WordStemRule("tation", "t"), // presentation -> present
            new WordStemRule("ation", "ate"), // inflation -> inflate
            new WordStemRule("ction", "ct"), // inspection -> inspect
            new WordStemRule("ppy", "p"), // floppy -> flop
            new WordStemRule("py", "p"), // sleepy -> sleep

            new WordStemRule("ppable", "p"), // swappable -> swap
            new WordStemRule("vable", "ve"), // removable -> remove
            new WordStemRule("sable", "se"), // reusable -> reuse
            new WordStemRule("uable", "ue"), // valuable -> value
            new WordStemRule("rable", "re"), // adorable -> adore
            new WordStemRule("able") // installable -> install
        };
    }

    public struct WordStemResult
    {
        private List<string> stemVariations;

        public string[] StemVariations
        {
            get { return stemVariations.ToArray(); }
        }

        public bool Stemmed;
        public WordStemRule RuleUsed;
        public string OriginalWord;

        public WordStemResult(string originalWord)
        {
            this.Stemmed = false;
            this.OriginalWord = originalWord;
            this.RuleUsed = WordStemRule.Empty;
            this.stemVariations = new List<string>();
        }

        public void AddStem(string stem)
        {
            stemVariations.Add(stem);
        }
    }

    public struct WordStemRule
    {
        public string Suffix;
        public bool NonPlural;
        public bool IsEmpty;
        public string[] ReplaceWith;

        public static WordStemRule Empty
        {
            get
            {
                WordStemRule rule = new WordStemRule(String.Empty);
                rule.IsEmpty = true;
                return rule;
            }
        }

        public string GetPureStem(string word)
        {
            return word.Remove(word.Length - Suffix.Length, Suffix.Length);
        }

        public WordStemRule(string suffix)
        {
            this.Suffix = suffix;
            this.NonPlural = false;
            this.IsEmpty = false;
            this.ReplaceWith = new string[] { string.Empty };
        }

        public WordStemRule(string suffix, string replaceWith)
            : this(suffix)
        {
            this.ReplaceWith = new string[] { replaceWith };
        }

        public WordStemRule(string suffix, params string[] replaceWith)
            : this(suffix)
        {
            this.ReplaceWith = replaceWith;
        }

        public WordStemRule(string suffix, bool nonPlural)
            : this(suffix)
        {
            this.NonPlural = true;
        }
    }
}
