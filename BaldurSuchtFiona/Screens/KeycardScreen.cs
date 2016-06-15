using System;
using BaldurSuchtFiona.Components;
using BaldurSuchtFiona.Controls;
using Microsoft.Xna.Framework;


namespace BaldurSuchtFiona.Screens
{
    internal class KeycardScreen : Screen
    {
        private KeycardList menu;

        private ListItem Keycard1 = new ListItem() { Text = "Keycard1" };
        private ListItem Keycard2 = new ListItem() { Text = "Keycard2" };
        private ListItem Keycard3 = new ListItem() { Text = "Keycard3" };
        private ListItem Keycard4 = new ListItem() { Text = "Keycard4" };
        private ListItem Keycard5 = new ListItem() { Text = "Keycard5" };
        private ListItem Keycard6 = new ListItem() { Text = "Keycard6" };

        public KeycardScreen (ScreenComponent manager) : base(manager, new Point(400, 300)) {
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 20, 360, 25) });
            Controls.Add (new Label (manager) { Text = "Keycards", Position = new Rectangle (30, 25, 0, 0) });
            Controls.Add (menu = new KeycardList (manager) { Position = new Rectangle (20, 50, 360, 230) });

            if (manager.Game.Baldur.KeycardCounter >= 1)
                menu.Items.Add (Keycard1);
            if (manager.Game.Baldur.KeycardCounter >= 2)
                menu.Items.Add (Keycard2);
            if (manager.Game.Baldur.KeycardCounter >= 3)
                menu.Items.Add (Keycard3);
            if (manager.Game.Baldur.KeycardCounter >= 4)
                menu.Items.Add (Keycard4);
            if (manager.Game.Baldur.KeycardCounter >= 5)
                menu.Items.Add (Keycard5);
            if (manager.Game.Baldur.KeycardCounter == 6)
                menu.Items.Add (Keycard6);

            menu.SelectedItem = Keycard1;

            menu.OnInteract += OnInteract;
        }

        void OnInteract (ListItem item)
        {
            if (item == Keycard1) {
                Manager.Game.LoadLevel(0);
                Manager.CloseScreen ();
            }

            if (item == Keycard2) {
                Manager.Game.LoadLevel(1);
                Manager.CloseScreen ();
            }

            if (item == Keycard3) {
                Manager.Game.LoadLevel(2);
                Manager.CloseScreen ();
            }

            if (item == Keycard4) {
                Manager.Game.LoadLevel(3);
                Manager.CloseScreen ();
            }

            if (item == Keycard5) {
                Manager.Game.LoadLevel(4);
                Manager.CloseScreen ();
            }

            if (item == Keycard5) {
                Manager.Game.LoadLevel (5);
                Manager.CloseScreen ();
            }
        }

        public override void Update(GameTime gametime) {
            if (!Manager.Game.Input.Handled) {
                if (Manager.Game.Input.Close)
                {
                    Manager.CloseScreen();
                    Manager.Game.Input.Handled = true;
                }
            }
        }
    }
}

