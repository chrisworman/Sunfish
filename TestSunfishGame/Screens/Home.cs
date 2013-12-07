using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Sunfish;
using Sunfish.Views;
using Sunfish.Views.Partitioning;
using Sunfish.Views.Effects;
using Sunfish.Utilities;

namespace TestSunfishGame.Screens
{
	public class Home : Screen
	{

		private Popup TestPopup;

		private Random RandomGenerator = new Random ();

		public Home (SunfishGame currentGame) :
			base(currentGame, Color.White)
		{
		}

		public override void PopulateScreenViews ()
		{
			PopulateSpriteAnimationTestScreenViews ();
		}

		#region "Sprite Animation Test"

		private void PopulateSpriteAnimationTestScreenViews()
		{
			BackgroundColor = Color.Black;

			Texture2D buttonTexture = LoadTexture ("Grid");
			Sprite button = new Sprite (buttonTexture, new Vector2(100));
			button.EnableTapGesture (CreateRandomExplosion);
			AddChildView (button);

		}

		private void CreateRandomExplosion(View viewThatWasTapped)
		{
			Rectangle frameRectangle = new Rectangle (0, 0, 134, 134);
			SpriteFraming framing = new SpriteFraming (frameRectangle, 12, 25d);
			framing.Loops = 1;
			framing.LoopingFinishedBehavior = Constants.SpriteFramingLoopingFinishedBehavior.HideSprite;

			Texture2D explosionTexture = LoadTexture ("LockExplosion");
			Sprite explosion = new Sprite (explosionTexture, framing);
			explosion.Position = Randomization.NextVector2 (new Vector2 (200, 200), new Vector2 (900, 500));

			AddChildView (explosion);

			if (Randomization.NextBool ()) {
				PlaySoundEffect ("Explosion1");
			} else {
				PlaySoundEffect ("Explosion2");
			}

		}

		#endregion

		#region "Paradox Test"

		private void PopulateParadoxTestScreenViews()
		{
			int rows = 8;
			int cols = 18;

			PanScaleContainer panScaleContainer = new PanScaleContainer (cols * 50, rows * 50, Constants.ViewLayer.Layer1, Constants.ViewContainerLayout.FloatLeft);
			ViewPositioner.ScreenCenter (panScaleContainer);
			ChildViews.Add (panScaleContainer);

			for (int grids = 0; grids < rows * cols; grids++) {
				Sprite grid = new Sprite (LoadTexture ("Grid"));
				grid.EnableDoubleTapGesture (HandleChildTap);
				panScaleContainer.AddChild (grid);
			}

			Sprite stats = new Sprite (LoadTexture ("Stats"), Constants.ViewLayer.Layer3);
			stats.Data = "";
			stats.Scale = 0.5f;
			ViewPositioner.ScreenTopLeft (stats);
			ChildViews.Add (stats);

			Sprite timeTravelButton = new Sprite (LoadTexture ("TimeTravelButton"), Constants.ViewLayer.Layer3);
			ViewPositioner.ScreenTopCenter (timeTravelButton);
			ChildViews.Add (timeTravelButton);

			TestPopup = AddPopup (LoadTexture ("PopupBackground"), Constants.ViewContainerLayout.Absolute);

			Sprite pauseButton = new Sprite (LoadTexture ("PauseButton"), Constants.ViewLayer.Layer3);
			//pauseButton.Scale = 2.0f;
			ViewPositioner.ScreenTopRight (pauseButton, PixelsWithDensity(20), PixelsWithDensity(20));
			//pauseButton.EnableTapGesture (HandlePauseTap);
			//pauseButton.OverlayColor = Color.Yellow;
			ChildViews.Add (pauseButton);

			Sprite directionalPad = new Sprite (LoadTexture ("DirectionalPad"), Constants.ViewLayer.Layer3);
			ViewPositioner.ScreenBottomRight (directionalPad, PixelsWithDensity(20), PixelsWithDensity(20));
			ChildViews.Add (directionalPad);

			Sprite dialog = new Sprite (LoadTexture ("Dialog"), Constants.ViewLayer.Layer3);
			ViewPositioner.ScreenBottomCenter (dialog);
			ChildViews.Add (dialog);
		}

		private void HandlePauseTap(View pauseButton)
		{
			TestPopup.Show ();
		}

		public void HandleChildTap(View viewThatWasTapped)
		{
			viewThatWasTapped.StartEffect(new Sunfish.Views.Effects.Pulsate(800d, 50, GetRandomColor()));
			viewThatWasTapped.StartEffect (new Sunfish.Views.Effects.Rotate (0f, (float)Math.PI * 2, 2000d));
		}

		private Color GetRandomColor()
		{
			return new Color ((float)RandomGenerator.NextDouble (), (float)RandomGenerator.NextDouble (), (float)RandomGenerator.NextDouble ());
		}

		#endregion

	}
}

