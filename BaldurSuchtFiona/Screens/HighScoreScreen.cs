using System;
using BaldurSuchtFiona.Controls;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework;
using System.Linq;

namespace BaldurSuchtFiona.Screens
{
    //TODO Properties mit Spielernamen und Spielzeit einfügen


    internal class HighScoreScreen : Screen
    {
        public HighScoreScreen (ScreenComponent manager) : base(manager, new Point(400, 300)) {
            //TODO: Prozedur zum laden der Properties ausführen und gegebenenfalls Properties einfügen

            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 20, 360, 30) });
            Controls.Add (new Label (manager) { Text = "HighScore", Position = new Rectangle (30, 30, 0, 0) });
            var highscoreCounter = 0;
            var test = manager.Game.GetHighscores ();
            foreach(var highscore in manager.Game.GetHighscores ().OrderBy(h => h.Ticks)){
                highscoreCounter++;
                if (highscoreCounter > 5)
                    continue;
                Controls.Add (new Panel (manager) { Position = new Rectangle (20, 40 + 40 * highscoreCounter, 360, 30) });
                var timespan = new TimeSpan (highscore.Ticks);
                Controls.Add (new Label (manager) {
                    Text = highscore.PlayerName + " : " + timespan.Hours.ToString () + "h," + timespan.Minutes.ToString () + "min," + timespan.Seconds.ToString () + "sec",
                    Position = new Rectangle (30, 50 + 40 * highscoreCounter, 0, 0)});
            };
            for (var i = highscoreCounter; i < 5; i++) {
                Controls.Add (new Panel (manager) { Position = new Rectangle (20, 40 + 40 * (i+1), 360, 30) });
                Controls.Add (new Label (manager) { Text = "leerer Eintrag : 0h,0min,0sec", Position = new Rectangle (30,50 + 40 * (i+1) , 0, 0) });
            }
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

