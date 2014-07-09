namespace NinjaWordsApi
{
    /// <summary>
    /// Specifies the lexical category in which a word falls under
    /// </summary>
    public enum LexicalCategory
    {
        /// <summary>
        /// A word that can be used to refer to a person, animal, place, 
        /// thing, event, substance, quality, or idea.
        /// </summary>
        Noun,
        /// <summary>
        /// A word that modifies a noun or describes a noun’s referent.
        /// </summary>
        Adjective,
        /// <summary>
        /// A word that indicates an action, event, or state.
        /// </summary>
        Verb,
        /// <summary>
        /// A type of noun that refers anaphorically to another noun or noun
        ///  phrase, but which cannot ordinarily be preceded by a determiner 
        /// and rarely takes an attributive adjective.
        /// </summary>
        Pronoun,
        /// <summary>
        /// A shortened or contracted form of a word or phrase, 
        /// used to represent the whole.
        /// </summary>
        Abbreviation,
        /// <summary>
        /// An exclamation or filled pause; a word or phrase with no particular
        ///  grammatical relation to a sentence, often an expression of emotion.
        /// </summary>
        Interjection,
        /// <summary>
        /// A closed class of non-inflecting words typically employed to connect 
        /// a noun or a pronoun, in an adjectival or adverbial sense, with some other word
        /// </summary>
        Preposition,
        /// <summary>
        /// A part of speech that connects words, sentences, phrases or clauses.
        /// </summary>
        Conjunction
    }
}
