using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
		/// Returns the maximum amount of characters
		/// you can encrypt
		/// </summary>
		public abstract int MaxChars { get; }

		/// <summary>
		/// Returns the amount of pixels in the image
		/// </summary>
		protected virtual int TotalImgPixelCount { get; set; }

		/// <summary>
		/// Encrypts the message
		/// <param name="path"></param>
		/// </summary>
		public abstract Bitmap Encrypt(string path, string text);

		/// <summary>
		/// Decrypts the message
		/// </summary>
		public abstract string  Decrypt(string path,string reference="");

		/// <summary>
		/// Converts an image file path to bitmap
		/// <param name="path"></param>
		/// </summary>
		protected virtual Bitmap PathToBitmap(string path)
		{

			return (Bitmap) Image.FromFile(path);

		}
		
		/// <summary>
		/// Gets all pixels in an image
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="image"></param>
		/// <returns></returns>
		protected Color[,] GetPixels(int width, int height, Bitmap image)
		{

			var pixels = new Color[width, height];
			
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
		protected List<int> GetBitsInImage(Color[,] pixels, int width, int height, int filler)
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
		protected string BitsToTextASCII(List<int> bits)
		{

			return Encoding.ASCII.GetString(BitsToBytes(bits));

		}

		protected string BitsToTextUTF8(List<int> bits)
		{

			return Encoding.UTF8.GetString(BitsToBytes(bits));

		}

		/// <summary>
		/// Converts a list of bits to
		/// an array of bytes
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		private byte[] BitsToBytes(List<int> bits)
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
		protected Color[,] ReplaceLastBits(Color[,] pixels, List<int> bits)
		{

			int textCounter = 0;

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

			return pixels;

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
		/// Replaces images pixels
		/// with pixels provided in
		/// a 2D array
		/// </summary>
		/// <param name="pixels"></param>
		/// <param name="original"></param>
		/// <returns></returns>
		protected Bitmap ReplaceImage(Color[,] pixels, Bitmap original)
		{

			for (int y = 0; y < original.Height; y++)
			{

				for (int x = 0; x < original.Width; x++)
				{

					original.SetPixel(x,y,pixels[x,y]);

				}

			}

			return original;

		}

		/// <summary>
		/// Removes excess characters in a string
		/// </summary>
		/// <param name="text"></param>
		/// <param name="text2"></param>
		/// <returns></returns>
		protected string RemoveExcess(string text, string text2)
		{

			var newtext = new StringBuilder();

			for (int i = 0; i < text.Length; i++)
			{

				if (text[i] != text2[i])
				{

					newtext.Append(text[i]);

				}
				
			}

			return newtext.ToString();

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