using System;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Controls
{
	internal class Label : Control
	{
        public string Name { get; set;}
		public string Text { get; set; }

		public SpriteFont Font { get; set; }

		public Color Color { get; set; }

		public Label (ScreenComponent manager) : base(manager) {
			Font = manager.Font;
			Color = Color.White;
		}

		public override void Draw(SpriteBatch spriteBatch, Point offset) {
			if (string.IsNullOrEmpty (Text))
				return;

			if (Font == null)
				return;

			spriteBatch.DrawString(Font, Text,
				new Vector2(offset.X + Position.X, offset.Y + Position.Y), Color);
		}
	}
}

