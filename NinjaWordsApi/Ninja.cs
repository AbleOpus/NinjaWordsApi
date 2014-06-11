﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NinjaWordsApi
{
    /// <summary>
    /// Specifies the category in which a ninja term falls under
    /// </summary>
    public enum Category
    {
        /// <summary>
        /// An unidentified catergory was encountered
        /// </summary>
        Unknown,
        Noun,
        Adjective,
        Verb,
        Pronoun,
        Abbreviation,
        Interjection,
        Preposition
    }

    /// <summary>
    /// Provides ninja like term lookups
    /// </summary>
    public static class Ninja
    {
        /// <summary>
        /// The host name for the website
        /// </summary>
        public const string Host = "http://ninjawords.com";

        /// <summary>
        /// Gets a link to a random definition on a minimal webpage
        /// </summary>
        public static string GetRandomLink()
        {
            return Host + "/definitions/random";
        }

        /// <summary>
        /// Creates a NinjaWords lookup link from the specified words
        /// </summary>
        /// <param name="words">The words to appenbd to the link</param>
        /// <param name="minimal">Whether to return minimal HTML content</param>
        public static string CreateLookupLink(string[] words, bool minimal)
        {
            var SB = new StringBuilder();
            SB.Append(Host + "/");
            if (minimal) SB.Append("definitions/get/");

            foreach (var word in words)
                SB.Append(word + ",");

            SB.TrimEnd(',');
            return SB.ToString();
        }

        /// <summary>
        /// Gets a random term from http://Ninjawords.com asyncronously
        /// </summary>
        /// <exception cref="WebException"></exception>
        public async static Task<NinjaTerm> GetRandomTermAsync()
        {
            string content = await DownloadNinjaPage(GetRandomLink());
            return ExtractNinjaWord(RemoveUnwantedData(content));
        }

        /// <summary>
        /// Gets an array of NinjaTerms from comma seperated terms asyncronously
        /// <example>Example: This,is,code</example>
        /// </summary>
        /// <exception cref="WebException"></exception>
        /// <returns>A Task that yeilds NinjaTerms</returns>
        /// <param name="terms">An array of terms</param>v
        public static Task<NinjaTerm[]> GetTermsAsync(string terms)
        {
            string[] input = terms.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return GetTermsAsync(input);
        }

        /// <summary>
        /// Gets an array of NinjaTerms from an array of terms asyncronously
        /// </summary>
        /// <param name="terms">An array of terms</param>
        /// <exception cref="WebException"></exception>
        /// <returns>A Task that yeilds NinjaTerms</returns>
        public async static Task<NinjaTerm[]> GetTermsAsync(string[] terms)
        {
            return await GetTermsAsyncBase(terms);
        }

        private static async Task<NinjaTerm[]> GetTermsAsyncBase(string[] terms)
        {
            // Process word array
            terms = terms.Distinct().ToArray();
            terms.TrimAll();
            var URL = CreateLookupLink(terms, true);
            string content = await DownloadNinjaPage(URL);
            content = RemoveUnwantedData(content);
            const string PATTERN = @"<dl id=""word-(?<Word>[^""]+)"">.+?</dl>";
            var MC = Regex.Matches(content, PATTERN, RegexOptions.Singleline);
            var ninjaTerms = GetUndefinedTerms(terms);

            foreach (Match match in MC)
            {
                // Lookup failed for this word, skip over it
                if (match.Value.Contains(@"class=""did-you-mean""")) continue;
                string word = match.Groups["Word"].Value;

                // Find the appropriate blank term to overwrite
                for (int i = 0; i < ninjaTerms.Length; i++)
                {
                    if (ninjaTerms[i].Term == word)
                    {
                        ninjaTerms[i] = ExtractNinjaWord(match.Value);
                        break;
                    }
                }
            }

            return ninjaTerms;
        }

        /// <summary>
        /// Downloads a NinjaWords page using UTF8 encoding.
        /// </summary>
        private static Task<string> DownloadNinjaPage(string address)
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                return client.DownloadStringTaskAsync(address);
            }
        }

        /// <summary>
        /// Remove spans and links to avoid unnessearily complex regex patterns.
        /// Also completely remove the spans that define the definition marker
        /// </summary>
        private static string RemoveUnwantedData(string element)
        {
            var SB = new StringBuilder(element);
            // Unescape quotations and remove newlines
            SB.Replace(@"\n", string.Empty); // For some reason, literal newlines are in HTML
            SB.Replace(@"\""", @"""");
            // Remove link tags
            var MC = Regex.Matches(SB.ToString(), @"(<a\s+[^>]+>)|(</a>)");
            SB.Remove(MC);
            // Remove spans
            const string PATTERN = @"(<span\s+[^>]+>&deg;</span>)|(<span\s+[^>]+>)|(</span>)";
            MC = Regex.Matches(SB.ToString(), PATTERN, RegexOptions.Singleline);
            SB.Remove(MC);
            return SB.ToString();
        }

        /// <summary>
        /// Creates an array of undefined NinjaTerms from the specified word list
        /// </summary>
        private static NinjaTerm[] GetUndefinedTerms(string[] terms)
        {
            var ninjaTerms = new NinjaTerm[terms.Length];

            for (int i = 0; i < ninjaTerms.Length; i++)
                ninjaTerms[i] = new NinjaTerm(terms[i]);

            return ninjaTerms;
        }

        /// <summary>
        /// Gets the category closest and above the specified position
        /// </summary>
        /// <param name="element">An element containing both the entry and category</param>
        /// <param name="index">The index to look above</param>
        private static Category GetAboveCategory(string element, int index)
        {
            const string ART_PATTERN = @"<dd\s+class=""article"">(?<Category>[^<]+)";
            var MC = Regex.Matches(element, ART_PATTERN, RegexOptions.Singleline);
            Match closest = Match.Empty;

            foreach (Match match in MC)
            {
                // If match above and current category is closer than closest
                if (match.Index < index && (index - closest.Index > index - match.Index))
                {
                    closest = match;
                }
            }

            Category category;
            Enum.TryParse(closest.Groups["Category"].Value, true, out category);
            return category;
        }

        /// <summary>
        /// Parses an element to a NinjaTerm
        /// </summary>
        /// <param name="element">The element that contains one term</param>
        /// <returns>An undefined term if nothing found</returns>
        private static NinjaTerm ExtractNinjaWord(string element)
        {
            const RegexOptions OPTIONS = RegexOptions.Singleline;
            const string WORD_PATTERN = @"id=""word-(?<Word>[^""]+)""";
            var word = Regex.Match(element, WORD_PATTERN, OPTIONS).Groups["Word"].Value;
            var MC = Regex.Matches(element, @"<dd class=""entry"">.+?</dd>", OPTIONS);
            var entries = new NinjaEntry[MC.Count];

            // Create entries from entry elements found
            for (int i = 0; i < entries.Length; i++)
            {
                const string DEF_PATTERN = @"<div class=""definition"">\s*(?<Def>.+?)\s*</div>";
                var definition = Regex.Match(MC[i].Value, DEF_PATTERN, OPTIONS).Groups["Def"].Value;
                var category = GetAboveCategory(element, MC[i].Index);
                const string EXAMP_PAT = @"<div class=""example"">\s*""(?<Inner>.+?)""\s*</div>";
                string example = Regex.Match(MC[i].Value, EXAMP_PAT, OPTIONS).Groups["Inner"].Value;
                entries[i] = new NinjaEntry(category, definition, example);
            }

            // Get synonyms
            const string PATTERN = @"synonyms:(?<Inner>.+?)</dd>";
            string synEl = Regex.Match(element, PATTERN, OPTIONS).Groups["Inner"].Value;
            string[] syns = synEl.Split(new[] { ',', ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            // Remove more link
            syns = syns.Where(s => !s.Contains("&raquo;")).ToArray();
            return new NinjaTerm(word, syns, entries.ToArray());
        }
    }
}
