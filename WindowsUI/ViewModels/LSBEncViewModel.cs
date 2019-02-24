using Microsoft.Win32;
using Steganografie.Encryption;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
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

			    Bitmap image = (Bitmap)Image.FromFile(value);
			    if (image.Height <= 2160 || image.Width <= 3840)
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

			GetImageCommand = new RelayCommand(GetImage);
		    SaveCommand = new RelayParameterizedCommand(parameter=>Save((string)parameter));

			Background = Brushes.Transparent;

	    }

		#endregion

		#region Public Methods

		public void Drop(ref object sender, ref DragEventArgs e)
		{

			var paths = (string[])e.Data.GetData(DataFormats.FileDrop);

			if(paths.Length > 0)
				if (Others.Helpers.IsImage(paths[0]))
					ImagePath = paths[0];

		}  

	    #endregion

		#region Helper Methods

		private Bitmap Encrypt(string Text)
	    {

		    if (!Others.Helpers.IsImage(ImagePath)) return null;

		    return Encrypter.Encrypt(ImagePath, Text);

	    }

	    private void GetImage()
	    {

		    var fileDialog = new OpenFileDialog();

		    fileDialog.ShowDialog(MainWindowViewModel.GetInstance.AppWindow);

		    if (Others.Helpers.IsImage(fileDialog.FileName)) return;

		    Bitmap image = (Bitmap)Image.FromFile(fileDialog.FileName);
		    if (image.Height < 2160 || image.Width < 3840) return;

			ImagePath = fileDialog.FileName;


	    }

	    private void Save(string Text)
	    {

		    Bitmap image = Encrypt(Text);

			if (image == null) return;

			var fileDialog = new SaveFileDialog
			{
				Filter = "Bitmap Image | *.bmp",
				FileName = "Obrázek"
			};

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