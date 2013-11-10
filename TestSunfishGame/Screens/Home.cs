using System;
using Sunfish;
using Sunfish.Views;
using Sunfish.Views.Partitioning;
using Sunfish.Views.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

//			Font testFont = new Font ("Helvetica40");
//			Label testLabel = new Label ("This is a \"test\" sentence! 1234567890 & *", testFont, Color.Black, new Vector2(50f, 100f));
//			testLabel.Layer = Constants.ViewLayer.Layer3;
//			ChildViews.Add (testLabel);

			//Texture2D explosion1Texture = LoadTexture ("Explosion1");
			//ParticleSystem explosions = new ParticleSystem (new Vector2 (200, 200), Constants.ViewLayer.Layer3, explosion1Texture);
			//AddChildView (explosions);

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

	}
}

