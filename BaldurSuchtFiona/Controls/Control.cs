using System;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BaldurSuchtFiona.Controls
{
	internal abstract class Control
	{
		protected ScreenComponent Manager { get; private set; }

		public Rectangle Position { get; set; }

		public Control (ScreenComponent manager) {
			Manager = manager;
		}

		public virtual void Update(GameTime gameTime) { }

		public abstract void Draw(SpriteBatch spritebatch, Point offset);

	}
}

