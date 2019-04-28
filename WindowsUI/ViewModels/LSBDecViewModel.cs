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

				if (Helpers.IsImage(value) && Helpers.CheckResolution(ref value))
					imagePath = value;

			}

		}

		public string DecryptText { get; set; }

		public string ButtonText { get; set; }

		public bool ButtonEnabled { get; set; }

		public RelayCommand GetImageCommand { get; }

		public RelayParameterizedCommand DecryptCommand { get; }

		#endregion

		#region Private Members

		private string imagePath;

		#endregion

		#region Constructor

		public LSBDecViewModel()
		{

			GetImageCommand = new RelayCommand(()=> ImagePath = Helpers.GetImage());
			DecryptCommand = new RelayParameterizedCommand(parameter=>Decrypt((string)parameter));;
			ButtonText = "Rozšifruj";
			ButtonEnabled = true;

		}

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		private void Decrypt(string password)
		{

			if (!Helpers.IsImage(ImagePath)) return;

			var backendThread = new Thread(() => DecryptText = Decrypter.Decrypt(ref imagePath, ref password));

			backendThread.Start();

			new Thread(() =>
			{

				ButtonEnabled = false;

				while (backendThread.ThreadState == ThreadState.Running)
				{

					ButtonText = "Pracuji...";

				}

				ButtonEnabled = true;

				ButtonText = "Rozšifruj";

			}).Start();
		}
		#endregion
	}
}