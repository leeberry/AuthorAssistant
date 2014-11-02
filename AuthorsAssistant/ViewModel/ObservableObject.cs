namespace AuthorsAssistant.ViewModel
{
    using System.ComponentModel;

    /// <summary>
    /// Base class for the ViewModel classes that handles the implementation of the INotifyPropertyChanged interface
    /// This interface allows update messages to be passed to the View.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The raise property changed event.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}


