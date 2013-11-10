using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Sunfish.Views.Effects
{
	public class Disappear : Effect
	{

		public Disappear (double lengthInMilliseconds):
			base(lengthInMilliseconds)
		{
		}

		public Disappear (double lengthInMilliseconds, SoundEffect sound):
			base(lengthInMilliseconds, sound)
		{
		}

		public void Reset(double lengthInMilliseconds)
		{
			ElapsedTimeInMilliseconds = 0;
			LengthInMilliseconds = lengthInMilliseconds;
		}

		protected override void UpdateEffect (GameTime gameTime, View view)
		{

			// New code: avoids creating a new color update
			float inverseAmountComplete = 1f - GetAmountComplete ();
			view.OverlayColor.R = (byte)(Byte.MaxValue * inverseAmountComplete);
			view.OverlayColor.G = (byte)(Byte.MaxValue * inverseAmountComplete);
			view.OverlayColor.B = (byte)(Byte.MaxValue * inverseAmountComplete);
			view.OverlayColor.A = (byte)(Byte.MaxValue * inverseAmountComplete);

			// Old code: creates a new color every update
			//view.OverlayColor = new Color (new Vector4 (1f - GetAmountComplete ()));

			if (IsComplete ()) {
				view.Visible = false;
			}
		}
	}
}

