using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Steganografie.Encryption
{
	public class LSB_ASCIIEncryption : BaseEncryption
	{

		public override EncTypes Type => EncTypes.LSB_ASCII;

		public override Bitmap Encrypt(string path, string text)
		{

			Bitmap Image = Helpers.PathToBitmap(ref path);

			List<int> bits = StringToBitsAscii(text);

			return ReplaceLastBits(bits, Image);

		}

		public override string Decrypt(string path, string refpath = "")
		{

			Bitmap Image = Helpers.PathToBitmap(ref path);

			var bytes = BitsToBytes(GetBitsInImage(Image.Width, Image.Height, 8,Image));

			if (refpath != "")
			{

				Bitmap refimg = Helpers.PathToBitmap(ref refpath);

				var refbytes = BitsToBytes(GetBitsInImage(refimg.Width, refimg.Height, 8, refimg));

				bytes = RemoveExcess(bytes, refbytes);

			}

			return BytesToTextASCII(bytes);

		}
	}
}