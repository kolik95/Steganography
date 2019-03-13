using Steganografie.Encryption;
using System;
using System.Threading;
using System.Windows;
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

				if (Helpers.CheckResolution(ref value))
					imagePath = value;

			}

		}

		public string DecryptText { get; set; }

		public string ButtonText { get; set; }

		public bool ButtonEnabled { get; set; }

		public RelayCommand GetImageCommand { get; }

		public RelayCommand DecryptCommand { get; }

		#endregion

		#region Private Members

		private string imagePath;

		#endregion

		#region Constructor

		public LSBDecViewModel()
		{

			GetImageCommand = new RelayCommand(()=> ImagePath = Helpers.GetImage());
			DecryptCommand = new RelayCommand(Decrypt);
			ButtonText = "Rozšifruj";
			ButtonEnabled = true;

		}

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		private void Decrypt()
		{

			if (!Helpers.IsImage(ImagePath)) return;

			var backendThread = new Thread(() => DecryptText = Decrypter.Decrypt(ref imagePath));

			backendThread.Start();

			new Thread(() =>
			{

				ButtonEnabled = false;

				while (backendThread.ThreadState == ThreadState.Running)
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