using Steganografie.Encryption;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WindowsUI.RelayCommands;
using System.IO;
using Microsoft.Win32;
using Image = System.Drawing.Image;

namespace WindowsUI.ViewModels
{
	public class LSBViewModel : BaseViewModel
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

		public double ImageHeight => MainWindowViewModel.GetInstance.AppWindow.ActualHeight / 1.6;

		public double TextHeight => MainWindowViewModel.GetInstance.AppWindow.ActualHeight / 1.5;

		public BitmapImage DragImage => new BitmapImage(new Uri(ImagePath, UriKind.RelativeOrAbsolute));

		public string ImagePath { get; set; }

	    #endregion

		#region Private Properties

		private EncTypes EncrytionType { get; set; }

		#endregion

		#region Constructor

		public LSBViewModel()
	    {

		    MainWindowViewModel.GetInstance.AppWindow.SizeChanged += (sender, e) =>
		    {

			    OnPropertyChanged(nameof(TextHeight));

			    OnPropertyChanged(nameof(ImageHeight));

			};

			GetImageCommand = new RelayCommand(GetImage);
		    SaveCommand = new RelayParameterizedCommand(parameter=>Save((string)parameter));

			Background = Brushes.Transparent;

			EncrytionType = EncTypes.LSB_ASCII;

		    ImagePath = "pack://application:,,,/Resources/Images/Transparent.png";


	    }

		#endregion

		#region Public Methods

		public void Drop(object sender, DragEventArgs e)
		{

			var paths = (string[])e.Data.GetData(DataFormats.FileDrop);

			if(paths.Length > 0)
				if (IsImage(paths[0]))
					ImagePath = paths[0];

		}

	    public void ChangeEncryptionType(object sender, SelectionChangedEventArgs e)
	    {

		    EncrytionType = (EncTypes) ((ComboBox) sender).SelectedIndex;

	    }

	    #endregion

		#region Helper Methods

		private Bitmap Encrypt(string Text)
	    {

		    if (ImagePath == "pack://application:,,,/Resources/Images/Transparent.png"
		        && !IsImage(ImagePath)) return null;

		    return Encrypter.Encrypt(ImagePath, Text);

	    }

	    private bool IsImage(string path)
	    {

		    try
		    {

			    Image.FromFile(path);

		    }
		    catch (Exception e)
		    {

			    return false;

		    }

		    return true;

	    }

	    private void GetImage()
	    {

		    var fileDialog = new OpenFileDialog();

		    fileDialog.ShowDialog(MainWindowViewModel.GetInstance.AppWindow);

		    if (IsImage(fileDialog.FileName))
			    ImagePath = fileDialog.FileName;


	    }

	    private void Save(string Text)
	    {

		    Bitmap image = Encrypt(Text);

		    if (image == null) return;

		    var filedalog = new SaveFileDialog();

			filedalog.Filter = "Bitmap Image | *.bmp";

			filedalog.ShowDialog();

		    var fs = (FileStream)filedalog.OpenFile();

			image.Save(fs, ImageFormat.Bmp);

		}

	    #endregion

    }
}