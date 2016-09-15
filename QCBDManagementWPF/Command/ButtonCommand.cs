using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QCBDManagementWPF.ViewModel;
using System.Windows.Threading;
using QCBDManagementCommon.Classes;

namespace QCBDManagementWPF.Command
{
    public class ButtonCommand<P> : ICommand
    {
        private object _lock = new object();
        private Action<P> _excecuteAction;
        private Func<P, bool> _canExecuteAction;

        public event EventHandler CanExecuteChanged;

        public ButtonCommand(Action<P> actionToExecute)
        {
            _excecuteAction = actionToExecute;
        }

        public ButtonCommand(Func<P, bool> canExecute)
        {
            _canExecuteAction = canExecute;
        }

        public ButtonCommand(Action<P> actionToExecute, Func<P, bool> canActionExecute)
        {
            _excecuteAction = actionToExecute;
            _canExecuteAction = canActionExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteAction != null)
            {
                return _canExecuteAction((P)parameter);
            }

            if (_excecuteAction != null)
                return true;

            return false;
        }

        public void Execute(object parameter)
        {
            if (_excecuteAction != null)
                _excecuteAction((P)parameter);
        }

        public void raiseCanExecuteActionChanged()
        {
            try
            {
                if (CanExecuteChanged != null)
                    CanExecuteChanged(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                lock (_lock)
                    Log.write(ex.Message, "ERR");
            }
        }

    }
}
