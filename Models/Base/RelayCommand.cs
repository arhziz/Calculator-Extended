using System;
using System.Windows.Input;

namespace Calculator_Extended
{
    public class RelayCommand : ICommand
    {
        #region Private Members
        /// <summary>
        /// action to be run
        /// </summary>
        private Action mAction;
        #endregion

        #region Public Events
        public event EventHandler CanExecuteChanged = (sender, e) => { };
        #endregion

        #region Constructor
        public RelayCommand(Action action)
        {
            mAction = action;
        }
        #endregion

        #region Command Methods
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mAction();
        }
        #endregion
    }
}
