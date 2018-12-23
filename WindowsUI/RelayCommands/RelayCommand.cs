using System;
using System.Windows.Input;

namespace WindowsUI.RelayCommands
{
	public class RelayCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private Action _action;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{

			_action();

		}

		public RelayCommand(Action action)
		{

			_action = action;

		}

	}
}
