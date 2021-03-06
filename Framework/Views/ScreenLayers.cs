using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Sunfish.Views
{
	public class ScreenLayers
	{

		private List<View>[] Layers { get; set; }

		public int Count { get; private set; }

		private GestureSamples Gestures;

		public ScreenLayers ()
		{
			Gestures = new GestureSamples ();
			Clear ();
		}

		public void Add (View view)
		{
			Layers [(int)view.Layer].Add (view);
			Count++;
		}

		public void Clear ()
		{
			Layers = new List<View>[(int)Constants.ViewLayer.Modal + 1];
			for (int layer = (int) Constants.ViewLayer.Layer1; layer < Layers.Length; layer++) {
				Layers [layer] = new List<View> ();
			}
			Count = 0;
		}

		public void RemoveView(View viewToRemove)
		{
			List<View> viewLayer = Layers [(int)viewToRemove.Layer];
			viewLayer.Remove (viewToRemove);
		}

		public void Update (GameTime gameTime)
		{
			HandleGestures ();
			UpdateViews (gameTime);
		}

		internal void HandleGestures ()
		{

			if (TouchPanel.IsGestureAvailable) { 

				while (TouchPanel.IsGestureAvailable) {
					Gestures.AddGestureSample (TouchPanel.ReadGesture ());
				}

				ConsumeGestures (Gestures);
				Gestures.Clear ();

			}

		}

		internal bool ConsumeGestures (GestureSamples gestureSamples)
		{
			// Iterate through the view layers, starting at the top
			for (int layer = Layers.Length-1; layer >= 0; layer--) {
				List<View> viewLayer = Layers [layer];
				foreach (View view in viewLayer) {
					if (view.Visible && view.ConsumeGestures (gestureSamples)) {
						return true;
					}
				}
			}
			return false;
		}

		internal void UpdateViews (GameTime gameTime)
		{
			for (int layer = 0; layer < Layers.Length; layer++) {
				List<View> viewLayer = Layers [layer];
				if (viewLayer != null) {
					foreach (View view in viewLayer) {
						if (view != null) {
							view.Update (gameTime);
						}
					}
				}
			}
		}

		public void HandleCollisions ()
		{
			for (int layer = 0; layer < Layers.Length; layer++) {
				List<View> viewLayer = Layers [layer];
				foreach (View view in viewLayer) {
					if (view.IsCollisionResponder) {
						view.DetectAndRespondToFirstCollision ();
					}
				}
			}
		}

		public void Draw (GameTime gameTime, GraphicsDeviceManager graphics)
		{
			// Iterate through the view layers, starting at the bottom
			for (int layer = 0; layer < Layers.Length; layer++) {
				List<View> viewLayer = Layers [layer];
				foreach (View view in viewLayer) {
					view.Draw (gameTime, graphics);
				}
			}
		}

		public void SetVisibility (bool visible)
		{
			for (int layer = 0; layer < Layers.Length; layer++) {
				List<View> viewLayer = Layers [layer];
				foreach (View view in viewLayer) {
					view.Visible = visible;
				}
			}
		}

		public List<View> GetLayer (Constants.ViewLayer layer)
		{
			return Layers [(int)layer];
		}

		public void RemoveNonVisibleViews ()
		{
			for (int layer = 0; layer < Layers.Length; layer++) {
				List<View> viewLayer = Layers [layer];
				Count -= viewLayer.RemoveAll (view => ! view.Visible);
			}
		}
	}
}

