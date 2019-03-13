using System;
using System.Collections.Generic;
using System.Drawing;

namespace Steganografie.Encryption
{
	public class LSB_ASCIIEncryption : BaseEncryption
	{

		public override EncTypes Type => EncTypes.LSB_ASCII;

		public override Bitmap Encrypt(ref string path, ref string text)
		{

			Bitmap Image = Helpers.PathToBitmap(ref path);

			if (text == "" || text == string.Empty || text == null) return Image;

			List<int> bits = StringToBitsAscii($"{text}/!?*");

			return ReplaceLastBits(bits, Image);

		}

		public override string Decrypt(ref string path)
		{

			Bitmap Image = Helpers.PathToBitmap(ref path);

			var bytes = BitsToBytes(GetBitsInImage(8,Image));

			bytes = RemoveExcess(ref bytes);

			GC.Collect();
			GC.WaitForPendingFinalizers();

			return BytesToTextASCII(bytes);

		}
	}
}