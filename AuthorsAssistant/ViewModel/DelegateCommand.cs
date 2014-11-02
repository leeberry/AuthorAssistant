namespace AuthorsAssistant.ViewModel
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Delegate to which the commands in the view can bind
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// The action.
        /// </summary>
        private readonly Action action;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public DelegateCommand(Action action)
        {
            this.action = action;
        }

#pragma warning disable 67
        /// <summary>
        /// The can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            this.action();
        }

        /// <summary>
        /// The can execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}


