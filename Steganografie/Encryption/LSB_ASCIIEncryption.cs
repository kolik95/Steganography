using System;
using System.Collections.Generic;
using System.Drawing;

namespace Steganografie.Encryption
{
	public class LSB_ASCIIEncryption : BaseEncryption
	{

        public override EncTypes Type => EncTypes.LSB_ASCII;

		public override Bitmap Encrypt(ref string path, ref string text, ref string password)
		{

			Bitmap Image = Helpers.PathToBitmap(ref path);

			if (text == "" || text == string.Empty || text == null) return Image;

            if (password != "")
            {

                text = EncryptText(ref text, ref password);

            }

			List<int> bits = StringToBitsAscii($"{text}/!?*");

			return ReplaceLastBits(bits, Image);

		}

		public override string Decrypt(ref string path, ref string password)
		{
            using (Bitmap Image = Helpers.PathToBitmap(ref path))
            {

                var bytes = BitsToBytes(GetBitsInImage(8, Image));

                bytes = RemoveExcess(ref bytes);


                return BytesToTextASCII(bytes);

            }
		}
	}
}