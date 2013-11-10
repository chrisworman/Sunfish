using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sunfish.Views
{

	public class SpriteFraming
	{

		public Rectangle FrameRectangle;

		protected int FrameCount;

		protected double MillisecondsPerFrame;

		protected int CurrentFrame;

		protected double ElapsedTimeMilliseconds;

		public SpriteFraming(Texture2D texture) :
			this(new Rectangle(0, 0, texture.Width, texture.Height), 1, -1d)
		{
		}

		public SpriteFraming(Rectangle frameRectangle, int frameCount, double millisecondsPerFrame)
		{
			FrameRectangle = frameRectangle;
			FrameCount = frameCount;
			MillisecondsPerFrame = millisecondsPerFrame;
			CurrentFrame = 0;
		}

		public void UpdateFrameRectangle (GameTime gameTime)
		{

			if (FrameCount > 1) {

				ElapsedTimeMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;

				if (ElapsedTimeMilliseconds > MillisecondsPerFrame) {
					CurrentFrame++;
					CurrentFrame = CurrentFrame % FrameCount;
					ElapsedTimeMilliseconds -= MillisecondsPerFrame;
					FrameRectangle.X = FrameRectangle.Width * CurrentFrame;
				}

			}

		}
	}
}

