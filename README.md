NinjaWordsApi
=============

An API for NinjaWords.com

'''c#
        /// Removes sections of a string using the ranges of matches 
        /// in a MatchCollection
        /// </summary>
        public static void Remove(this StringBuilder SB, MatchCollection MC)
        {
            for (int i = MC.Count - 1; i >= 0; i--)
                SB.Remove(MC[i].Index, MC[i].Length);
        }
'''
