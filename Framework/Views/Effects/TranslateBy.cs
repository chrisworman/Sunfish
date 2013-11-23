using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Sunfish.Views.Effects
{
	public class TranslateBy : Effect
	{

		private bool IsFirstUpdate = true;
		private Vector2 InitialPosition;
		private Vector2 FinalPosition;
		private Vector2 DeltaPosition;
		private Vector2 Delta;

		public TranslateBy (Vector2 delta, double lengthInMilliseconds, SoundEffect sound):
		base(lengthInMilliseconds, sound)
		{
			Delta = delta;
		}

		public TranslateBy (Vector2 delta, double lengthInMilliseconds):
		base(lengthInMilliseconds)
		{
			Delta = delta;
		}

		protected override void UpdateEffect (GameTime gameTime, View view)
		{

			// First update? If so, capture the base position for accurate translation
			if (IsFirstUpdate) {
				IsFirstUpdate = false;
				InitialPosition = view._BasePosition;
				FinalPosition = Vector2.Add (InitialPosition, Delta);
				DeltaPosition = Vector2.Subtract (FinalPosition, InitialPosition); // Compute this once now for better performance in UpdateEffect
			}

			float amountComplete;
			if (IsComplete (out amountComplete)) {
				view.Position = FinalPosition;
			} else {
				Vector2 offsetPosition = Vector2.Multiply (DeltaPosition, new Vector2 (amountComplete));
				view.Position = Vector2.Add (InitialPosition, offsetPosition);
			}
		}
	}
}

