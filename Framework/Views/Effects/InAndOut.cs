using System;
using Microsoft.Xna.Framework;

namespace Sunfish.Views.Effects
{
	public class InAndOut : Effect
	{

		private Color PulseColor { get; set; }

		private int Pulses { get; set; }

		private int PulseNumber { get; set; }

		private double MillisecondsPerPulse { get; set; }

		private double PulseEllapsedTimeMilliseconds { get; set; }

		public InAndOut (double lengthInMillisecondsPerPulse, int pulses, Color pulseColor) :
			base (lengthInMillisecondsPerPulse * pulses)
		{
			Pulses = pulses;
			PulseColor = pulseColor;
			MillisecondsPerPulse = LengthInMilliseconds / Pulses;
			PulseNumber = 1;
			PulseEllapsedTimeMilliseconds = 0d;
		}

		protected override void UpdateEffect(GameTime gameTime, View view)
		{
			if (IsComplete ()) {
				view.OverlayColor = Color.White;
			} else {

				// Calculate how long this pulse has been running for
				PulseEllapsedTimeMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;

				// Has the current pulse ended? If so, increment to the next pulse and reset the pulse start time
				if (PulseEllapsedTimeMilliseconds >= MillisecondsPerPulse) {
					PulseNumber++;
					PulseEllapsedTimeMilliseconds = PulseEllapsedTimeMilliseconds - MillisecondsPerPulse;
				}

				// Calculate how much of the current pulse is complete (between 0 and 1)
				double pulseAmountComplete = PulseEllapsedTimeMilliseconds / MillisecondsPerPulse;

				// Interpolate using the SIN function for smooth pulsing
				float pulseColor = 1.0f - (float)Math.Sin (Math.PI * pulseAmountComplete);

				view.OverlayColor.R = (byte) (Byte.MaxValue * pulseColor);
				view.OverlayColor.G = (byte) (Byte.MaxValue * pulseColor);
				view.OverlayColor.B = (byte) (Byte.MaxValue * pulseColor);
				view.OverlayColor.A = (byte) (Byte.MaxValue * pulseColor);

			}
		}

	}
}

