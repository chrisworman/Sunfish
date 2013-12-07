using System;
using Microsoft.Xna.Framework;

namespace Sunfish.Utilities
{
	public static class Randomization
	{

		public static Random RandomGenerator = new Random ();

		public static bool NextBool()
		{
			return NextDouble() > 0.5d;
		}

		public static double NextDouble()
		{
			return RandomGenerator.NextDouble ();
		}

		public static float NextFloat()
		{
			return (float) NextDouble ();
		}

		public static float NextFloat(float min, float max)
		{
			float magnitude = Math.Abs (max - min);
			float randomMagnitude = (float) NextDouble() * magnitude;
			return min + randomMagnitude;
		}

		public static Vector2 NextVector2(Vector2 min, Vector2 max)
		{
			return new Vector2 (NextFloat (min.X, max.X), NextFloat (min.Y, max.Y));
		}

		public static float NextFloatSign()
		{
			return NextBool () ? -1.0f : 1f;
		}

	}
}