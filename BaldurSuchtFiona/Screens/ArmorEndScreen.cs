using BaldurSuchtFiona.Controls;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Screens
{
    internal class ArmorEndScreen : Screen
    {
        public ArmorEndScreen (ScreenComponent manager) : base (manager, new Point (400, 300))
        {
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 20, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Ruestung herstellen:", Position = new Rectangle (30, 30, 0, 0) });

            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 160, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Du hast bereits die hoechste Ruestungsstufe!", Position = new Rectangle (30, 170, 0, 0) });
        }

        public override void Update (GameTime gametime)
        {
            if (!Manager.Game.Input.Handled) {
                if (Manager.Game.Input.Close) {
                    Manager.CloseScreen ();
                    Manager.Game.Input.Handled = true;
                }
                if (Manager.Game.Input.Interact) {
                    Manager.CloseScreen ();
                    Manager.Game.Input.Handled = true;
                }
            }
        }
    }
}