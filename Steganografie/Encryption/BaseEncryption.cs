using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Steganografie.Encryption
{
	public abstract class BaseEncryption
	{

		/// <summary>
		/// Defines the type of encryption
		/// </summary>
		public abstract EncTypes Type { get; }

		/// <summary>
		/// Encrypts the message
		/// <param name="path"></param>
		/// </summary>
		public abstract Bitmap Encrypt(ref string path, ref string text, ref string password);

		/// <summary>
		/// Decrypts the message
		/// </summary>
		public abstract string Decrypt(ref string path, ref string password);
		
		/// <summary>
		/// Returns a list of
		/// all last bits
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		protected List<int> GetBitsInImage(int filler, Bitmap Image)
		{

			var lastbits = new List<int>();

			for (int y = 0; y < Image.Height; y++)
			{

				for (int x = 0; x < Image.Width; x++)
				{

					lastbits.Add(Image.GetPixel(x,y).R % 2);
					lastbits.Add(Image.GetPixel(x, y).G % 2);
					lastbits.Add(Image.GetPixel(x, y).B % 2);

				}

			}

			return FillList(lastbits, filler);

		}

		/// <summary>
		/// Fills the list with zeroes
		/// so its dividable by the divider
		/// </summary>
		/// <param name="list"></param>
		/// <param name="divider"></param>
		/// <returns></returns>
		protected List<int> FillList(List<int> list, int divider)
		{

			for (int i = divider - list.Count % divider; i > 0; i--)
			{

				list.Add(0);

			}

			return list;

		}

		/// <summary>
		/// Converts input string to
		/// an array of bits
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		protected List<int> StringToBitsAscii(string input)
		{

			byte[] bytes = Encoding.ASCII.GetBytes(input);

			var bits = new BitArray(bytes);

			var bitsInInt = new List<int>();

			foreach (bool bit in bits)
			{

				bitsInInt.Add(Convert.ToInt32(bit));

			}

			return bitsInInt;

		}

		protected List<int> StringToBitsUTF8(string input)
		{

			byte[] bytes = Encoding.UTF8.GetBytes(input);

			var bits = new BitArray(bytes);

			var bitsInInt = new List<int>();

			foreach (bool bit in bits)
			{

				bitsInInt.Add(Convert.ToInt32(bit));

			}

			return bitsInInt;

		}

		/// <summary>
		/// Converts bits into text
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		protected string BytesToTextASCII(byte[] bytes)
		{

			return Encoding.ASCII.GetString(bytes);

		}

		protected string BytesToTextUTF8(byte[] bytes)
		{

			return Encoding.UTF8.GetString(bytes);

		}

		/// <summary>
		/// Converts a list of bits to
		/// an array of bytes
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		protected byte[] BitsToBytes(List<int> bits)
		{

			for (int i = bits.Count % 8; i > -1; i--)
			{

				bits.Remove(bits.Count - i - 1);

			}
			
			var bytes = new byte[bits.Count / 8];

			for (var i = 0; i < bits.Count /8 ; i++)
			{

				var values = new bool[8];
                
				for (var j = 0; j < 8; j++)
				{

					values[j] = Convert.ToBoolean(bits[j + 8 * i]);
                    
				}
             
				new BitArray(values).CopyTo(bytes, i);

			}
			
			return bytes;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pixels"></param>
		/// <param name="bits"></param>
		/// <returns></returns>
		protected Bitmap ReplaceLastBits(List<int> bits, Bitmap Image)
		{

			int textCounter = 0;

			for (int y = 0; y < Image.Height; y++)
			{

				for (int x = 0; x < Image.Width; x++)
				{

					Image.SetPixel(x,y, ReplaceR(bits[textCounter], Image.GetPixel(x,y)));

					textCounter++;

					if (textCounter == bits.Count)
						break;

					Image.SetPixel(x, y, ReplaceG(bits[textCounter], Image.GetPixel(x, y)));

					textCounter++;

					if (textCounter == bits.Count)
						break;

					Image.SetPixel(x, y, ReplaceB(bits[textCounter], Image.GetPixel(x, y)));

					textCounter++;

					if (textCounter == bits.Count)
						break;

				}

				if (textCounter == bits.Count)
					break;

			}

			return Image;

		}

        /// <summary>
        /// Encrypts the input text with a given password and converts it to Base64
        /// </summary>
        /// <returns></returns>
        protected string EncryptText(ref string text, ref string password)
        {

            password = AppendPassword(ref password);

            var newText = new StringBuilder();

            var ms = new MemoryStream();
            var rmCrypto = new RijndaelManaged {KeySize = 128, Key = Encoding.UTF8.GetBytes(password), BlockSize = 256};
            rmCrypto.GenerateIV();
            using (var cryptStream = new CryptoStream(ms,rmCrypto.CreateEncryptor(rmCrypto.Key, rmCrypto.IV), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cryptStream))
            {
                sw.Write(text);
            }

            newText.Append(Convert.ToBase64String(ms.ToArray()));
            newText.Append("IV=");

            foreach (var number in rmCrypto.IV)
            {

                newText.Append($"Num={number}");

            }

            return newText.ToString();

        }


        /// <summary>
        /// Decrypts the input with a given password and converts it from Base64
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected string DecryptText(byte[] input, ref string password)
        {

            string text = Encoding.UTF8.GetString(input);

            password = AppendPassword(ref password);

            byte[] IV = GetIV(Encoding.UTF8.GetString(input).Substring(text.LastIndexOf("IV=") + 3)).ToArray();

            text = text.Remove(text.LastIndexOf("IV="));

            var ms = new MemoryStream(Convert.FromBase64String(text));
            var rmCrypto = new RijndaelManaged {KeySize = 128, Key = Encoding.UTF8.GetBytes(password), BlockSize = 256, IV = IV};
            using (var cryptStream = new CryptoStream(ms, rmCrypto.CreateEncryptor(rmCrypto.Key, rmCrypto.IV), CryptoStreamMode.Read))
            using (var sr = new StreamReader(cryptStream))
            {

                  return sr.ReadToEnd();

            }

        }

	    private IEnumerable<Byte> GetIV(string input)
	    {

            string[] nums = input.Split(new[]{"Num="}, StringSplitOptions.RemoveEmptyEntries);

	        foreach (var num in nums)
	        {

	            yield return Byte.Parse(num);

	        }

	    }

        private string AppendPassword(ref string password)
        {

            var sb = new StringBuilder();

            sb.Append(password);

            for (int i = password.Length; i < 16; i++)
            {

                sb.Append('A');

            }

            return sb.ToString();

        }


        private Color ReplaceR(int value, Color pixel)
		{

			return Color.FromArgb(pixel.A,
				ReplaceLastBit(pixel.R, value),
				pixel.G,
				pixel.B);
			
		}
		
		private Color ReplaceG(int value, Color pixel)
		{

			return Color.FromArgb(pixel.A,
				pixel.R,
				ReplaceLastBit(pixel.G, value),
				pixel.B);
			
		}
		
		private Color ReplaceB(int value, Color pixel)
		{

			return Color.FromArgb(pixel.A,
				pixel.R,
				pixel.G,
				ReplaceLastBit(pixel.B, value));
			
		}

		/// <summary>
		/// Removes excess characters in a string
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		protected byte[] RemoveExcess(ref byte[] text)
		{

			var newtext = new List<byte>();

			for (int i = 0; i < text.Length; i++)
			{

				if (text[i] == 47 &&
					text[i + 1] == 33 &&
				    text[i + 2] == 63 &&
				    text[i + 3] == 42) break;
				newtext.Add(text[i]);

			}

			return newtext.ToArray();

		}
		
		/// <summary>
		/// Replaces the inputs last
		/// bit with the value
		/// </summary>
		/// <param name="input"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private byte ReplaceLastBit(byte input, int value)
		{

			if (input % 2 == 0 && value == 1)
				input++;

			if (input % 2 == 1 && value == 0)
				input--;

			return input;

		}     
	}
}