using System;
using System.Collections.Generic;
using System.Drawing;

namespace Steganografie.Encryption
{
	public class LSB_UTF8Encryption : BaseEncryption
	{

		public override EncTypes Type => EncTypes.LSB_UTF8;

		public override int MaxChars => TotalImgPixelCount * 3 / 24;

		public override Bitmap Encrypt(string path, string text)
		{

			Bitmap Image = Helpers.PathToBitmap(path);

			int height = Image.Height;

			int width = Image.Width;

			TotalImgPixelCount = width * height;

			List<int> bits = StringToBitsUTF8(text);

			Color[,] newpixels = ReplaceLastBits(GetPixels(width, height, Image), bits);

			Bitmap newImage = ReplaceImage(newpixels, Image);

			return newImage;

		}

		public override string Decrypt(string path, string refpath = "")
		{

			Bitmap Image = Helpers.PathToBitmap(path);

			int height = Image.Height;

			int width = Image.Width;

			Color[,] pixels = GetPixels(width, height, Image);

			var text = BitsToTextUTF8(GetBitsInImage(pixels, width, height, 16));

			if (refpath != "")
			{

				Bitmap refimg = Helpers.PathToBitmap(refpath);

				Color[,] refpixels = GetPixels(refimg.Width, refimg.Height, refimg);

				var reftext = BitsToTextUTF8(GetBitsInImage(refpixels, refimg.Width, refimg.Height,16));

				text = RemoveExcess(text, reftext);

			}

			return text;

		}

	}
}
