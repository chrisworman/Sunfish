using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Sunfish.Utilities;

namespace Sunfish.Views
{
	public class Font
	{

		private const string SupportedCharacters = ".,:;\"'!?-()#@$%&*+|/0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		private Dictionary<char,Rectangle> FontCharacterFrames;

		public readonly Texture2D FontTexture;

		public readonly int FontHeight;

		public Font(string fontName)
		{
			FontTexture = SunfishGame.ActiveScreen.LoadFontTexture (fontName);
			FontHeight = FontTexture.Height;
			FontCharacterFrames = ComputeFontCharacterFrames (fontName, FontHeight);
		}

		public Rectangle GetCharacterFrame(char character)
		{
			Rectangle characterFrame;
			FontCharacterFrames.TryGetValue (character, out characterFrame);
			return characterFrame;
		}

		private static Dictionary<char, Rectangle> ComputeFontCharacterFrames(string fontName, int fontHeight)
		{

			// Read the font boundaries from the file for the font
			string fontBoundariesFilePath = string.Format ("{0}{1}.txt", Constants.FontBoundaryFolder, fontName);
			string fontBoundariesCsvString = File.ReadAllText (fontBoundariesFilePath);
			Csv fontBoundariesCsv = new Csv (fontBoundariesCsvString);
			int[] fontBoundaries = fontBoundariesCsv.ToIntArray ();

			// Populate FontCharacterFrames, which contains the frames for each supported character within FontTexture
			int boundaryIndex = 0;
			Dictionary<char, Rectangle> result = new Dictionary<char, Rectangle> ();
			char[] supportedCharArray = SupportedCharacters.ToCharArray ();
			foreach (char character in supportedCharArray) {
				int x = fontBoundaries [boundaryIndex++];
				// Special case for ' ' (aka space)
				if (boundaryIndex == 1) {
					Rectangle spaceFrame = new Rectangle (0, 0, (int)(x * 1.5), fontHeight);
					result.Add (' ', spaceFrame);
				}
				int width = fontBoundaries [boundaryIndex++];
				Rectangle characterFrame = new Rectangle (x, 0, width, fontHeight);
				result.Add (character, characterFrame);
			}

			return result;

		}

	}
}

