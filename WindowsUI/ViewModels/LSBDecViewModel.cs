using Microsoft.Win32;
using Steganografie.Encryption;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using WindowsUI.Others;
using WindowsUI.RelayCommands;

namespace WindowsUI.ViewModels
{
	public class LSBDecViewModel : BaseViewModel
    {

		#region Public Members

		public BaseEncryption Decrypter => MainWindowViewModel.GetInstance.Encrypter;

		public string ImagePath { get; set; }

	    public string ReferenceImagePath { get; set; }

		public string DecryptText { get; set; }

		public RelayParameterizedCommand GetImageCommand { get; }

		public RelayCommand DecryptCommand { get; }

		#endregion

		#region Private Members

		#endregion

		#region Constructor

		public LSBDecViewModel()
		{

			GetImageCommand = new RelayParameterizedCommand(parameter => GetImage((string)parameter));
			DecryptCommand = new RelayCommand(Decrypt);

		}

		#endregion

		#region Public Methods

		public void Drop(ref object sender, ref DragEventArgs e)
		{

			var paths = (string[])e.Data.GetData(DataFormats.FileDrop);

			if (paths.Length == 0) return;
			if (!Others.Helpers.IsImage(paths[0])) return;

			if (((ContentControl)sender).Name == "ImageControl1")
				ImagePath = paths[0];
			else
				ReferenceImagePath = paths[0];

		}

		#endregion

		#region Private Methods

		private void GetImage(string buttonName)
	    {

		    var fileDialog = new OpenFileDialog();

		    fileDialog.ShowDialog(MainWindowViewModel.GetInstance.AppWindow);

		    if (!Others.Helpers.IsImage(fileDialog.FileName)) return;

		    if (buttonName == "ImageButton")
			    ImagePath = fileDialog.FileName;
		    else
			    ReferenceImagePath = fileDialog.FileName;
	    }

		private void Decrypt()
		{

			if (!Helpers.IsImage(ImagePath)) return;

			if (Helpers.IsImage(ReferenceImagePath))
				DecryptText = Decrypter.Decrypt(ImagePath, ReferenceImagePath);
			else
				DecryptText = Decrypter.Decrypt(ImagePath);

		}

		#endregion

	}
}