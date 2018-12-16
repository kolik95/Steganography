using System.ComponentModel.Design;
using System.Drawing;

namespace StgLib.Encryption
{
	public class LSB_ASCIIEncryption : BaseEncryption
	{

		public override EncTypes Type => EncTypes.LSB_ASCII;

		public override int MaxChars => TotalImgPixelCount * 3 / 8;

		public override Bitmap Encrypt(string path, string text)
		{

			var image = PathToBitmap(path);

			var height = image.Height;

			var width = image.Width;

			TotalImgPixelCount = width * height;

			var bits = StringToBitsAscii(text);

			var pixels = GetPixels(width, height, image);

			var textCounter = 0;

			for (int y = 0; y < pixels.GetLength(1); y++)
			{

				for (int x = 0; x < pixels.GetLength(0); x++)
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
					
			var newImage = ReplaceImage(pixels, image);

			return newImage;

		}

		public override string Decrypt(string path, string reference="")
		{

			var image = PathToBitmap(path);

			var height = image.Height;

			var width = image.Width;

			var pixels = GetPixels(width,height, image);

			var text = BitsToTextASCII(GetBitsInImage(pixels,width,height));

			if (reference != "")
			{

				var refimg = PathToBitmap(reference);

				var refpixels = GetPixels(refimg.Width, refimg.Height, refimg);

				var reftext = BitsToTextASCII(GetBitsInImage(refpixels, refimg.Width, refimg.Height));

				text = RemoveExcess(text, reftext);

			}
			
			return text;

		}
	}
}