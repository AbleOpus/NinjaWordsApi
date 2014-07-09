using System;
using System.Collections.Generic;
using System.Text;

namespace NinjaWordsApi
{
    /// <summary>
    /// Represents a term that can be looked up on http://Ninjawords.com
    /// </summary>
    public class NinjaTerm
    {
        #region Properties
        /// <summary>
        /// Gets the definition entries for this term
        /// </summary>
        public NinjaEntry[] Entries { get; private set; }

        /// <summary>
        /// Gets the text of this term
        /// </summary>
        public string Term { get; private set; }

        /// <summary>
        /// Gets the available synonyms for this term
        /// </summary>
        public string[] Synonyms { get; private set; }

        /// <summary>
        /// Gets whether this term has any entries
        /// </summary>
        public bool Defined
        {
            get { return Entries != null && Entries.Length > 0; }
        }
        #endregion

        public NinjaTerm(string term, string[] synonyms, params NinjaEntry[] entries)
        {
            Entries = entries;
            Synonyms = synonyms;
            Term = term;
        }

        /// <summary>
        /// Creates an instance of NinjaTerm. This term will be undefined 
        /// </summary>
        public NinjaTerm(string term) : this(term, null, null) { }

        public override string ToString()
        {
            if (Entries == null) return string.Empty;   

            var SB = new StringBuilder();
            SB.AppendLine(Term + ":"); // Add the term text itself

            var valuesList = new List<LexicalCategory>((LexicalCategory[])
                Enum.GetValues(typeof(LexicalCategory)));

            foreach (var entry in Entries)
            {
                if (valuesList.Contains(entry.Category))
                {
                    SB.AppendLine("[" + entry.Category + "]");
                    valuesList.Remove(entry.Category);
                }

                SB.AppendLine(entry.ToString());
            }

            return SB.ToString();
        }
    }
}
