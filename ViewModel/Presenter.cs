namespace AuthorsAssistant.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text.RegularExpressions;
    using System.Windows.Input;

    using AuthorsAssistant.Model;

    /// <summary>
    /// The presenter.
    /// </summary>
    public class Presenter : ObservableObject
    {
        /// <summary>
        /// The word list.
        /// </summary>
        private readonly ObservableCollection<string> wordList = new ObservableCollection<string>();

        /// <summary>
        /// The sentence splitter.
        /// </summary>
        private readonly SentenceSplitter sentenceSplitter = new SentenceSplitter(new Regex(@"((\b[^\s]+\b)((?<=\.\w).)?)"));

        /// <summary>
        /// The a sentence.
        /// </summary>
        private string sentence;

        /// <summary>
        /// The distinct word list.
        /// </summary>
        private IDictionary<string, int> distinctWordList = new Dictionary<string, int>();

        /// <summary>
        /// Gets or sets the sentence.
        /// </summary>
        public string Sentence
        {
            get
            {
                return this.sentence;
            }

            set
            {
                this.sentence = value;
                this.RaisePropertyChangedEvent("sentence");
            }
        }

        /// <summary>
        /// Gets the word list.
        /// </summary>
        public IEnumerable<string> WordList
        {
            get { return this.wordList; }
        }

        /// <summary>
        /// Gets or sets the distinct word list.
        /// </summary>
        public IDictionary<string, int> DistinctWordList
        {
            get
            {
                return this.distinctWordList;
            }

            set
            {
                this.distinctWordList = value;
                this.RaisePropertyChangedEvent("DistinctWordList");
            }
        }

        /// <summary>
        /// Gets the analyze sentence command.
        /// </summary>
        public ICommand AnalyseSentenceCommand
        {
            get { return new DelegateCommand(this.AnalyseSentence); }
        }

        /// <summary>
        /// Gets the clear controls command.
        /// </summary>
        public ICommand ClearControlsCommand
        {
            get { return new DelegateCommand(this.Clear); }
        }

        /// <summary>
        /// The analyze sentence.
        /// </summary>
        private void AnalyseSentence()
        {
            this.wordList.Clear();
            this.distinctWordList = this.sentenceSplitter.SplitSentenceWithDistinctCount(this.Sentence);

            this.PopulateWordList();
        }

        /// <summary>
        /// The populate word list.
        /// </summary>
        private void PopulateWordList()
        {
            foreach (KeyValuePair<string, int> keyValuePair in this.distinctWordList)
            {
                this.wordList.Add(string.Format("{0} - {1}", keyValuePair.Key, keyValuePair.Value));
            }
        }

        /// <summary>
        /// The clear.
        /// </summary>
        private void Clear()
        {
            this.Sentence = string.Empty;
            this.wordList.Clear();
        }
    }
}


