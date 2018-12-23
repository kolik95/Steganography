using Steganografie.Encryption;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WindowsUI.RelayCommands;
using Color = System.Windows.Media.Color;
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

	    public Color ImageBackground { get; set; }

	    public RelayCommand DragLeaveCommand { get; }

	    public RelayCommand DragOverCommand { get; }

	    public RelayParameterizedCommand EncryptCommand { get; }

		public double TextHeight => MainWindowViewModel.GetInstance.AppWindow.ActualHeight / 1.5;

		public BitmapImage DragImage => new BitmapImage(new Uri(CurrentImagePath, UriKind.RelativeOrAbsolute));

	    #endregion

		#region Private Properties

	    private Color White { get; }

	    private Color LightGray { get; }

		private string CurrentImagePath { get; set; }

		private EncTypes EncrytionType { get; set; }

		#endregion

		#region Constructor

		public LSBViewModel()
	    {

		    MainWindowViewModel.GetInstance.AppWindow.SizeChanged += (sender, e) =>
		    {

			    OnPropertyChanged(nameof(TextHeight));

		    };

		    DragOverCommand = new RelayCommand(DragOver);
		    DragLeaveCommand = new RelayCommand(DragLeave);
		    EncryptCommand = new RelayParameterizedCommand(parameter=>Encrypt((string)parameter));

			White = Color.FromRgb(255,255,255);
			LightGray = Color.FromRgb(223,223,223);

		    Background = Brushes.Transparent;
			ImageBackground = White;

			EncrytionType = EncTypes.LSB_ASCII;

		    CurrentImagePath = "pack://application:,,,/Resources/Images/Transparent.png";


	    }

		#endregion

		#region Public Methods

		public void Drop(object sender, DragEventArgs e)
		{

			ImageBackground = White;

			var paths = (string[])e.Data.GetData(DataFormats.FileDrop);

			if(paths.Length > 0)
				if (IsImage(paths[0]))
					CurrentImagePath = paths[0];

		}

	    public void ChangeEncryptionType(object sender, SelectionChangedEventArgs e)
	    {

		    EncrytionType = (EncTypes)((ComboBox)sender).SelectedIndex;

	    }

		#endregion

		#region Helper Methods

	    private void Encrypt(string Text)
	    {

		    if (CurrentImagePath == "pack://application:,,,/Resources/Images/Transparent.png"
		        && !IsImage(CurrentImagePath)) return;

		    Encrypter.Encrypt(CurrentImagePath, Text).Save("Encrypted.bmp", ImageFormat.Bmp);

	    }

		private void DragOver()
	    {

		    ImageBackground = LightGray;

	    }

	    private void DragLeave()
	    {

		    ImageBackground = White;

	    }

	    public bool IsImage(string path)
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

		#endregion

	}
}
