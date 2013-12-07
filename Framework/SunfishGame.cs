using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Sunfish
{
	public abstract class SunfishGame : Game
	{

		#region Properties

		public static GraphicsDeviceManager Graphics;

		public static Screen ActiveScreen;

		public static SpriteBatch ActiveSpriteBatch;

		public static int ScreenWidth;

		public static int ScreenHeight;

		private Screen NextScreen;

		public static bool SoundEffectsOn = true;

		private static bool _MusicOn = false;
		public static bool MusicOn {

			get {
				return _MusicOn;
			}

			set {
				_MusicOn = value;
				if (GameSong != null) {
					if (_MusicOn && (GameSong.State == SoundState.Paused || GameSong.State == SoundState.Stopped)) {
						GameSong.Resume ();
					} else if (!_MusicOn && GameSong.State == SoundState.Playing) {
						GameSong.Pause ();
					}
				}
			}

		}

		private static SoundEffectInstance GameSong = null;

		#endregion

		public SunfishGame ()
		{
			Graphics = new GraphicsDeviceManager (this);
			Graphics.IsFullScreen = true;
			Graphics.SupportedOrientations = GetDisplayOrientation ();
			Content.RootDirectory = Constants.ContentFolder;
		}
	
		protected override void Initialize ()
		{

			base.Initialize ();

			ScreenWidth = Graphics.GraphicsDevice.Viewport.Height;
			ScreenHeight = Graphics.GraphicsDevice.Viewport.Width;

			TouchPanel.EnabledGestures = GestureType.Pinch | GestureType.FreeDrag | GestureType.Tap | GestureType.DoubleTap | GestureType.Hold;
			ActiveSpriteBatch = new SpriteBatch (Graphics.GraphicsDevice);
			SetActiveScreen (GetHomeScreen ());

		}

		public ContentManager CreateContentManager ()
		{
			ContentManager newManager = new ContentManager (Content.ServiceProvider);
			newManager.RootDirectory = Constants.ContentFolder;
			return newManager;
		}

		public void SetAndStartGameSong(string audioFileNameWithoutExtension, float volume = 1.0f)
		{
			GameSong = Content.Load<SoundEffect> (Constants.AudioContentFolder + audioFileNameWithoutExtension).CreateInstance ();
			GameSong.Volume = volume;
			GameSong.IsLooped = true;
			MusicOn = true;
		}

		#region Abstract Methods

		protected abstract Screen GetHomeScreen ();

		protected abstract DisplayOrientation GetDisplayOrientation ();

		#endregion

		public static int PixelsWithDensity(int pixels)
		{
			if (IsiPad()) {
				return pixels;
			} else {
				return pixels * 2;
			}
		}

		public static bool IsiPad()
		{
			return (ScreenWidth == 768) || (ScreenWidth == 1024);
		}

		public static bool IsiPadRetina()
		{
			return !IsiPad();
		}

		#region Screen Navigation

		public void SetActiveScreen (Screen newScreen)
		{
			NextScreen = newScreen;
			if (ActiveScreen == null) {
				PopulateAndTransitionInNextScreen ();
			} else {
				ActiveScreen.OnTransitionedOut = HandleActiveScreenTransitionedOut;
				ActiveScreen.TransitionOut ();
			}
		}

		private void HandleActiveScreenTransitionedOut (Screen screen)
		{
			ActiveScreen.UnloadContent ();
			PopulateAndTransitionInNextScreen ();
		}

		private void PopulateAndTransitionInNextScreen ()
		{
			ActiveScreen = NextScreen;
			NextScreen.PopulateScreenViews ();
			NextScreen.TransitionIn ();
		}

		#endregion

		protected override void Update (GameTime gameTime)
		{
			if (ActiveScreen != null) {
				ActiveScreen.Update (gameTime);
			}
			base.Update (gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			// Draw the current screen
			if (ActiveScreen != null) {
				ActiveScreen.Draw (gameTime, Graphics);
			} else {
				Graphics.GraphicsDevice.Clear (Color.Black);
			}
			base.Draw (gameTime);
		}



	}
}

