using BaldurSuchtFiona.Controls;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework;
using System.Linq;

namespace BaldurSuchtFiona.Screens
{
    internal class WinningScreen : Screen
    {
        private ScreenComponent _manager;
        public WinningScreen (ScreenComponent manager) : base (manager, new Point (400, 300))
        {
            _manager = manager;
            Controls.Add (new Panel (_manager) { Position = new Rectangle (20, 20, 360, 30) });
            Controls.Add (new Label (_manager) { Text = "Bitte gib deinen Namen (max 5 Zeichen) fuer den HighScore an:", Position = new Rectangle (30, 30, 0, 0) });

            Controls.Add (new Panel (_manager) { Position = new Rectangle (20, 160, 360, 30) });
            Controls.Add (new Label (_manager) { Name= "ReplaceableLabel",Text = "Name: " + _manager.Game.WinnerName, Position = new Rectangle (30, 170, 0, 0) });
        }

        public override void Update (GameTime gametime)
        {                    
            var labelToRemove = Controls.OfType<Label> ().FirstOrDefault (l => l.Name == "ReplaceableLabel");
            Controls.Remove (labelToRemove);
            Controls.Add (new Label (_manager) { Name= "ReplaceableLabel",Text = "Name: " + _manager.Game.WinnerName, Position = new Rectangle (30, 170, 0, 0) });
            if (!Manager.Game.Input.Handled) {
                if (Manager.Game.Input.Interact) {
                    Manager.CloseScreen ();
                    Manager.Game.SaveHighScore ();
                }
            }
        }
    }
}