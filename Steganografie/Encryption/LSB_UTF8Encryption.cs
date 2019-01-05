using System;
using System.Collections.Generic;
using System.Drawing;

namespace Steganografie.Encryption
{
	public class LSB_UTF8Encryption : BaseEncryption
	{

		public override EncTypes Type => EncTypes.LSB_UTF8;

		public override Bitmap Encrypt(string path, string text)
		{

			Bitmap Image = Helpers.PathToBitmap(ref path);

			List<int> bits = StringToBitsUTF8(ref text);

			Color[,] newpixels = ReplaceLastBits(GetPixels(Image), ref bits);

			Bitmap newImage = ReplaceImage(ref newpixels, Image);

			return newImage;

		}

		public override string Decrypt(string path, string refpath = "")
		{

			Bitmap Image = Helpers.PathToBitmap(ref path);

			Color[,] pixels = GetPixels(Image);

			var bytes = BitsToBytes(GetBitsInImage(ref pixels, Image.Width, Image.Height, 16));

			if (refpath != "")
			{

				Bitmap refimg = Helpers.PathToBitmap(ref refpath);

				Color[,] refpixels = GetPixels(refimg);

				var refbytes = BitsToBytes(GetBitsInImage(ref refpixels, refimg.Width, refimg.Height, 16));

				bytes = RemoveExcess(ref bytes, ref refbytes);

			}

			var text = BytesToTextUTF8(ref bytes);

			return text;

		}

	}
}
