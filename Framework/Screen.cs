using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Sunfish
{
	public abstract class Screen
	{

		public SunfishGame CurrentGame;

		public Color BackgroundColor;

		public static Color DefaultBackgroundColor = Color.Black;

		public Views.ScreenLayers ChildViews;

		public ContentManager ScreenContent;

		public Views.Container TopBar { get; private set; }

		public delegate void OnTransitionedOutDelegate (Screen screenThatTransitionedOut);

		public OnTransitionedOutDelegate OnTransitionedOut;

		//private Rectangle SavedScissorRectangle;

		//private RasterizerState SavedRasterizedState;

		private Texture2D BackgroundTexture;

		protected Screen (SunfishGame currentGame) :
		this(currentGame, DefaultBackgroundColor)
		{
		}

		protected Screen (SunfishGame currentGame, Color backgroundColor) :
		this (currentGame, backgroundColor, null)
		{
		}

		protected Screen (SunfishGame currentGame, string backgroundTextureName) :
		this (currentGame, Color.Black, backgroundTextureName)
		{
		}

		protected Screen (SunfishGame currentGame, Color backgroundColor, string backgroundTextureName)
		{
			ScreenContent = currentGame.CreateContentManager ();
			ChildViews = new Views.ScreenLayers ();
			CurrentGame = currentGame;
			BackgroundColor = backgroundColor;
			if (!string.IsNullOrEmpty (backgroundTextureName)) {
				BackgroundTexture = LoadTexture (backgroundTextureName);
			}
		}

		public abstract void PopulateScreenViews ();

		public void Update (GameTime gameTime)
		{
			ChildViews.Update (gameTime);
			ChildViews.HandleCollisions ();
		}

		public void Draw (GameTime gameTime, GraphicsDeviceManager graphics)
		{

			// Draw the background, which is either a Color or a Texture2D
			graphics.GraphicsDevice.Clear (BackgroundColor);
			if (BackgroundTexture != null) {
				SunfishGame.ActiveSpriteBatch.Begin ();
				SunfishGame.ActiveSpriteBatch.Draw (BackgroundTexture, new Vector2(0), Color.White);
				SunfishGame.ActiveSpriteBatch.End ();
			}

			// Draw the child views of this screen
			SunfishGame.ActiveSpriteBatch.Begin ();
			ChildViews.Draw (gameTime, graphics);
			SunfishGame.ActiveSpriteBatch.End ();

		}

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

		public void PlaySoundEffect (string audioFileNameWithoutExtension, float volume = 1.0f)
		{
			if (SunfishGame.SoundEffectsOn) {
				SoundEffectInstance soundEffectToPlay = LoadSoundEffect (audioFileNameWithoutExtension);
				soundEffectToPlay.Volume = volume;
				soundEffectToPlay.Play ();
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

