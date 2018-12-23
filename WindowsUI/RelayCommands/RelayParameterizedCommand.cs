using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowsUI.RelayCommands
{

	public class RelayParameterizedCommand : ICommand
    {

	    public event EventHandler CanExecuteChanged;

	    private Action<object> _action;

	    public bool CanExecute(object parameter)
	    {
		    return true;
	    }

	    public void Execute(object parameter)
	    {

		    _action(parameter);

	    }

	    public RelayParameterizedCommand(Action<object> action)
	    {

		    _action = action;

	    }

	}
}
