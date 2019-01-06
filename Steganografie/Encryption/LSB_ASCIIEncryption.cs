using System.Collections.Generic;
using System.Drawing;

namespace Steganografie.Encryption
{
	public class LSB_ASCIIEncryption : BaseEncryption
	{

		public override EncTypes Type => EncTypes.LSB_ASCII;

		public override Bitmap Encrypt(string path, string text)
		{

			Bitmap Image = Helpers.PathToBitmap(ref path);

			List<int> bits = StringToBitsAscii(ref text);

			Color[,] newpixels = ReplaceLastBits(GetPixels(Image, bits.Count), ref bits);
					
			Bitmap newImage = ReplaceImage(ref newpixels, Image);

			return newImage;

		}

		public override string Decrypt(string path, string refpath = "")
		{

			Bitmap Image = Helpers.PathToBitmap(ref path);

			Color[,] pixels = GetPixels(Image, Image.Height*Image.Width);

			var bytes = BitsToBytes(GetBitsInImage(ref pixels, Image.Width, Image.Height, 8));

			if (refpath != "")
			{

				Bitmap refimg = Helpers.PathToBitmap(ref refpath);

				Color[,] refpixels = GetPixels(refimg, refimg.Height*refimg.Width);

				var refbytes = BitsToBytes(GetBitsInImage(ref refpixels, refimg.Width, refimg.Height, 8));

				bytes = RemoveExcess(ref bytes, ref refbytes);

			}

			var text = BytesToTextASCII(ref bytes);

			return text;

		}
	}
}