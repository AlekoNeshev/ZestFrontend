using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZestFrontend.Parameters
{
	public class ReplyCommand : ICommand
	{
		private readonly Action<ReplyCommandParameter> _execute;
		private readonly Func<ReplyCommandParameter, bool> _canExecute;

		public ReplyCommand(Action<ReplyCommandParameter> execute, Func<ReplyCommandParameter, bool> canExecute = null)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
			_canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute((ReplyCommandParameter)parameter);
		}

		public void Execute(object parameter)
		{
			_execute((ReplyCommandParameter)parameter);
		}
	}
}


