using System;
using BaldurSuchtFiona.Components;
using BaldurSuchtFiona.Controls;
using Microsoft.Xna.Framework;


namespace BaldurSuchtFiona.Screens
{
    internal class BedScreen : Screen
    {
        private MenuList menu;

        private ListItem SaveItem = new ListItem () { Text = "Diesen Spielstand Speichern" };
        private ListItem LoadItem = new ListItem () { Text = "Letzten Spielstand Laden" };

        public BedScreen (ScreenComponent manager) : base (manager, new Point (400, 300))
        {
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 20, 360, 40) });
            Controls.Add (new Label (manager) { Text = "Speichern / Laden:", Position = new Rectangle (30, 30, 0, 0) });
            Controls.Add (menu = new MenuList (manager) { Position = new Rectangle (20, 70, 360, 200) });

            menu.Items.Add (SaveItem);
            menu.Items.Add (LoadItem);


            menu.SelectedItem = SaveItem;

            menu.OnInteract += OnInteract;
        }

        void OnInteract (ListItem item)
        {
            if (item == SaveItem) {
                Manager.CloseScreen ();
                Manager.Game.SaveGame ();
            }

            if (item == LoadItem) {
                Manager.CloseScreen ();
                Manager.Game.LoadGame ();
            }
        }

        public override void Update (GameTime gametime)
        {
            if (!Manager.Game.Input.Handled) {
                if (Manager.Game.Input.Close) {
                    Manager.CloseScreen ();
                    Manager.Game.Input.Handled = true;
                }
            }
        }
    }
}