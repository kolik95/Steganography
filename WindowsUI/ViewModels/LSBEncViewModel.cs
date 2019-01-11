using Microsoft.Win32;
using Steganografie.Encryption;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WindowsUI.RelayCommands;

namespace WindowsUI.ViewModels
{
	public class LSBEncViewModel : BaseViewModel
    {

		#region Public Properties

	    public BaseEncryption Encrypter
	    {

		    get
		    {

				if(EncrytionType == 0)
					return new LSB_ASCIIEncryption();
				return new LSB_UTF8Encryption();

		    }

	    }

	    public Brush Background { get; set; }

	    public RelayCommand GetImageCommand { get; }

	    public RelayParameterizedCommand SaveCommand { get; }

		public BitmapImage DragImage => new BitmapImage(new Uri(ImagePath, UriKind.RelativeOrAbsolute));

		public string ImagePath { get; set; }

	    #endregion

		#region Private Properties

	    private EncTypes EncrytionType => MainWindowViewModel.GetInstance.EncrytionType;

		#endregion

		#region Constructor

		public LSBEncViewModel()
	    {

			GetImageCommand = new RelayCommand(GetImage);
		    SaveCommand = new RelayParameterizedCommand(async parameter=>await Save((string)parameter));

			Background = Brushes.Transparent;

		    ImagePath = Others.Helpers.DefaultPath;


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

		private async Task<Bitmap> Encrypt(string Text)
	    {

		    if (ImagePath == Others.Helpers.DefaultPath
		        && !Others.Helpers.IsImage(ImagePath)) return null;

		    return await Encrypter.Encrypt(ImagePath, Text);

	    }

	    private void GetImage()
	    {

		    var fileDialog = new OpenFileDialog();

		    fileDialog.ShowDialog(MainWindowViewModel.GetInstance.AppWindow);

		    if (Others.Helpers.IsImage(fileDialog.FileName))
			    ImagePath = fileDialog.FileName;


	    }

	    private async Task Save(string Text)
	    {

		    Bitmap image = await Encrypt(Text);

		    if (image == null) return;

		    var fileDialog = new SaveFileDialog
		    {
			    Filter = "Bitmap Image | *.bmp",
			    FileName = "Obrázek"
		    };

		    fileDialog.ShowDialog(MainWindowViewModel.GetInstance.AppWindow);

		    if (fileDialog.FileName == "Obrázek") return;

		    var fs = (FileStream)fileDialog.OpenFile();

			image.Save(fs, ImageFormat.Bmp);

			fs.Close();

			image.Dispose();

		}

	    #endregion

    }
}