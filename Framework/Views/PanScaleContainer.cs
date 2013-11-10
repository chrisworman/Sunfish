using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace Sunfish.Views
{
	public class PanScaleContainer : Container
	{

		#region Constructors

		public PanScaleContainer (Texture2D backgroundTexture, Constants.ViewContainerLayout layout) :
			base(backgroundTexture, layout)
		{
		}

		public PanScaleContainer (Texture2D backgroundTexture, Constants.ViewLayer layer, Constants.ViewContainerLayout layout) :
			base(backgroundTexture, layer, layout)
		{
		}

		public PanScaleContainer (Texture2D backgroundTexture, Vector2 position, Constants.ViewContainerLayout layout) :
			base(backgroundTexture, position, layout)
		{
		}

		public PanScaleContainer (int width, int height, Constants.ViewLayer layer, Constants.ViewContainerLayout layout) :
			base(width, height, layer, layout)
		{
		}

		public PanScaleContainer (int width, int height, Constants.ViewContainerLayout layout) :
			base(width, height, layout)
		{
		}

		public PanScaleContainer (int width, int height, Vector2 position, Constants.ViewContainerLayout layout) :
			base(width, height, position, layout)
		{
		}

		public PanScaleContainer (int width, int height, Vector2 position, Constants.ViewLayer layer, Constants.ViewContainerLayout layout) :
			base(width, height, position, layer, layout)
		{
		}

		public PanScaleContainer (Texture2D backgroundTexture, Vector2 position, Constants.ViewLayer layer, Constants.ViewContainerLayout layout) :
			base(backgroundTexture, position, layer, layout)
		{
		}

		public override bool ConsumeGestures (GestureSamples gestureSamples)
		{
			return ConsumeDrags (gestureSamples) || ConsumePinches (gestureSamples);
		}

		#endregion

		#region Gestures

		private bool ConsumeDrags(GestureSamples gestureSamples)
		{
			bool dragsConsumed = false;
			List<GestureSample> freeDrags = gestureSamples.GetSamples (GestureType.FreeDrag);
			foreach (GestureSample freeDrag in freeDrags) {
				if (IsGestureWithinView (freeDrag)) {
					Position += freeDrag.Delta;
					dragsConsumed = true;
				}
			}
			return dragsConsumed;
		}

		private bool ConsumePinches(GestureSamples gestureSamples)
		{
			Vector2 initialPosition = Position;
			bool pinchesConsumed = false;
			List<GestureSample> pinches = gestureSamples.GetSamples (GestureType.Pinch);
			foreach (GestureSample pinch in pinches) {
				if (IsGestureWithinView (pinch)) {
					float scaleFactor = GetPinchScaleFactor (pinch);
					Scale *= scaleFactor;
					Position = GetPinchScaledPosition (this, pinch, scaleFactor);
					ScaleAndRepositionChildViews (pinch, scaleFactor);
					pinchesConsumed = true;
				}
			}

			return pinchesConsumed;

		}

		private void ScaleAndRepositionChildViews(GestureSample pinch, float scaleFactor)
		{
			foreach (View child in ChildViews) {
				child.Scale *= scaleFactor;
				if (child != BackgroundSprite) {
					child.PositionMultiply (new Vector2 (scaleFactor));
				}
			}
		}

		private float GetPinchScaleFactor(GestureSample pinch)
		{
			Vector2 oldPosition1 = pinch.Position - pinch.Delta;
			Vector2 oldPosition2 = pinch.Position2 - pinch.Delta2;

			float distance = Vector2.Distance (pinch.Position, pinch.Position2);
			float oldDistance = Vector2.Distance (oldPosition1, oldPosition2);

			if (oldDistance == 0 || distance == 0) {
				return 1f;
			} else {
				return  distance / oldDistance;
			}
		}

		private Vector2 GetPinchScaledPosition(View view, GestureSample pinch, float pinchScaleFactor)
		{
			Vector2 oldPosition1 = pinch.Position - pinch.Delta;
			Vector2 oldPosition2 = pinch.Position2 - pinch.Delta2;
			var newPos1 = pinch.Position - (oldPosition1 - view.Position) * pinchScaleFactor;
			var newPos2 = pinch.Position2 - (oldPosition2 - view.Position) * pinchScaleFactor;
			return Vector2.Multiply( Vector2.Add(newPos1, newPos2), 0.5f);
		}

		#endregion

	}
}

