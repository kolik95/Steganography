using PropertyChanged;
using System.ComponentModel;

namespace WindowsUI.ViewModels
{

	[AddINotifyPropertyChangedInterface]
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

		public void OnPropertyChanged(string name)
		{

			PropertyChanged(this, new PropertyChangedEventArgs(name));

		}
	}

}
