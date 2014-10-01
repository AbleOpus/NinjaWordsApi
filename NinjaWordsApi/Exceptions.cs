using System;

namespace NinjaWordsApi
{
    /// <summary>
    /// The exception that is thrown when a extracted category text could not 
    /// be represented as a <see cref="LexicalCategory"/>.
    /// </summary>
    [Serializable]
    public class CategoryNotEnumeratedException : Exception
    {
        /// <summary>
        /// Gets the string that was not valid.
        /// </summary>
        public string CategoryString { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryNotEnumeratedException"/> class.
        /// </summary>
        /// <param name="category">The string that could not be parsed.</param>
        public CategoryNotEnumeratedException(string category)
        {
            CategoryString = category;
        }
    }
}
