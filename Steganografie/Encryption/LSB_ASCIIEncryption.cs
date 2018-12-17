using System.Collections.Generic;
using System.Drawing;

namespace Steganografie.Encryption
{
	public class LSB_ASCIIEncryption : BaseEncryption
	{

		public override EncTypes Type => EncTypes.LSB_ASCII;

		public override int MaxChars => TotalImgPixelCount * 3 / 8;

		public override Bitmap Encrypt(string path, string text)
		{

			Bitmap image = PathToBitmap(path);

			int height = image.Height;

			int width = image.Width;

			TotalImgPixelCount = width * height;

			List<int> bits = StringToBitsAscii(text);

			Color[,] newpixels = ReplaceLastBits(GetPixels(width, height, image), bits);
					
			Bitmap newImage = ReplaceImage(newpixels, image);

			return newImage;

		}

		public override string Decrypt(string path, string reference="")
		{

			Bitmap image = PathToBitmap(path);

			int height = image.Height;

			int width = image.Width;

			Color[,] pixels = GetPixels(width,height, image);

			var text = BitsToTextASCII(GetBitsInImage(pixels,width,height,8));

			if (reference != "")
			{

				Bitmap refimg = PathToBitmap(reference);

				Color[,] refpixels = GetPixels(refimg.Width, refimg.Height, refimg);

				var reftext = BitsToTextASCII(GetBitsInImage(refpixels, refimg.Width, refimg.Height,8));

				text = RemoveExcess(text, reftext);

			}
			
			return text;

		}
	}
}