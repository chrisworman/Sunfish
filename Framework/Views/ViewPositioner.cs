using System;
using Microsoft.Xna.Framework;

namespace Sunfish.Views
{
	public static class ViewPositioner
	{

		public static void ScreenCenter (View viewToPosition)
		{
			float x = ((float)SunfishGame.ScreenHeight - (float)viewToPosition.Width) * 0.5f;
			float y = ((float)SunfishGame.ScreenWidth - (float)viewToPosition.Height) * 0.5f;
			viewToPosition.Position = new Vector2 (x, y);
		}

		public static void ScreenTopCenter (View viewToPosition, int topMargin = 0)
		{
			float x = ((float)SunfishGame.ScreenHeight - (float)viewToPosition.Width) * 0.5f;
			viewToPosition.Position = new Vector2 (x, (float) topMargin);
		}

		public static void ScreenBottomCenter (View viewToPosition, int bottomMargin = 0)
		{
			float x = ((float)SunfishGame.ScreenHeight - (float)viewToPosition.Width) * 0.5f;
			float y = (float) (SunfishGame.ScreenWidth - viewToPosition.Height - bottomMargin);
			viewToPosition.Position = new Vector2 (x, y);
		}

		public static void ScreenLeftCenter (View viewToPosition, int leftMargin = 0)
		{
			float y = ((float)SunfishGame.ScreenWidth - (float)viewToPosition.Height) * 0.5f;
			viewToPosition.Position = new Vector2 ((float) leftMargin, y);
		}

		public static void ScreenRightCenter (View viewToPosition, int rightMargin = 0)
		{
			float x = (float) (SunfishGame.ScreenHeight - viewToPosition.Width - rightMargin);
			float y = ((float)SunfishGame.ScreenWidth - (float)viewToPosition.Height) * 0.5f;
			viewToPosition.Position = new Vector2 (x, y);
		}


		public static void ScreenTopLeft (View viewToPosition, int leftMargin = 0, int topMargin = 0)
		{
			viewToPosition.Position = new Vector2 ((float) leftMargin, (float) topMargin);
		}

		public static void ScreenTopRight (View viewToPosition, int rightMargin = 0, int topMargin = 0)
		{
			float x = (float) (SunfishGame.ScreenHeight - viewToPosition.Width - rightMargin);
			viewToPosition.Position = new Vector2 (x, (float) topMargin);
		}

		public static void ScreenBottomLeft (View viewToPosition, int leftMargin = 0, int bottomMargin = 0)
		{
			float y = (float) (SunfishGame.ScreenWidth - viewToPosition.Height - bottomMargin);
			viewToPosition.Position = new Vector2 ((float) leftMargin, y);
		}

		public static void ScreenBottomRight (View viewToPosition, int rightMargin = 0, int bottomMargin = 0)
		{
			float x = (float) (SunfishGame.ScreenHeight - viewToPosition.Width - rightMargin);
			float y = (float) (SunfishGame.ScreenWidth - viewToPosition.Height - bottomMargin);
			viewToPosition.Position = new Vector2 (x, y);
		}

	}
}

