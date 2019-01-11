using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Steganografie.Encryption
{
	public class LSB_UTF8Encryption : BaseEncryption
	{

		public override EncTypes Type => EncTypes.LSB_UTF8;

		public override async Task<Bitmap> Encrypt(string path, string text)
		{

			Bitmap Image = Helpers.PathToBitmap(ref path);

			List<int> bits = StringToBitsUTF8(text);

			return ReplaceLastBits(bits, Image);

		}

		public override async Task<string> Decrypt(string path, string refpath = "")
		{

			Bitmap Image = Helpers.PathToBitmap(ref path);

			var bytes = BitsToBytes(GetBitsInImage(Image.Width, Image.Height, 16, Image));

			if (refpath != "")
			{

				Bitmap refimg = Helpers.PathToBitmap(ref refpath);

				var refbytes = BitsToBytes(GetBitsInImage(refimg.Width, refimg.Height, 16, refimg));

				bytes = RemoveExcess(bytes, refbytes);

			}

			return BytesToTextUTF8(bytes);

		}

	}
}
