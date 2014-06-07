using System.Text;
using System.Text.RegularExpressions;

namespace NinjaWordsApi
{
    static class ExtensionMethods
    {
        /// <summary>
        /// Removes sections of a string using the ranges of matches 
        /// in a MatchCollection
        /// </summary>
        public static void Remove(this StringBuilder SB, MatchCollection MC)
        {
            for (int i = MC.Count - 1; i >= 0; i--)
                SB.Remove(MC[i].Index, MC[i].Length);
        }

        /// <summary>
        /// Trim all leading and trailing whitespace for each element of the array
        /// </summary>
        public static void TrimAll(this string[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = array[i].Trim();
        }

        /// <summary>
        /// Trims the specified characters from the end of the StringBuilder
        /// </summary>
        public static void TrimEnd(this StringBuilder SB, char c)
        {
            while (SB[SB.Length - 1] == ',')
                SB.Remove(SB.Length - 1, 1);
        } 
    }
}
