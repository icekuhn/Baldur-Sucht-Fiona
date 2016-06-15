using System;
using System.Linq;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework.Graphics;
using BaldurSuchtFiona.Rendering;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Controls
{
    internal class KeycardList : VerticalListControl<ListItem>
    {
        public KeycardList (ScreenComponent manager) : base(manager){
        }

        public override void Draw(SpriteBatch spriteBatch, Point offset) {
            int x = offset.X + Position.X;
            int y = offset.Y + Position.Y;

            int zaehler = 0;
            foreach (var item in Items.Where(i => i.Visible)) {
                NineTileRenderer renderer = Manager.Icon;
                float alpha = (item.Enabled ? 1f : 0.3f);
                if (item.Equals (SelectedItem))
                    renderer = Manager.IconHovered;

                renderer.Draw (spriteBatch, new Rectangle (x, y, Position.Width, 35), alpha);

                Vector2 size = new Vector2(32, 32);
                spriteBatch.Draw ( Manager.CollectableIcons, new Vector2 (x + ((Position.Width - size.X) / 2f), y + ((35f - size.Y) / 2f)), new Rectangle(zaehler * 32, 64, 32, 32), Color.White * alpha);
                y += 40;
                zaehler += 1;
            }
        }
    }
}