using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WindowsUI.RelayCommands;
using Microsoft.Win32;
using Steganografie.Encryption;

namespace WindowsUI.ViewModels
{
    public class LSBDecViewModel : BaseViewModel
    {

		#region Public Members

		public BaseEncryption Decrypter
	    {

		    get
		    {

			    if (EncrytionType == EncTypes.LSB_ASCII)
				    return new LSB_ASCIIEncryption();
				return new LSB_UTF8Encryption();
			}

	    }

	    public string ImagePath { get; set; }

	    public string ReferenceImagePath { get; set; }

		public string DecryptText { get; set; }

	    public BitmapImage Image => new BitmapImage(new Uri(ImagePath, UriKind.RelativeOrAbsolute));

		public BitmapImage ReferenceImage => new BitmapImage(new Uri(ReferenceImagePath, UriKind.RelativeOrAbsolute));

		public RelayParameterizedCommand GetImageCommand { get; }

		public RelayCommand DecryptCommand { get; }

		#endregion

		#region Private Members

		private EncTypes EncrytionType => MainWindowViewModel.GetInstance.EncrytionType;

		#endregion

		#region Constructor

		public LSBDecViewModel()
		{

			ImagePath = Helpers.DefaultPath;
			ReferenceImagePath = Helpers.DefaultPath;

			GetImageCommand = new RelayParameterizedCommand(parameter => GetImage((string)parameter));
			DecryptCommand = new RelayCommand(Decrypt);

		}

		#endregion

		#region Public Methods

		public void Drop(object sender, DragEventArgs e)
		{

			var paths = (string[])e.Data.GetData(DataFormats.FileDrop);

			if (paths.Length == 0) return;
			if (!Helpers.IsImage(paths[0])) return;

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

		    if (!Helpers.IsImage(fileDialog.FileName)) return;

		    if (buttonName == "ImageButton")
			    ImagePath = fileDialog.FileName;
		    else
			    ReferenceImagePath = fileDialog.FileName;
	    }

	    private void Decrypt()
	    {

		    if (ImagePath == Helpers.DefaultPath) return;

		    if (ReferenceImagePath == Helpers.DefaultPath)
			    DecryptText = Decrypter.Decrypt(ImagePath);
		    else
			    DecryptText = Decrypter.Decrypt(ImagePath, ReferenceImagePath);

	    }

		#endregion

	}
}