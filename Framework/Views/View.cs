using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using Sunfish.Views.Partitioning;

namespace Sunfish.Views
{
	/// <summary>
	/// An abstract entity that can be "viewed" or "drawn" on the graphics display.
	/// </summary>
	public abstract class View
	{

		public Object Data = null;

		//private Texture2D PositionTexture = null;

		//private Texture2D DrawingPositionTexture = null;

		#region Location and Dimension Properties

		private View ParentView;

		public Vector2 _BasePosition;
		private Vector2 _TempPosition; // used so new vectors are not continually created
		public Vector2 Position { 
			get {
				if (ParentView == null) {
					//_TempPosition.X = _BasePosition.X;
					//_TempPosition.Y = _BasePosition.Y;
					_TempPosition.X = _BasePosition.X + Origin.X;
					_TempPosition.Y = _BasePosition.Y + Origin.Y;

				} else {
					//_TempPosition.X = _BasePosition.X + ParentView.Position.X - Origin.X;
					//_TempPosition.Y = _BasePosition.Y + ParentView.Position.Y - Origin.Y;
					_TempPosition.X = _BasePosition.X + ParentView.Position.X + Origin.X;
					_TempPosition.Y = _BasePosition.Y + ParentView.Position.Y + Origin.Y;
				}
				return _TempPosition;
			}
			set {
				_BasePosition = value;
				NeedsCollisionCellUpdate = true;
			}
		}

		public float RotationRadians;

		public Vector2 Origin;

		public virtual int Width { get; set; }

		public virtual int Height { get; set; }

		private bool _Visible;
		public virtual bool Visible { 
			get {
				if (ParentView == null) {
					return _Visible;
				} else {
					if (! ParentView.Visible) {
						return false;
					} else {
						return _Visible;
					}
				}
			}
			set {
				_Visible = value;
			}
		}

		public Constants.ViewLayer Layer;

		#endregion

		#region Rendering Properties

		public Color OverlayColor;

		public float Scale;

		private List<Effects.Effect> Effects;

		#endregion

		#region Physics Properties

		public Vector2 Velocity;

		public Vector2 Acceleration;

		public bool CollisionEnabled { get; private set; }

		public Partition CollisionPartition;
	
		public List<PartitionCell> CollisionCells;

		public bool IsCollisionResponder;

		private bool NeedsCollisionCellUpdate;

		public delegate void OnCollisionDelegate (View responder, View collider);

		public OnCollisionDelegate OnCollision;

		#endregion

		#region Gesture Properties

		public bool IsTapEnabled;

		public bool IsDoubleTapEnabled;

		public bool IsHoldEnabled;

		public delegate void OnTapDelegate (View viewThatWasTapped);

		private OnTapDelegate OnTap;

		public delegate void OnDoubleTapDelegate (View viewThatWasDobuleTapped);

		private OnDoubleTapDelegate OnDoubleTap;

		public delegate void OnHoldDelegate (View viewThatWasHeld);

		private OnHoldDelegate OnHold;

		#endregion

		#region Constructors

		protected View (Constants.ViewLayer layer, int width, int height, bool visible) : 
		this(new Vector2(0,0), width, height, layer, visible)
		{
		}

		protected View (Vector2 position, Constants.ViewLayer layer) : 
			this(position, 0, 0, layer)
		{
		}

		protected View (Constants.ViewLayer layer) : 
		this(new Vector2(0,0), 0, 0, layer)
		{
		}

		public View (Vector2 position, int width, int height, Constants.ViewLayer layer) :
		this(position, width, height, layer, true)
		{
		}

		public View (Vector2 position, int width, int height, Constants.ViewLayer layer, bool visible)
		{
			Position = position;
			Width = width;
			Height = height;
			Layer = layer;
			Visible = visible;
			ParentView = null; 
			OverlayColor = Color.White;
			Effects = new List<Effects.Effect> ();
			RotationRadians = 0f;
			//CenterOrigin ();
			Scale = 1.0f;
			CollisionEnabled = false;
			IsCollisionResponder = false;
			Velocity = Vector2.Zero;
			Acceleration = Vector2.Zero;
			//PositionTexture = SunfishGame.ActiveScreen.LoadTexture ("Position");
			//DrawingPositionTexture = SunfishGame.ActiveScreen.LoadTexture ("DrawingPosition");
		}

		#endregion

		#region Drawing, Location & Dimensions

		/// <summary>
		/// Set the parent view of this view, which will cause this view to use the Visible, Layer, and Position from to the parent.
		/// </summary>
		/// <param name="parentView">Parent view.</param>
		public void SetParent (View parentView)
		{
			ParentView = parentView;
		}

		public virtual void Update (GameTime gameTime)
		{
			// Update each effect and remove effects that are now complete
			Effects.ForEach (effect => effect.Update (gameTime, this));
			Effects.RemoveAll (effect => effect.IsComplete ());

			// Apply physics
			if (Visible) {
				// Collision detected and response
				if (CollisionEnabled && NeedsCollisionCellUpdate) {
					CollisionPartition.UpdatePartitionCells (this, CollisionCells);
					NeedsCollisionCellUpdate = false;
				}
				// Motion
				if (Velocity != Vector2.Zero || Acceleration != Vector2.Zero) {
					if (Acceleration != Vector2.Zero) {
						Velocity = Vector2.Add (Velocity, Acceleration);
					}
					PositionAdd(Velocity);
				}
			}
		}

		public abstract void Draw (GameTime gameTime, GraphicsDeviceManager graphics);

		public void StartEffect (Effects.Effect newEffect)
		{
			Effects.Add (newEffect);
		}

		public void ClearEffects()
		{
			Effects.Clear ();
		}

		protected void DrawTexture (Texture2D texture, Rectangle sourceRectangle)
		{
			if (Visible && texture != null) {

				float depth = 0f;

				// Sort of works; draws correctly, bounding box is correct, but rotation doesn't work
				float halfWidth = Width * 0.5f;
				float halfHeight = Height * 0.5f;
				Vector2 drawingOrigin = new Vector2 (halfWidth / Scale, halfHeight / Scale);
				Vector2 drawingPosition = new Vector2 (Position.X + halfWidth, Position.Y + halfHeight);
				SunfishGame.ActiveSpriteBatch.Draw (texture, drawingPosition, sourceRectangle, OverlayColor, RotationRadians, drawingOrigin, Scale, SpriteEffects.None, depth);

				//SunfishGame.ActiveSpriteBatch.Draw (texture, Position, sourceRectangle, Color.LightCoral, RotationRadians, Origin, Scale, SpriteEffects.None, depth);
				// INSERT DRAWING ORIGN, DRAWING POSITION AND DRAWING HERE
				//SunfishGame.ActiveSpriteBatch.Draw (DrawingPositionTexture, drawingPosition, Color.White);
				//SunfishGame.ActiveSpriteBatch.Draw (PositionTexture, Position, Color.White);

			}
		}

		protected void DrawTextureFullScreen (Texture2D texture)
		{
			if (Visible && texture != null) {
				SunfishGame.ActiveSpriteBatch.Draw (texture, new Rectangle (0, 0, SunfishGame.ScreenHeight, SunfishGame.ScreenWidth), OverlayColor);
			}
		}

		protected void DrawBoundingBox()
		{
			Texture2D pixel = SunfishGame.ActiveScreen.LoadTexture ("OverlayTransparentPixel");
			SunfishGame.ActiveSpriteBatch.Draw(pixel, GetBoundingBox(), Color.DarkGreen);
		}

		public void CenterInScreen ()
		{
			Position = new Vector2 (((float)SunfishGame.ScreenHeight - (float)Width) / 2, ((float)SunfishGame.ScreenWidth - (float)Height) / 2);
		}

		public int PixelsWithDensity(int pixels)
		{
			return SunfishGame.PixelsWithDensity (pixels);
		}

		public void CenterOrigin()
		{
			Origin = new Vector2 (Width / 2f, Height / 2f);
		}

		public Rectangle GetBoundingBox()
		{
			//return new Rectangle ((int) (Position.X), (int) (Position.Y), (int) Math.Ceiling(Width * Scale), (int) Math.Ceiling(Height * Scale));
			return new Rectangle ((int) (Position.X - Origin.X), (int) (Position.Y - Origin.Y), (int) Math.Ceiling(Width * Scale), (int) Math.Ceiling(Height * Scale));
		}

		public void PositionAdd(Vector2 vector)
		{
			_BasePosition.X += vector.X;
			_BasePosition.Y += vector.Y;
			//Origin.X += vector.X;
			//Origin.Y += vector.Y;
			NeedsCollisionCellUpdate = true;
		}

		public void PositionSubtract(Vector2 vector)
		{
			_BasePosition.X -= vector.X;
			_BasePosition.Y -= vector.Y;
			//Origin.X -= vector.X;
			//Origin.Y -= vector.Y;
			NeedsCollisionCellUpdate = true;
		}

		public void PositionMultiply(Vector2 vector)
		{
			_BasePosition.X *= vector.X;
			_BasePosition.Y *= vector.Y;
			//Origin.X *= vector.X;
			//Origin.Y *= vector.Y;
		}

		#endregion

		#region Gestures

		public void EnableTapGesture (OnTapDelegate onTap)
		{
			IsTapEnabled = true;
			OnTap = onTap;
		}

		public void DisableTapGestures()
		{
			IsTapEnabled = false;
			OnTap = null;
		}

		public void EnableDoubleTapGesture (OnDoubleTapDelegate onDoubleTap)
		{
			IsDoubleTapEnabled = true;
			OnDoubleTap = onDoubleTap;
		}

		public void DisableDoubleTapGestures()
		{
			IsDoubleTapEnabled = false;
			OnDoubleTap = null;
		}

		public void EnableHoldGesture (OnHoldDelegate onHold)
		{
			IsHoldEnabled = true;
			OnHold = onHold;
		}

		public void DisableHoldGesture ()
		{
			IsHoldEnabled = false;
			OnHold = null;
		}

		public virtual bool ConsumeGestures (GestureSamples gestureSamples)
		{

			bool gestureConsumed = false;

			if (IsTapEnabled) {
				List<GestureSample> taps = gestureSamples.GetSamples (GestureType.Tap);
				foreach (GestureSample tap in taps) {
					if (IsGestureWithinView (tap)) {
						OnTap (this);
						gestureConsumed = true;
						break;
					}
				}
			}

			if (IsDoubleTapEnabled && ! gestureConsumed) {
				List<GestureSample> doubleTaps = gestureSamples.GetSamples (GestureType.DoubleTap);
				foreach (GestureSample doubleTap in doubleTaps) {
					if (IsGestureWithinView (doubleTap)) {
						OnDoubleTap (this);
						gestureConsumed = true;
						break;
					}
				}
			}

			if (IsHoldEnabled && ! gestureConsumed) {
				List<GestureSample> holds = gestureSamples.GetSamples (GestureType.Hold);
				foreach (GestureSample hold in holds) {
					if (IsGestureWithinView (hold)) {
						OnHold (this);
						gestureConsumed = true;
						break;
					}
				}
			}

			return gestureConsumed;

		}

		public bool IsGestureWithinView (GestureSample gestureSample)
		{
			Rectangle boundingBox = GetBoundingBox ();
			if (gestureSample.GestureType == GestureType.Pinch) {
				return boundingBox.Contains (gestureSample.Position) && boundingBox.Contains (gestureSample.Position2);
			} else {
				return boundingBox.Contains (gestureSample.Position);
			}
		}

		#endregion

		#region Collision Detection

		public void EnableCollisions(Partition collisionPartition, bool isCollisionResponder)
		{
			IsCollisionResponder = isCollisionResponder;
			CollisionPartition = collisionPartition;
			CollisionCells = new List<PartitionCell> ();
			CollisionPartition.UpdatePartitionCells (this, CollisionCells);
			CollisionEnabled = true;
		}

		public void DisableCollisions()
		{
			IsCollisionResponder = false;
			CollisionEnabled = false;
			CollisionPartition = null;
			CollisionCells = null;
		}

		public void DetectAndRespondToFirstCollision()
		{
			if (Visible && IsCollisionResponder) {
				List<View> collidedWithViews = CollisionPartition.GetCollidedViews (this);
				if (OnCollision != null && collidedWithViews != null && collidedWithViews.Count > 0 ) {
					OnCollision (this, collidedWithViews.ToArray () [0]);
				}
			}
		}

		#endregion

	}
}

