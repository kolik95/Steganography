using System;
using Microsoft.Win32;
using Steganografie.Encryption;
using System.Drawing;
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

		public string ImagePath
		{

			get { return imagePath; }

			set
			{

				Bitmap image = (Bitmap)System.Drawing.Image.FromFile(value);
				if (image.Height <= 2160 || image.Width <= 3840)
					imagePath = value;

			}

		}

		public string ReferenceImagePath
		{

			get { return imageReferencePath; }

			set
			{

				Bitmap image = (Bitmap)System.Drawing.Image.FromFile(value);
				if (image.Height <= 2160 || image.Width <= 3840)
					imageReferencePath = value;

			}

		}

		public string DecryptText { get; set; }

		public string ButtonText { get; set; }

		public bool ButtonEnabled { get; set; }

		public RelayParameterizedCommand GetImageCommand { get; }

		public RelayCommand DecryptCommand { get; }

		#endregion

		#region Private Members

		private string imagePath;

		private string imageReferencePath;

		#endregion

		#region Constructor

		public LSBDecViewModel()
		{

			GetImageCommand = new RelayParameterizedCommand(parameter => GetImage((string)parameter));
			DecryptCommand = new RelayCommand(Decrypt);
			ButtonText = "Rozšifruj";
			ButtonEnabled = true;

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

			Thread thread;

			if (Helpers.IsImage(ReferenceImagePath))
				thread = new Thread(() =>
				{
					DecryptText = Decrypter.Decrypt(ImagePath, ReferenceImagePath);
					GC.Collect();
					GC.WaitForPendingFinalizers();
				});
			else
				thread = new Thread(() =>
				{
					DecryptText = Decrypter.Decrypt(ImagePath).Remove(250);
					GC.Collect();
					GC.WaitForPendingFinalizers();
				});

			thread.Start();

			new Thread(() =>
			{

				ButtonEnabled = false;

				while (thread.ThreadState == ThreadState.Running)
				{

					ButtonText = "Pracuji.";
					Thread.Sleep(1200);
					ButtonText = "Pracuji..";
					Thread.Sleep(1200);
					ButtonText = "Pracuji...";
					Thread.Sleep(1200);

				}

				ButtonEnabled = true;

				ButtonText = "Rozšifruj";

			}).Start();
		}
		#endregion
	}
}