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

			Bitmap Image = Helpers.PathToBitmap(path);

			int height = Image.Height;

			int width = Image.Width;

			TotalImgPixelCount = width * height;

			List<int> bits = StringToBitsAscii(text);

			Color[,] newpixels = ReplaceLastBits(GetPixels(width, height, Image), bits);
					
			Bitmap newImage = ReplaceImage(newpixels, Image);

			return newImage;

		}

		public override string Decrypt(string path, string refpath = "")
		{

			Bitmap Image = Helpers.PathToBitmap(path);

			int height = Image.Height;

			int width = Image.Width;

			Color[,] pixels = GetPixels(width,height, Image);

			var text = BitsToTextASCII(GetBitsInImage(pixels,width,height,8));

			if (refpath != "")
			{

				Bitmap refimg = Helpers.PathToBitmap(refpath);

				Color[,] refpixels = GetPixels(refimg.Width, refimg.Height, refimg);

				var reftext = BitsToTextASCII(GetBitsInImage(refpixels, refimg.Width, refimg.Height,8));

				text = RemoveExcess(text, reftext);

			}
			
			return text;

		}
	}
}