using System;

namespace NinjaWordsApi
{
    /// <summary>
    /// The exception that is thrown when a extracted category text could not 
    /// be represented as an Enum.
    /// </summary>
    [Serializable]
    public class CategoryNotEnumeratedException : Exception
    {
        /// <summary>
        /// Gets the string that was not valid.
        /// </summary>
        public string CategoryString { get; private set; }

        public CategoryNotEnumeratedException(string category)
        {
            CategoryString = category;
        }
    }
}
