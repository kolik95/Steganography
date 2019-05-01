using System;
using System.Collections.Generic;
using System.Drawing;

namespace Steganografie.Encryption
{
	public class LSB_UTF8Encryption : BaseEncryption
	{

		public override EncTypes Type => EncTypes.LSB_UTF8;

		public override Bitmap Encrypt(ref string path, ref string text, ref string password)
		{

			Bitmap Image = Helpers.PathToBitmap(ref path);

			if (text == "" || text == string.Empty || text == null) return Image;

            if (password != "")
            {

                text = EncryptText(ref text, ref password);

            }

			List<int> bits = StringToBitsUTF8($"{text}/!?*");

			return ReplaceLastBits(bits, Image);

		}

		public override string Decrypt(ref string path, ref string password)
		{
            using (Bitmap Image = Helpers.PathToBitmap(ref path))
            {
                var bytes = BitsToBytes(GetBitsInImage(16, Image));

                bytes = RemoveExcess(ref bytes);

                if (password != "")
                {

                    return DecryptText(bytes, ref password);

                }

                return BytesToTextUTF8(bytes);
            }	
		}
	}
}
