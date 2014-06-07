using System;

namespace NinjaWordsApiDemo
{
    /// <summary>
    /// Represents an entry for one word. A word may consist of
    /// one or more entries (words can have more than one definition and so forth)
    /// </summary>
    public class NinjaEntry
    {
        /// <summary>
        /// Gets the category the word falls under
        /// </summary>
        public Category Article { get; private set; }

        /// <summary>
        /// Gets the text that defines the word
        /// </summary>
        public string Definition { get; private set; }

        /// <summary>
        /// Gets an example for the word
        /// </summary>
        public string Example { get; private set; }

        public NinjaEntry(Category article, string definition, string example)
        {
            Article = article;
            Definition = definition;
            Example = example;
        }

        public override string ToString()
        {
            // I Ex. (noun) To refer to ones self.
            //       "I often write code when no one is around"
            string text = "(" + Article + ") " + Definition;

            if (!String.IsNullOrEmpty(Example))
                text += Environment.NewLine + @"""" + Example + @"""";

            return text;
        }
    }
}
