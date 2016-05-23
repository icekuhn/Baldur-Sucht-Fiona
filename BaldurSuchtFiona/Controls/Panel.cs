using System;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Controls
{
	internal class Panel : Control
	{
		public Panel (ScreenComponent manager) : base(manager) {
		}

		public override void Draw(SpriteBatch spriteBatch, Point offset) {
			Manager.Border.Draw (spriteBatch, new Rectangle (
				Position.X + offset.X,
				Position.Y + offset.Y,
				Position.Width,
				Position.Height));
		}
	}
}

