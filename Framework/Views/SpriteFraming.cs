using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sunfish.Views
{

	public class SpriteFraming
	{

		/// <summary>
		/// A rectangle (positioned relative to the texture) that will used to draw a sub-rectangle of the texture.
		/// </summary>
		public Rectangle FrameRectangle;

		/// <summary>
		/// The number of frames in the sprite sheet: 1 implies a static sprite.
		/// </summary>
		protected int FrameCount;

		protected double MillisecondsPerFrame;

		protected int CurrentFrame;

		protected double ElapsedTimeMilliseconds;

		/// <summary>
		/// The number of times that the frame rectangle will move through all sprites.
		/// </summary>
		public int Loops = Constants.SpriteFramingLoopInfinte;

		/// <summary>
		/// The number of time that the frame rectangle has moved through all the sprites thus far.
		/// </summary>
		protected int LoopCounter = 0;

		/// <summary>
		/// The behavior when looping has finished.
		/// </summary>
		public Constants.SpriteFramingLoopingFinishedBehavior LoopingFinishedBehavior = Constants.SpriteFramingLoopingFinishedBehavior.FirstFrameRectangle;

		/// <summary>
		/// The actual sprite that is being framed.
		/// </summary>
		internal Sprite SpriteForFraming;

		/// <summary>
		/// Create a new static sprite framing object from the entire texture.
		/// </summary>
		/// <param name="texture">Texture.</param>
		public SpriteFraming(Texture2D texture) :
			this(new Rectangle(0, 0, texture.Width, texture.Height), 1, -1d)
		{
		}

		/// <summary>
		/// Create an animated sprite framing object that shifts the specified rectangle from left to right
		/// accross the underlying texture 'frameCount' times per loop at the specified milliseconds per frame.
		/// </summary>
		/// <param name="frameRectangle">Frame rectangle.</param>
		/// <param name="frameCount">Frame count.</param>
		/// <param name="millisecondsPerFrame">Milliseconds per frame.</param>
		public SpriteFraming(Rectangle frameRectangle, int frameCount, double millisecondsPerFrame)
		{
			FrameRectangle = frameRectangle;
			FrameCount = frameCount;
			MillisecondsPerFrame = millisecondsPerFrame;
			CurrentFrame = 0;
		}

		public void UpdateFrameRectangle (GameTime gameTime)
		{

			if (ShouldCheckForFrameRectangleShift()) {

				ElapsedTimeMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;

				// Time to shift the frame rectangle?
				if (ElapsedTimeMilliseconds > MillisecondsPerFrame) {

					ElapsedTimeMilliseconds -= MillisecondsPerFrame;

					// Advance the frame number, looping back to the first frame if necessary
					CurrentFrame++;
					CurrentFrame = CurrentFrame % FrameCount;

					// Looped back to the first frame?
					if (CurrentFrame == 0) {

						// Handle looping behavior unless looping forever
						if (Loops != Constants.SpriteFramingLoopInfinte) {

							LoopCounter++; // Count this loop

							if (LoopCounter >= Loops) { // Done looping?  Apply the looping finished behavior
								if (LoopingFinishedBehavior == Constants.SpriteFramingLoopingFinishedBehavior.LastFrameRectangle) {
									CurrentFrame = FrameCount - 1;
								} else if (LoopingFinishedBehavior == Constants.SpriteFramingLoopingFinishedBehavior.HideSprite) {
									SpriteForFraming.Visible = false;
								}
							}

						}

					}

					// Shift the frame rectangle to the current frame
					FrameRectangle.X = FrameRectangle.Width * CurrentFrame;

				}

			}

		}

		private bool ShouldCheckForFrameRectangleShift()
		{
			return (FrameCount > 1) && (Loops == Constants.SpriteFramingLoopInfinte || LoopCounter < Loops);
		}

	}
}

