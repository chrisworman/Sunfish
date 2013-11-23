using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Sunfish.Views.Effects
{
	public class BackAndForth : Effect
	{

		private Vector2 InitialPosition;
		private Vector2 FinalPosition;
		private TranslateTo InnerTranslateToEffect;
		private double LengthPerMovementInMilliseconds;
		private bool IsMovingForth = true;

		public BackAndForth (Vector2 initialPosition, Vector2 finalPosition, double lengthPerMovementInMilliseconds, SoundEffect sound) :
		base(Constants.ViewEffectInfiniteLength, sound)
		{
			LengthPerMovementInMilliseconds = lengthPerMovementInMilliseconds;
			InitialPosition = initialPosition;
			FinalPosition = finalPosition;
			InnerTranslateToEffect = new TranslateTo (InitialPosition, FinalPosition, LengthPerMovementInMilliseconds);
		}

		public BackAndForth (Vector2 initialPosition, Vector2 finalPosition, double lengthPerMovementInMilliseconds) :
		base(Constants.ViewEffectInfiniteLength)
		{
			LengthPerMovementInMilliseconds = lengthPerMovementInMilliseconds;
			InitialPosition = initialPosition;
			FinalPosition = finalPosition;
			InnerTranslateToEffect = new TranslateTo (InitialPosition, FinalPosition, LengthPerMovementInMilliseconds);
		}

		protected override void UpdateEffect (GameTime gameTime, View view)
		{
			InnerTranslateToEffect.Update (gameTime, view);
			if (InnerTranslateToEffect.IsComplete ()) {
				if (IsMovingForth) {
					InnerTranslateToEffect = new TranslateTo (FinalPosition, InitialPosition, LengthPerMovementInMilliseconds);
				} else {
					InnerTranslateToEffect = new TranslateTo (InitialPosition, FinalPosition, LengthPerMovementInMilliseconds);
				}
				IsMovingForth = !IsMovingForth;
			}
		}

	}
}

