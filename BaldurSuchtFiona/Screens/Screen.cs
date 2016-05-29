using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using BaldurSuchtFiona.Components;
using BaldurSuchtFiona.Controls;

namespace BaldurSuchtFiona.Screens
{
	public abstract class Screen
	{
		public List<Control> Controls { get; private set; }

		public Rectangle Position { get; set; }

		protected ScreenComponent Manager { get; private set; }

		public Screen (ScreenComponent manager) {
			Manager = manager;
			Controls = new List<Control> ();
		}

		public Screen(ScreenComponent manager, Point size) : this(manager) {
			Point pos = new Point (
				(manager.GraphicsDevice.Viewport.Width - size.X) / 2,
				(manager.GraphicsDevice.Viewport.Height - size.X) / 2);

			Position = new Rectangle (pos, size);
		}

		public abstract void Update (GameTime gameTime);

		public void Draw(GameTime gameTime, SpriteBatch batch)
		{
			Manager.Panel.Draw (batch, Position);
			// batch.Draw (Manager.Pixel, Position, Color.DarkBlue);
			foreach (var control in Controls)
				control.Draw (batch, Position.Location);
		}
	}
}

