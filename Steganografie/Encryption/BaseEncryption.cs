using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
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
		public abstract Bitmap Encrypt(string path, string text);

		/// <summary>
		/// Decrypts the message
		/// </summary>
		public abstract string  Decrypt(string path, string refpath = "");

		private int ReplacedBits { get; set; }
		
		/// <summary>
		/// Gets all pixels in an image
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="image"></param>
		/// <returns></returns>
		protected Color[,] GetPixels(Bitmap image)
		{

			var pixels = new Color[image.Width, image.Height];
			
			for (int x = 0; x < image.Width; x++)
			{

				for (int y = 0; y < image.Height; y++)
				{

					pixels[x, y] = image.GetPixel(x, y);

				}

			}

			return pixels;

		}
		
		/// <summary>
		/// Returns a list of
		/// all last bits
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		protected List<int> GetBitsInImage(ref Color[,] pixels, int width, int height, int filler)
		{
	
			var lastbits = new List<int>();

			for (int y = 0; y < height; y++)
			{

				for (int x = 0; x < width; x++)
				{

					lastbits.Add(pixels[x, y].R % 2);
					lastbits.Add(pixels[x, y].G % 2);
					lastbits.Add(pixels[x, y].B % 2);

				}

			}

			return FillList(lastbits, ref filler);

		}

		/// <summary>
		/// Fills the list with zeroes
		/// so its dividable by the divider
		/// </summary>
		/// <param name="list"></param>
		/// <param name="divider"></param>
		/// <returns></returns>
		protected List<int> FillList(List<int> list, ref int divider)
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
		protected List<int> StringToBitsAscii(ref string input)
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

		protected List<int> StringToBitsUTF8(ref string input)
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
		protected string BytesToTextASCII(ref byte[] bytes)
		{

			return Encoding.ASCII.GetString(bytes);

		}

		protected string BytesToTextUTF8(ref byte[] bytes)
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
		protected Color[,] ReplaceLastBits(Color[,] pixels, ref List<int> bits)
		{

			int textCounter = 0;

			for (int y = 0; y < pixels.GetLength(1); y++)
			{

				for (int x = 0; x < pixels.GetLength(0); x++)
				{

					pixels[x, y] = ReplaceR(bits[textCounter], ref pixels[x, y]);

					textCounter++;

					if (textCounter == bits.Count)
						break;

					pixels[x, y] = ReplaceG(bits[textCounter], ref pixels[x, y]);

					textCounter++;

					if (textCounter == bits.Count)
						break;

					pixels[x, y] = ReplaceB(bits[textCounter], ref pixels[x, y]);

					textCounter++;

					if (textCounter == bits.Count)
						break;

				}

				if (textCounter == bits.Count)
					break;

			}

			return pixels;

		}

		private Color ReplaceR(int value, ref Color pixel)
		{

			ReplacedBits++;

			return Color.FromArgb(pixel.A,
				ReplaceLastBit(pixel.R, value),
				pixel.G,
				pixel.B);
			
		}
		
		private Color ReplaceG(int value, ref Color pixel)
		{

			ReplacedBits++;

			return Color.FromArgb(pixel.A,
				pixel.R,
				ReplaceLastBit(pixel.G, value),
				pixel.B);
			
		}
		
		private Color ReplaceB(int value, ref Color pixel)
		{

			ReplacedBits++;

			return Color.FromArgb(pixel.A,
				pixel.R,
				pixel.G,
				ReplaceLastBit(pixel.B, value));
			
		}

		/// <summary>
		/// Replaces images pixels
		/// with pixels provided in
		/// a 2D array
		/// </summary>
		/// <param name="pixels"></param>
		/// <param name="original"></param>
		/// <returns></returns>
		protected Bitmap ReplaceImage(ref Color[,] pixels, Bitmap original)
		{

			int Counter = 0;

			for (int y = 0; y < original.Height; y++)
			{

				for (int x = 0; x < original.Width; x++)
				{

					original.SetPixel(x,y,pixels[x,y]);

					Counter++;

					if (Counter == ReplacedBits)
					{

						ReplacedBits = 0;

						return original;
					}

				}

			}

			ReplacedBits = 0;

			return original;

		}

		/// <summary>
		/// Removes excess characters in a string
		/// </summary>
		/// <param name="text"></param>
		/// <param name="text2"></param>
		/// <returns></returns>
		protected byte[] RemoveExcess(ref byte[] text, ref byte[] text2)
		{

			var newtext = new List<byte>();

			foreach (var letter in SubtractArrays(text, text2))
			{

				newtext.Add(letter);

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

		/// <summary>
		/// Removes the common part of two strings
		/// </summary>
		/// <param name="string1"></param>
		/// <param name="string2"></param>
		/// <returns></returns>
		private IEnumerable<byte> SubtractArrays(byte[] string1, byte[] string2)
		{

			for (int i = 0; i < string1.Length; i++)
			{

				if (string1[i] != string2[i])
				{

					yield return string1[i];

				}

			}

		}
	}
}