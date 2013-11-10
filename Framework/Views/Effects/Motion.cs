using System;
using Microsoft.Xna.Framework;

namespace Sunfish.Views.Effects
{
	public class Motion : Effect
	{

		public Vector2 Velocity;

		public Vector2 Acceleration;

		public Motion (Vector2 initialVelocity, Vector2 acceleration, double lengthInMilliseconds) :
			base (lengthInMilliseconds)
		{
			Velocity = initialVelocity;
			Acceleration = acceleration;
		}

		protected override void UpdateEffect (GameTime gameTime, View view)
		{
			if (Acceleration != Vector2.Zero) {
				Velocity = Vector2.Add (Velocity, Acceleration);
			}
			view.PositionAdd(Velocity);
		}

	}
}

