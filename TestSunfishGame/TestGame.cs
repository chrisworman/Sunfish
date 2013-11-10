using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Sunfish;

namespace TestSunfishGame
{

	public class TestGame : SunfishGame
	{

		protected override Screen GetHomeScreen ()
		{
			return new Screens.Home (this);
		}

		protected override DisplayOrientation GetDisplayOrientation ()
		{
			//return DisplayOrientation.Portrait | DisplayOrientation.PortraitDown;
			return DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
		}

	}
}
