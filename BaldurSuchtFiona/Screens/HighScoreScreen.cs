using System;
using BaldurSuchtFiona.Controls;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Screens
{
    //TODO Properties mit Spielernamen und Spielzeit einfügen


    internal class HighScoreScreen : Screen
    {
        public HighScoreScreen (ScreenComponent manager) : base(manager, new Point(400, 300)) {
            //TODO: Prozedur zum laden der Properties ausführen und gegebenenfalls Properties einfügen

            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 20, 360, 30) });
            Controls.Add (new Label (manager) { Text = "HighScore", Position = new Rectangle (30, 30, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 80, 360, 30) });
            Controls.Add (new Label (manager) { Text = "leerer Eintrag : 0h,0min,0sec", Position = new Rectangle (30, 90, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 120, 360, 30) });
            Controls.Add (new Label (manager) { Text = "leerer Eintrag : 0h,0min,0sec", Position = new Rectangle (30, 130, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 160, 360, 30) });
            Controls.Add (new Label (manager) { Text = "leerer Eintrag : 0h,0min,0sec", Position = new Rectangle (30, 170, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 200, 360, 30) });
            Controls.Add (new Label (manager) { Text = "leerer Eintrag : 0h,0min,0sec", Position = new Rectangle (30, 210, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 240, 360, 30) });
            Controls.Add (new Label (manager) { Text = "leerer Eintrag : 0h,0min,0sec", Position = new Rectangle (30, 250, 0, 0) });
        }

        public override void Update(GameTime gametime) {
            if (!Manager.Game.Input.Handled) {
                if (Manager.Game.Input.Close) {
                    Manager.CloseScreen ();
                    Manager.Game.Input.Handled = true;
                }
            }
        }
    }
}

