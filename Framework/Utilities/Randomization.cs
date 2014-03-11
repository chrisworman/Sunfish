using System;
using Microsoft.Xna.Framework;

namespace Sunfish.Utilities
{
	public static class Randomization
	{

		public static Random RandomGenerator = new Random ();

		#region "Bool"

		public static bool NextBool()
		{
			return NextDouble() >= 0.5d;
		}

		#endregion

		#region "Int"

		public static int NextInt(int minValue, int maxValue)
		{
			return RandomGenerator.Next(minValue, maxValue);
		}

		#endregion

		#region "Float"

		public static float NextFloat()
		{
			return (float) NextDouble ();
		}

		public static float NextFloat(float min, float max)
		{
			return (float)NextDouble ((double)min, (double)max);
		}

		public static float NextFloatSign()
		{
			return NextBool () ? -1.0f : 1f;
		}

		#endregion

		#region "Double"

		public static double NextDouble()
		{
			return RandomGenerator.NextDouble ();
		}

		public static double NextDouble(double min, double max)
		{
			double magnitude = Math.Abs (max - min);
			double randomMagnitude = NextDouble() * magnitude;
			return min + randomMagnitude;
		}

		#endregion

		#region "Vector2"

		public static Vector2 NextVector2(Vector2 min, Vector2 max)
		{
			return new Vector2 (NextFloat (min.X, max.X), NextFloat (min.Y, max.Y));
		}

		public static Vector2 NextVector2(Vector2 max)
		{
			return new Vector2 (NextFloat (0f, max.X), NextFloat (0f, max.Y));
		}

		public static Vector2 NextVector2Sign()
		{
			return new Vector2 (NextFloatSign (), NextFloatSign ());
		}

		public static Vector2 NextVector2Perturbed(Vector2 vectorToPerturb, Vector2 maxPerturbation)
		{
			Vector2 pertubation = NextVector2 (maxPerturbation);
			pertubation = Vector2.Multiply (pertubation, NextVector2Sign ());

			return Vector2.Add (vectorToPerturb, pertubation);

		}

		#endregion

	}
}