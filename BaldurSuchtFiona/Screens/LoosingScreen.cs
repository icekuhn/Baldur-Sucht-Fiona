using BaldurSuchtFiona.Controls;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Screens
{
    internal class LoosingScreen : Screen
    {
        public LoosingScreen (ScreenComponent manager) : base (manager, new Point (400, 300))
        {
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 20, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Du hast leider zu lange gebraucht, um in den HighScore zu kommen:", Position = new Rectangle (30, 30, 0, 0) });

            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 160, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Schade!", Position = new Rectangle (30, 170, 0, 0) });
        }

        public override void Update (GameTime gametime)
        {
            if (!Manager.Game.Input.Handled) {
                if (Manager.Game.Input.Close) {
                    Manager.Game.NewGame ();
                    Manager.CloseScreen ();
                }
                if (Manager.Game.Input.Interact) {
                    Manager.Game.NewGame ();
                    Manager.CloseScreen ();
                }
            }
        }
    }
}