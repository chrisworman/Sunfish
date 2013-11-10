using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Sunfish.Views {

	public class Switch : Sprite {

		private Texture2D OnTexture { get; set; }

		private Texture2D OffTexture { get; set; }

		public bool IsOn { get; private set; }

		public delegate void OnToggleDelegate (Switch switchThatWasToggled);

		public OnToggleDelegate OnToggle;

		public static Switch CreateOn (Texture2D onTexture, Texture2D offTexture, Constants.ViewLayer layer) {
			return new Switch (onTexture, onTexture, offTexture, true, new Vector2 (0, 0), layer);
		}

		public static Switch CreateOff (Texture2D onTexture, Texture2D offTexture, Constants.ViewLayer layer) {
			return new Switch (offTexture, onTexture, offTexture, false, new Vector2 (0, 0), layer);
		}

		protected Switch (Texture2D currentTexture, Texture2D onTexture, Texture2D offTexture, bool isOn, Vector2 texturePosition, Constants.ViewLayer layer) :
		base(currentTexture, texturePosition, layer) {
			OnTexture = onTexture;
			OffTexture = offTexture;
			IsOn = isOn;
			EnableTapGesture (HandleTap);
		}

		private void HandleTap(View thisSwitch)
		{
			Toggle (true);
		}

		public bool Toggle(bool callOnToggle = true) {
			IsOn = !IsOn;
			this.Texture = IsOn ? OnTexture : OffTexture;
			if (callOnToggle && OnToggle != null) {
				OnToggle (this);
			}
			return IsOn;
		}

	}
}

