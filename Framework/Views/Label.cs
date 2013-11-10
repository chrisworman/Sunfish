using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sunfish.Utilities;

namespace Sunfish.Views
{
	public class Label : View
	{

		public string Text { get; private set; }

		public Color TextColor;

		private Rectangle[] TextCharacterFrames;

		private Font LabelFont;

		public Label (string text, Font labelFont, Color textColor) :
			this(text, labelFont, textColor, new Vector2(0,0), Constants.ViewLayer.Layer1)
		{
		}

		public Label (string text, Font labelFont, Color textColor, Vector2 position) :
			this(text, labelFont, textColor, position, Constants.ViewLayer.Layer1)
		{
		}

		public Label (string text, Font labelFont, Color textColor, Constants.ViewLayer layer) :
			this(text, labelFont, textColor, new Vector2(0,0), layer)
		{
		}

		public Label (string text, Font labelFont, Color textColor, Vector2 position, Constants.ViewLayer layer) :
			base(position, layer)
		{
			LabelFont = labelFont;
			Height = LabelFont.FontHeight;
			TextColor = textColor;
			SetText (text);
		}

		public void SetText(string newText)
		{
			Text = newText;
			UpdateTextCharacterFramesAndWidth ();
		}

		private void UpdateTextCharacterFramesAndWidth()
		{
			Width = 0;
			char[] textCharacters = Text.ToCharArray ();
			TextCharacterFrames = new Rectangle[textCharacters.Length];
			for (int i=0; i < textCharacters.Length; i ++) {
				char character = textCharacters [i];
				Rectangle fontCharacterFrame = LabelFont.GetCharacterFrame (character);
				TextCharacterFrames [i] = fontCharacterFrame;
				Width += fontCharacterFrame.Width;
			}
		}

		public override void Draw (GameTime gameTime, GraphicsDeviceManager graphics)
		{
			Vector2 characterPosition = Position;
			for (int i=0; i < TextCharacterFrames.Length; i ++) {
				Rectangle textCharacterFrame = TextCharacterFrames [i];
				SunfishGame.ActiveSpriteBatch.Draw (LabelFont.FontTexture, characterPosition, textCharacterFrame, TextColor);  
				characterPosition.X += textCharacterFrame.Width;
			}
		}

	}
}

