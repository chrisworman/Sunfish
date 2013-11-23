using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Sunfish.Views
{
	public class Popup : Container
	{

		private Vector2 HiddenPosition;

		private Vector2 ShowingPosition;

		private Overlay Overlay;

		private const double TransitionMilliseconds = 500;

		public delegate void OnHiddenDelegate (Popup popupThatIsNowHidden);

		public OnHiddenDelegate OnHidden;

		public delegate void OnShownDelegate (Popup popupThatIsNowShown);

		public OnShownDelegate OnShown;

		public Popup (Texture2D backgroundTexture, Constants.ViewContainerLayout layout) : 
		base(backgroundTexture, new Vector2(0,0), Constants.ViewLayer.Modal, layout, false)
		{

			// Create an overlay to obscure the screen when this popup is showing
			Overlay = Overlay.CreateTransparent ();
			//SunfishGame.ActiveScreen.AddChildView (Overlay);
			AddChild (Overlay);

			// Compute the position coordinates for the various states of this popup
			float CenterXPosition = (SunfishGame.ScreenHeight - Width) / 2.0f;
			HiddenPosition = new Vector2 (CenterXPosition, -SunfishGame.ScreenWidth - 1.0f);
			ShowingPosition = new Vector2 (CenterXPosition, (SunfishGame.ScreenWidth - Height) / 2.0f);
			Position = HiddenPosition;

		}

		public void Show ()
		{
			Overlay.Appear ();
			Effects.TranslateTo translateToShownPosition = new Effects.TranslateTo (Position, ShowingPosition, TransitionMilliseconds);
			translateToShownPosition.OnComplete = HandleTranslatedtoShownPosition;
			StartEffect (translateToShownPosition);
			Visible = true;
		}

		public void Hide ()
		{
			Overlay.Disappear ();
			Effects.TranslateTo translateToHiddenPosition = new Effects.TranslateTo (Position, HiddenPosition, TransitionMilliseconds);
			translateToHiddenPosition.OnComplete = HandleTranslatedtoHiddenPosition;
			StartEffect (translateToHiddenPosition);
		}
		//		public override void RemoveChildren ()
		//		{
		//			base.RemoveChildren ();
		//			Sprite background = new Sprite (SunfishGame.ActiveScreen.LoadTexture ("PopupBackground"), Constants.ViewLayer.Modal);
		//			AddChild (background, Constants.ViewContainerLayout.Absolute, 0, 0);
		//			Overlay = Overlay.CreateTransparent ();
		//			Overlay.Layer = Constants.ViewLayer.ModalOverlay;
		//			ChildViews.Add (Overlay);
		//		}
		public void HandleTranslatedtoHiddenPosition (Effects.Effect effect)
		{
			Visible = false;
			if (OnHidden != null) {
				OnHidden (this);
			}
		}

		public void HandleTranslatedtoShownPosition (Effects.Effect effect)
		{
			if (OnShown != null) {
				OnShown (this);
			}
		}
	}
}

