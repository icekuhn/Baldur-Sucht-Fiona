using System;
using BaldurSuchtFiona.Controls;
using BaldurSuchtFiona.Components;
using BaldurSuchtFiona.Models;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BaldurSuchtFiona.Screens
{
    internal class PotionScreen : Screen
    {
        private int Flowers1;
        private int Flowers2;
        private int Flowers3;
        private int Potions;

        private List<Item> Speicher;

        public PotionScreen (ScreenComponent manager) : base (manager, new Point (400, 300))
        {
            Speicher = new List<Item>();

            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 20, 360, 30) });
            Flowers1 = 0;
            Flowers2 = 0;
            Flowers3 = 0;
            foreach (var item in Manager.Game.Baldur.Inventory) {
                if (!(item is Flower))
                    continue;
                if ((item as Flower).Value == 1) { Flowers1 += 1;}
                if ((item as Flower).Value == 2) { Flowers2 += 1;}
                if ((item as Flower).Value == 3) { Flowers3 += 1;}
            }
            Potions = Flowers1 / 5 + Flowers2 + Flowers3 * 2;

            Controls.Add (new Label (manager) { Text = "Enter druecken um Heiltraenke aus deinen Pflanzen herzustellen:", Position = new Rectangle (30, 30, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 80, 360, 30) });

            Controls.Add (new Label (manager) { Text = String.Format (": {0}", Flowers1), Position = new Rectangle (70, 90, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 120, 360, 30) });
            Controls.Add (new Label (manager) { Text = String.Format (": {0}", Flowers2), Position = new Rectangle (70, 130, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 160, 360, 30) });
            Controls.Add (new Label (manager) { Text = String.Format (": {0}", Flowers3), Position = new Rectangle (70, 170, 0, 0) });

            Controls.Add (new Label (manager) { Text = "_________________________________________________________", Position = new Rectangle (30, 210, 0, 0) });

            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 240, 360, 30) });
            Controls.Add (new Label (manager) { Text = String.Format (": {0}", Potions), Position = new Rectangle (70, 250, 0, 0) });

        }

        public override void Update (GameTime gametime)
        {
            if (!Manager.Game.Input.Handled) {
                if (Manager.Game.Input.Close) {
                    Manager.CloseScreen ();
                    Manager.Game.Input.Handled = true;
                }
                if (Manager.Game.Input.Interact) {
                    int zaehler = 0;
                    foreach (var item in Manager.Game.Baldur.Inventory) {
                        if (!(item is Flower))
                            continue;
                        if ((item as Flower).Value == 1) {
                            if ((Flowers1 - zaehler) % 5 != 0) {
                                zaehler += 1;
                                continue;
                            }
                            Speicher.Add (item);
                        }
                        if ((item as Flower).Value == 2) { Speicher.Add (item); }

                        if ((item as Flower).Value == 3) { Speicher.Add (item); }
                    }
                    foreach (var item in Speicher) {
                        Manager.Game.Baldur.Inventory.Remove (item);
                    }
                    for (int i = Potions; i > 0; i--) {
                        Manager.Game.Baldur.Inventory.Add (new Healpot());
                    }
                    Manager.CloseScreen ();
                    Manager.Game.Input.Handled = true;
                }
            }
        }
    }
}