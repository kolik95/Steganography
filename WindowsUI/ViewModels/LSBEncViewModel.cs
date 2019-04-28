using Microsoft.Win32;
using Steganografie.Encryption;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using WindowsUI.Others;
using WindowsUI.RelayCommands;

namespace WindowsUI.ViewModels
{
	public class LSBEncViewModel : BaseViewModel
    {

		#region Public Properties

	    public BaseEncryption Encrypter => MainWindowViewModel.GetInstance.Encrypter;

	    public Brush Background { get; set; }

	    public RelayCommand GetImageCommand { get; }

	    public RelayParameterizedCommand SaveCommand { get; }

	    public string ImagePath
	    {

		    get { return imagePath;}

		    set
		    {
			    if (Helpers.IsImage(value) && Helpers.CheckResolution(ref value))
				    imagePath = value;

		    }
	    }

	    #endregion

		#region Private Properties

		private string imagePath;

		#endregion

		#region Constructor

		public LSBEncViewModel()
	    {

			GetImageCommand = new RelayCommand(()=>ImagePath = Helpers.GetImage());
		    SaveCommand = new RelayParameterizedCommand(parameter=>Save((EncryptionParameters)parameter));

			Background = Brushes.Transparent;

	    }

		#endregion

		#region Public Methods

	    #endregion

		#region Helper Methods

		private Bitmap Encrypt(string Text, string password)
	    {

		    if (!Others.Helpers.IsImage(imagePath)) return null;

		    return Encrypter.Encrypt(ref imagePath, ref Text, ref password);

	    }

	    private void Save(EncryptionParameters Parameters)
	    {

		    Bitmap image = Encrypt(Parameters.Text, Parameters.Password);

			if (image == null) return;

			var fileDialog = new SaveFileDialog
			{
				Filter = "Bitmap Image | *.bmp",
				FileName = "Obrázek"
			};

		    fileDialog.OverwritePrompt = true;

			fileDialog.ShowDialog(MainWindowViewModel.GetInstance.AppWindow);

			if (fileDialog.FileName == "Obrázek") return;

			var fs = (FileStream) fileDialog.OpenFile();

			image.Save(fs, ImageFormat.Bmp);

			fs.Close();

			image.Dispose();

	    }

	    #endregion

    }
}