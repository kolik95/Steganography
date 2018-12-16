using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Text;

namespace StgLib.Encryption
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

			Color[,] pixels = GetPixels(width, height, image);

			int textCounter = 0;

			for (int y = 0; y < height; y++)
			{

				for (int x = 0; x < width; x++)
				{

					pixels[x, y] = ReplaceR(bits[textCounter], pixels[x, y]);

					textCounter++;

					if (textCounter == bits.Count)
						break;
					
					pixels[x, y] = ReplaceG(bits[textCounter], pixels[x, y]);

					textCounter++;
					
					if (textCounter == bits.Count)
						break;
					
					pixels[x, y] = ReplaceB(bits[textCounter], pixels[x, y]);

					textCounter++;

					if (textCounter == bits.Count)
						break;
					
				}

				if (textCounter == bits.Count)
					break;

			}
					
			Bitmap newImage = ReplaceImage(pixels, image);

			return newImage;

		}

		public override string Decrypt(string path, string reference="")
		{

			Bitmap image = PathToBitmap(path);

			int height = image.Height;

			int width = image.Width;

			Color[,] pixels = GetPixels(width,height, image);

			var text = BitsToTextASCII(GetBitsInImage(pixels,width,height));

			if (reference != "")
			{

				Bitmap refimg = PathToBitmap(reference);

				Color[,] refpixels = GetPixels(refimg.Width, refimg.Height, refimg);

				var reftext = BitsToTextASCII(GetBitsInImage(refpixels, refimg.Width, refimg.Height));

				text = RemoveExcess(text, reftext);

			}
			
			return text;

		}
	}
}