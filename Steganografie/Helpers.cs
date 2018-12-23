using System;
using System.Drawing;

namespace Steganografie
{
	public static class Helpers
	{

		/// <summary>
		/// Converts an image file path to bitmap
		/// <param name="path"></param>
		/// </summary>
		public static Bitmap PathToBitmap(string path)
		{

			return (Bitmap)Image.FromFile(path);

		}
	}
}
