using BaldurSuchtFiona.Controls;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BaldurSuchtFiona.Models;
using System.Collections.Generic;

namespace BaldurSuchtFiona.Screens
{
    internal class Armor3Screen : Screen
    {
        private List<Item> Speicher;

        public Armor3Screen (ScreenComponent manager) : base (manager, new Point (400, 300))
        {
            Speicher = new List<Item> ();

            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 20, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Ruestung herstellen:", Position = new Rectangle (30, 30, 0, 0) });

            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 90, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Druecke Enter um aus 70 Erzen die naechste Ruestung herzustellen!", Position = new Rectangle (30, 100, 0, 0) });
        }

        public new void Draw (GameTime gameTime, SpriteBatch batch)
        {
            Manager.Panel.Draw (batch, Position);
            // batch.Draw (Manager.Pixel, Position, Color.DarkBlue);
            foreach (var control in Controls)
                control.Draw (batch, Position.Location);
            batch.Draw (Manager.CollectableIcons, new Rectangle ((Manager.GraphicsDevice.Viewport.Width / 2) - 32, (Manager.GraphicsDevice.Viewport.Height / 2) - 32, 64, 64), new Rectangle (64, 128, 32, 32), Color.White);
        }

        public override void Update (GameTime gametime)
        {
            if (!Manager.Game.Input.Handled) {
                if (Manager.Game.Input.Close) {
                    Manager.CloseScreen ();
                    Manager.Game.Input.Handled = true;
                }
                if (Manager.Game.Input.Interact) {
                    int rest = 70;
                    int ores1 = 0;
                    int ores2 = 0;
                    int ores3 = 0;
                    foreach (var item in Manager.Game.Baldur.Inventory) {
                        if (!(item is Iron))
                            continue;
                        if ((item as Iron).Value == 1) { ores1 += 1; }
                        if ((item as Iron).Value == 2) { ores2 += 1; }
                        if ((item as Iron).Value == 3) { ores3 += 1; }
                    }
                    foreach (var item in Manager.Game.Baldur.Inventory) {
                        if (!(item is Iron))
                            continue;
                        if ((item as Iron).Value == 3 && rest > 0) {
                            rest -= 10;
                            Speicher.Add (item);
                        }
                    }
                    foreach (var item in Manager.Game.Baldur.Inventory) {
                        if (!(item is Iron))
                            continue;
                        if ((item as Iron).Value == 2 && rest > 0) {
                            rest -= 5;
                            Speicher.Add (item);
                        }
                    }
                    foreach (var item in Manager.Game.Baldur.Inventory) {
                        if (!(item is Iron))
                            continue;
                        if ((item as Iron).Value == 1 && rest > 0) {
                            rest -= 1;
                            Speicher.Add (item);
                        }
                    }
                    foreach (var item in Speicher) {
                        Manager.Game.Baldur.Inventory.Remove (item);
                    }

                    Manager.Game.Baldur.Inventory.Add (new Armor (Manager.Game, 3));

                    Manager.CloseScreen ();
                    Manager.Game.Input.Handled = true;
                    Manager.Game.Baldur.ChangeCharacterTexture ();
                }
            }
        }
    }
}