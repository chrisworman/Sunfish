using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Sunfish
{
	public abstract class Screen
	{

		public SunfishGame CurrentGame { get; set; }

		public Color BackgroundColor { get; set; }

		public static Color DefaultBackgroundColor = Color.Black;

		public Views.ScreenLayers ChildViews { get; set; }

		public ContentManager ScreenContent { get; set; }

		public Views.Container TopBar { get; private set; }

		public delegate void OnTransitionedOutDelegate (Screen screenThatTransitionedOut);

		public OnTransitionedOutDelegate OnTransitionedOut;

		private Rectangle SavedScissorRectangle { get; set; }

		private RasterizerState SavedRasterizedState { get; set; }

		protected Screen (SunfishGame currentGame) :
		this(currentGame, DefaultBackgroundColor)
		{
		}

		protected Screen (SunfishGame currentGame, Color backgroundColor)
		{
			ScreenContent = currentGame.CreateContentManager ();
			ChildViews = new Views.ScreenLayers ();
			CurrentGame = currentGame;
			BackgroundColor = backgroundColor;
		}

		public abstract void PopulateScreenViews ();

		public void Update (GameTime gameTime)
		{
			ChildViews.Update (gameTime);
			ChildViews.HandleCollisions ();
		}

		public void Draw (GameTime gameTime, GraphicsDeviceManager graphics)
		{
			graphics.GraphicsDevice.Clear (BackgroundColor);

			SunfishGame.ActiveSpriteBatch.Begin ();
			ChildViews.Draw (gameTime, graphics);
			SunfishGame.ActiveSpriteBatch.End ();

//			BeginClipping (new Rectangle (0, 0, 1000, 600));
//			SunfishGame.ActiveSpriteBatch.Draw (LoadTexture ("Depth1"), new Vector2 (130, 130), null, null, null, 0f, null, null, SpriteEffects.None, 1f);
//			EndClipping ();
//			BeginClipping (new Rectangle (0, 0, 1000, 600));
//			SunfishGame.ActiveSpriteBatch.Draw (LoadTexture ("DepthPoint5"), new Vector2 (90, 90), null, null, null, 0f, null, null, SpriteEffects.None, 0.5f);
//			EndClipping ();
//			BeginClipping (new Rectangle (0, 0, 1000, 600));
//			SunfishGame.ActiveSpriteBatch.Draw (LoadTexture ("Depth0"), new Vector2 (50, 50), null, null, null, 0f, null, null, SpriteEffects.None, 0f);
//			EndClipping ();

//			BeginClipping (new Rectangle (100, 100, 100, 100));
//			SunfishGame.ActiveSpriteBatch.Draw (LoadTexture ("PopupBackground"), new Vector2(50, 50), Color.White);
//			SunfishGame.ActiveSpriteBatch.Draw (LoadTexture ("RetryButton"), new Vector2(90, 110), Color.White);
//			EndClipping ();
//
//			BeginClipping (new Rectangle (500, 100, 100, 100));
//			SunfishGame.ActiveSpriteBatch.Draw (LoadTexture ("PopupBackground"), new Vector2(400, 50), Color.White);
//			SunfishGame.ActiveSpriteBatch.Draw (LoadTexture ("RetryButton"), new Vector2(490, 110), Color.White);
//			EndClipping ();

//			RasterizerState savedRasterizedState = SunfishGame.ActiveSpriteBatch.GraphicsDevice.RasterizerState;
//			Rectangle savedScissorRectangle = SunfishGame.ActiveSpriteBatch.GraphicsDevice.ScissorRectangle;
//			RasterizerState clipState = new RasterizerState() { ScissorTestEnable = true };
//			SunfishGame.ActiveSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, clipState);
//			SunfishGame.ActiveSpriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(100,100,100,100);
//			SunfishGame.ActiveSpriteBatch.Draw (LoadTexture ("PopupBackground"), new Vector2(50, 50), Color.White);
//			SunfishGame.ActiveSpriteBatch.GraphicsDevice.ScissorRectangle = savedScissorRectangle;
//			SunfishGame.ActiveSpriteBatch.GraphicsDevice.RasterizerState = savedRasterizedState;
//			SunfishGame.ActiveSpriteBatch.Draw (LoadTexture ("PopupBackground"), new Vector2(500, 50), Color.White);
//			SunfishGame.ActiveSpriteBatch.End ();
		}

//		public void BeginClipping(Rectangle clippingRectangle)
//		{
//			//SavedScissorRectangle = SunfishGame.ActiveSpriteBatch.GraphicsDevice.ScissorRectangle;
//			//SavedRasterizedState = SunfishGame.ActiveSpriteBatch.GraphicsDevice.RasterizerState;
//			RasterizerState clippingState = new RasterizerState() { ScissorTestEnable = true };
//			SunfishGame.ActiveSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, clippingState);
//			SunfishGame.ActiveSpriteBatch.GraphicsDevice.ScissorRectangle = clippingRectangle;
//		}
//
//		public void EndClipping()
//		{
//			SunfishGame.ActiveSpriteBatch.End ();
//		}

//		public void RestoreDefaultGraphicsDeviceState()
//		{
//			if (SavedRasterizedState != null) {
//				SunfishGame.ActiveSpriteBatch.GraphicsDevice.ScissorRectangle = SavedScissorRectangle;
//				SunfishGame.ActiveSpriteBatch.GraphicsDevice.RasterizerState = SavedRasterizedState;
//			}
//		}

		public void AddChildView(Sunfish.Views.View childView)
		{
			ChildViews.Add (childView);
		}

		public Views.Popup AddPopup (Texture2D popupBackgroundTexture, Constants.ViewContainerLayout childPosition)
		{
			Views.Popup newPopup = new Sunfish.Views.Popup (popupBackgroundTexture, childPosition);
			ChildViews.Add (newPopup);
			return newPopup;
		}

		public void CreateTopBar ()
		{
			if (TopBar == null) {
				Texture2D topBarBackground = LoadTexture ("TopBarBackground");
				TopBar = new Views.Container (topBarBackground, Constants.ViewContainerLayout.FloatLeft);
				ChildViews.Add (TopBar);
			}
		}

		public Texture2D LoadTexture (string imageFileNameWithoutExtension)
		{
			if (SunfishGame.IsiPad ()) {
				return ScreenContent.Load<Texture2D> (Constants.IPadImageContentFolder + imageFileNameWithoutExtension);
			} else {
				return ScreenContent.Load<Texture2D> (Constants.IPadRetinaImageContentFolder + imageFileNameWithoutExtension);
			}
		}

		public Texture2D LoadFontTexture (string fontImageFileNameWithoutExtension)
		{
			return ScreenContent.Load<Texture2D> (Constants.FontContentFolder + fontImageFileNameWithoutExtension);
		}

		public SoundEffectInstance LoadSoundEffect (string audioFileNameWithoutExtension)
		{
			return ScreenContent.Load<SoundEffect> (Constants.AudioContentFolder + audioFileNameWithoutExtension).CreateInstance ();
		}

		public void PlaySoundEffect (string audioFileNameWithoutExtension)
		{
			if (SunfishGame.SoundEffectsOn) {
				LoadSoundEffect (audioFileNameWithoutExtension).Play ();
			}
		}

		public void TransitionIn ()
		{
			Views.Overlay overlay = Views.Overlay.CreateOpaque (this);
			ChildViews.Add (overlay);
			overlay.Disappear ();
		}

		public void TransitionOut ()
		{
			Views.Overlay overlay = Views.Overlay.CreateOpaque (this);
			overlay.OnAppear = this.HandleTransitionedOut; 
			ChildViews.Add (overlay);
			overlay.Appear ();
		}

		private void HandleTransitionedOut ()
		{
			if (OnTransitionedOut != null) {
				OnTransitionedOut (this);
			}
		}

		public void UnloadContent ()
		{
			ScreenContent.Unload ();
			ScreenContent.Dispose ();
		}

		public int PixelsWithDensity(int pixels)
		{
			return SunfishGame.PixelsWithDensity (pixels);
		}

	}
}

