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
        /// <summary>
        /// An empty/undefined NinjaTerm
        /// </summary>
        public readonly static NinjaTerm Empty = new NinjaTerm(string.Empty, null, null);

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

            var valuesList = new List<Category>((Category[])
                Enum.GetValues(typeof(Category)));

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

            //SB.AppendLine(Text + ":");
            //SB.AppendLine();

            //foreach (var entry in Entries)
            //{
            //    SB.AppendLine(entry.ToString());
            //    SB.AppendLine();
            //}

            //if (Synonyms != null && Synonyms.Length > 0)
            //{
            //    SB.Append("Synonyms: ");

            //    foreach (var syn in Synonyms)
            //        SB.Append(syn + ", ");
            //}

            //return SB.ToString().TrimEnd(' ', ',');
        }
    }
}
