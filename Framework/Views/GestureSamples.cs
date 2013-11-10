using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Sunfish.Views
{
	public class GestureSamples
	{

		private Dictionary<GestureType, List<GestureSample>> Samples;

		public GestureSamples ()
		{
			Samples = new Dictionary<GestureType, List<GestureSample>> ();
			GestureType[] gestureTypes = (GestureType[])Enum.GetValues (typeof(GestureType));
			for (int i=0; i < gestureTypes.Length; i++) {
				Samples.Add (gestureTypes[i], new List<GestureSample> ());
			}
		}

		public void Clear()
		{
			foreach (List<GestureSample> gestureSampleList in Samples.Values) {
				gestureSampleList.Clear ();
			}
		}

		public void AddGestureSample(GestureSample sample)
		{
			List<GestureSample> gestureSampleList;
			Samples.TryGetValue (sample.GestureType, out gestureSampleList);
			gestureSampleList.Add (sample);
		}

		public List<GestureSample> GetSamples(GestureType gestureType)
		{
			List<GestureSample> gestureSampleList;
			Samples.TryGetValue (gestureType, out gestureSampleList);
			return gestureSampleList;
		}

	}
}

