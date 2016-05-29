using System;
using BaldurSuchtFiona.Controls;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Screens
{
    internal class InfoScreen : Screen
    {
        public InfoScreen (ScreenComponent manager) : base(manager, new Point(400, 300)) {
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 20, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Informationen zum Spiel", Position = new Rectangle (30, 30, 0, 0) });

            Controls.Add (new Label (manager) { Text = "In dem Spiel geht es darum, so schnell wir moeglich Fiona \n" +
                                                       "aus den Haenden der Roboter zu retten. Dazu muessen vor- \n" +
                                                       "allem Schluesselkarten gefunden werden, mit denen man auf \n" +
                                                       "andere Planeten beamen kann. Um im Spiel weiterzukommen \n" +
                                                       "kannst du zum Beispiel im Raumschiff aus den Pflanzen, die \n" +
                                                       "du sammelst, Heiltraenke herstellen. Auch mit anderen \n" +
                                                       "Dingen kann interagiert werden. Probiere etwas herum. Doch \n" +
                                                       "vergesse nicht: Ziel des Spiels ist es, Fiona so schnell \n" +
                                                       "wie moeglich zu retten: um so schneller du es schaffst, \n" +
                                                       "um so weiter oben landest du im Highscore! \n" +
                                                       "Viel Spass!", Position = new Rectangle (30,80, 0, 0) });

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