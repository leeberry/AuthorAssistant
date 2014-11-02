using AuthorsAssistant.Model;

namespace UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    
    using NUnit.Framework;

    /// <summary>
    /// The class 1.
    /// </summary>
    [TestFixture]
    public class SentenceSplittingTests
    {
        /// <summary>
        /// The splitting regex
        /// </summary>
        private readonly Regex splittingRegex = new Regex(@"((\b[^\s]+\b)((?<=\.\w).)?)");

        /// <summary>
        /// The test sentences.
        /// </summary>
        private Dictionary<string, string> validSentences = new Dictionary<string, string>();

        /// <summary>
        /// The sentence to validate.
        /// </summary>
        private string sentenceToValidate;

        /// <summary>
        /// The prose splitter.
        /// </summary>
        private SentenceSplitter sentenceSplitter;

        /// <summary>
        /// Set up the test fixture.
        /// </summary>
        [SetUp]
        public void Initiailise()
        {
            this.LoadTestSentences();
            this.sentenceSplitter = new SentenceSplitter(this.splittingRegex);
        }

        /// <summary>
        /// The distinct word count is correct in simple sentence.
        /// </summary>
        [Test]
        [Description("Given a sentence | When the program is run | Then I am returned a distinct list of words in the sentence and the number of times they have appeared")]
        public void WordCountCorrectInSimpleSentence()
        {
            if (this.validSentences.TryGetValue("1", out this.sentenceToValidate))
            {
                IDictionary<string, int> result = this.sentenceSplitter.SplitSentenceWithDistinctCount(this.sentenceToValidate);

                Assert.AreEqual(8, result.Count);

                foreach (var word in result)
                {
                    Assert.AreEqual(word.Key == "the" ? 2 : 1, word.Value);
                }
            }
        }

        /// <summary>
        /// Case of a word has no affect.
        /// </summary>
        [Test]
        [Description("Given a sentence | When the casing of two identical words is different | Then the words are still recognised as the same")]
        public void DifferentlyCasedWordsAreRecognisedAsTheSame()
        {
            if (this.validSentences.TryGetValue("2", out this.sentenceToValidate))
            {
                IDictionary<string, int> result = this.sentenceSplitter.SplitSentenceWithDistinctCount(this.sentenceToValidate);

                Assert.AreEqual(8, result.Count);

                foreach (var word in result)
                {
                    switch (word.Key)
                    {
                        case "the":
                            Assert.AreEqual(word.Value, 3);
                            break;
                        case "over":
                            Assert.AreEqual(word.Value, 2);
                            break;
                        default:
                            Assert.AreEqual(word.Value, 1);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// The sentences with invalid words are treated as valid.
        /// </summary>
        [Test]
        [Description("Given a sentence | When a word contains an invalid character | Then the sentence is still considered valid ")]
        public void SentencesWithInvalidWordsAreTreatedAsValid()
        {
            if (this.validSentences.TryGetValue("3", out this.sentenceToValidate))
            {
                IDictionary<string, int> result = this.sentenceSplitter.SplitSentenceWithDistinctCount(this.sentenceToValidate);

                Assert.AreEqual(11, result.Count);

                foreach (var word in result)
                {
                    Assert.AreEqual(word.Value, 1);
                }
            }
        }

        /// <summary>
        /// The decimal numbers are parsed as words.
        /// </summary>
        [Test]
        [Description("Given a sentence | When decimal numbers are contained | Then those numbers will be treated as words")]
        public void DecimalNumbersAreParsedAsWords()
        {
            if (this.validSentences.TryGetValue("4", out this.sentenceToValidate))
            {
                IDictionary<string, int> result = this.sentenceSplitter.SplitSentenceWithDistinctCount(this.sentenceToValidate);

                Assert.AreEqual(10, result.Count);

                foreach (var word in result)
                {
                    switch (word.Key)
                    {
                        case "1.23l":
                            Assert.AreEqual(word.Value, 2);
                            break;
                        case "of":
                            Assert.AreEqual(word.Value, 2);
                            break;
                        default:
                            Assert.AreEqual(word.Value, 1);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// The email addresses are treated as one word.
        /// </summary>
        [Test]
        [Description("Given a sentence | When an email address is contained | Then the whole address is treated as one word")]
        public void EmailAddressesAreTreatedAsOneWord()
        {
            if (this.validSentences.TryGetValue("5", out this.sentenceToValidate))
            {
                IDictionary<string, int> result = this.sentenceSplitter.SplitSentenceWithDistinctCount(this.sentenceToValidate);

                Assert.AreEqual(5, result.Count);

                Assert.IsTrue(result.ContainsKey("john.doe@nowhere.com"));

                foreach (var word in result)
                {
                    Assert.AreEqual(word.Value, 1);
                }
            }
        }

        /// <summary>
        /// The single words sentences are analyzed correctly.
        /// </summary>
        [Test]
        [Description("Given a sentence | When it only contains one word | Then a correct distinct count is still returned")]
        public void SingleWordsSentencesAreAnalysedCorrectly()
        {
            if (this.validSentences.TryGetValue("6", out this.sentenceToValidate))
            {
                IDictionary<string, int> result = this.sentenceSplitter.SplitSentenceWithDistinctCount(this.sentenceToValidate);

                Assert.AreEqual(1, result.Count);

                Assert.IsTrue(result.ContainsKey("two"));

                Assert.AreEqual(1, result.First(w => w.Key == "two").Value);
            }
        }

        /// <summary>
        /// The single number sentences are analyzed correctly.
        /// </summary>
        [Test]
        [Description("Given a sentence | When it is just one number | Then a correct distinct count is still returned ")]
        public void SingleNumberSentencesAreAnalysedCorrectly()
        {
            if (this.validSentences.TryGetValue("7", out this.sentenceToValidate))
            {
                IDictionary<string, int> result = this.sentenceSplitter.SplitSentenceWithDistinctCount(this.sentenceToValidate);

                Assert.AreEqual(1, result.Count);

                Assert.IsTrue(result.ContainsKey("2"));

                Assert.AreEqual(1, result.First(w => w.Key == "2").Value);
            }
        }

        /// <summary>
        /// The sentences with speech marks are analyzed correctly.
        /// </summary>
        [Test]
        [Description("Given a sentence | When it contains speech marks | Then a correct distinct count of all words is still returned")]
        public void SentencesWithSpeechMarksAreAnalysedCorrectly()
        {
            if (this.validSentences.TryGetValue("8", out this.sentenceToValidate))
            {
                IDictionary<string, int> result = this.sentenceSplitter.SplitSentenceWithDistinctCount(this.sentenceToValidate);

                Assert.AreEqual(10, result.Count);

                Assert.IsTrue(result.ContainsKey("hi"));

                foreach (var word in result)
                {
                    Assert.AreEqual(word.Value, 1);
                }
            }
        }

        /// <summary>
        /// The sentences with colons are analyzed correctly.
        /// </summary>
        [Test]
        [Description("Given a sentence | When it contains a colon | Then a correct distinct count of all words is still returned")]
        public void SentencesWithColonsAreAnalysedCorrectly()
        {
            if (this.validSentences.TryGetValue("9", out this.sentenceToValidate))
            {
                IDictionary<string, int> result = this.sentenceSplitter.SplitSentenceWithDistinctCount(this.sentenceToValidate);

                Assert.AreEqual(10, result.Count);

                Assert.IsTrue(result.ContainsKey("includes"));

                foreach (var word in result)
                {
                    switch (word.Key)
                    {
                        case "the":
                            Assert.AreEqual(word.Value, 4);
                            break;
                        default:
                            Assert.AreEqual(word.Value, 1);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// The sentences with interim question marks are not terminated prematurely.
        /// </summary>
        [Test]
        [Description("Given a sentence | When it contains question marks inside | Then all subsequent words are analysed correctly as part of the sentence")]
        public void SentencesWithInterimQuestionMarksAreNotTerminatedPrematurely()
        {
            if (this.validSentences.TryGetValue("10", out this.sentenceToValidate))
            {
                IDictionary<string, int> result = this.sentenceSplitter.SplitSentenceWithDistinctCount(this.sentenceToValidate);

                Assert.AreEqual(25, result.Count);

                foreach (var word in result)
                {
                    switch (word.Key)
                    {
                        case "the":
                            Assert.AreEqual(word.Value, 4);
                            break;
                        case "is":
                            Assert.AreEqual(word.Value, 2);
                            break;
                        default:
                            Assert.AreEqual(word.Value, 1);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// The sentences with interim exclamation marks are not terminated prematurely.
        /// </summary>
        [Test]
        [Description("Given a sentence | When it contains an exclamation mark inside | Then all subsequent words are analysed correctly as part of the sentence")]
        public void SentencesWithInterimExclamationMarksAreNotTerminatedPrematurely()
        {
            if (this.validSentences.TryGetValue("11", out this.sentenceToValidate))
            {
                IDictionary<string, int> result = this.sentenceSplitter.SplitSentenceWithDistinctCount(this.sentenceToValidate);

                Assert.AreEqual(7, result.Count);

                foreach (var word in result)
                {
                    switch (word.Key)
                    {
                        case "went":
                            Assert.AreEqual(word.Value, 2);
                            break;
                        default:
                            Assert.AreEqual(word.Value, 1);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// The load test sentences.
        /// </summary>
        private void LoadTestSentences()
        {
            var xdoc = XDocument.Load("../../Sentences.xml");
            if (xdoc.Root != null)
            {
                this.validSentences = xdoc.Root.Elements().ToDictionary(a => (string)a.Attribute("test"), a => (string)a.Attribute("text"));
            }
        }
    }
}


