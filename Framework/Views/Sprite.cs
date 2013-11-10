using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Sunfish.Views
{

	public class Sprite : View
	{

		public Texture2D Texture;

		public SpriteFraming Framing;

		public Sprite (Texture2D texture) :
			this(texture, new Vector2(0,0), Constants.ViewLayer.Layer1)
		{
		}

		public Sprite (Texture2D texture, Vector2 position) :
			this(texture, position, Constants.ViewLayer.Layer1)
		{
		}

		public Sprite (Texture2D texture, Constants.ViewLayer layer) :
			this(texture, new Vector2(0,0), layer)
		{
		}

		public Sprite (Texture2D texture, Vector2 position, Constants.ViewLayer layer) :
			this(texture, position, layer, new SpriteFraming(texture))
		{
		}

		public Sprite (Texture2D texture, SpriteFraming framing) :
			this(texture, new Vector2(0,0), Constants.ViewLayer.Layer1, framing)
		{
		}

		public Sprite (Texture2D texture, Vector2 position, SpriteFraming framing) :
			this(texture, position, Constants.ViewLayer.Layer1, framing)
		{
		}

		public Sprite (Texture2D texture, Constants.ViewLayer layer, SpriteFraming framing) :
			this(texture, new Vector2(0,0), layer, framing)
		{
		}

		public Sprite (Texture2D texture, Vector2 position, Constants.ViewLayer layer, SpriteFraming framing) :
			base(position, framing.FrameRectangle.Width, framing.FrameRectangle.Height, layer, true)
		{
			Texture = texture;
			Framing = framing;
		}

		public override void Draw (GameTime gameTime, GraphicsDeviceManager graphics)
		{
			DrawTexture (Texture, Framing.FrameRectangle);
				//DrawBoundingBox ();
		}

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);
			Framing.UpdateFrameRectangle (gameTime);
		}
	}
}

