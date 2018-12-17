using System;
using System.Drawing.Imaging;
using System.IO;
using Steganografie.Encryption;

namespace Debugger
{
	class Program
	{
		static void Main(string[] args)
		{

			Console.WriteLine("Encrypt-1\nDecrypt-2");

			var input = int.Parse(Console.ReadLine());

			if (input == 1)
			{
				
				Console.WriteLine("Enter a full path to an image:");

				var path = Console.ReadLine();
				
				Console.WriteLine("Enter the text to encrypt: ");

				var text = Console.ReadLine();
				
				var encrypter = new LSB_UTF8Encryption();

				var image = encrypter.Encrypt(path, text);
				
				image.Save("Encrypted.bmp",ImageFormat.Bmp);

			}

			else
			{
				
				Console.WriteLine("Enter a full path to an image:");

				var path = Console.ReadLine();

				Console.WriteLine("Enter a full path to reference image (optional):");

				var path2 = Console.ReadLine();
				
				var encrypter = new LSB_UTF8Encryption();
				
				var image = encrypter.Decrypt(path, path2);
				
				File.WriteAllText("Ahoj.txt", image);
				
			}
		}
	}
}
