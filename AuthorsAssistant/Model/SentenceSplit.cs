namespace AuthorsAssistant.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The sentence splitter.
    /// </summary>
    public class SentenceSplitter
    {
        /// <summary>
        /// The sentence splitting regex.
        /// </summary>
        private readonly Regex sentenceSplittingRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="SentenceSplitter"/> class.
        /// </summary>
        /// <param name="sentenceRegex">
        /// The sentence regex.
        /// </param>
        /// <exception cref="Exception">Invalid construction
        /// </exception>
        public SentenceSplitter(Regex sentenceRegex)
        {
            if (sentenceRegex == null)
            {
                throw new Exception("sentenceRegex");
            }

            this.sentenceSplittingRegex = sentenceRegex;
        }

        /// <summary>
        /// The split sentence with distinct count.
        /// </summary>
        /// <param name="sentence">
        /// The sentence.
        /// </param>
        /// <returns>A Dictionary interface of string and integer        
        /// </returns>
        public IDictionary<string, int> SplitSentenceWithDistinctCount(string sentence)
        {
            var words = this.Split(sentence);

            return (from word in words.Distinct()
                    select new { uniqueWord = word, occurs = words.Count(s => s.ToLower().Equals(word.ToLower())) })
                .ToDictionary(x => x.uniqueWord, y => y.occurs);
        }

        /// <summary>
        /// The split.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/>.
        /// </returns>
        public List<string> Split(string text)
        {
            return (from Match match in this.sentenceSplittingRegex.Matches(text) select match.Value.ToLower()).ToList();
        }
    }
}


