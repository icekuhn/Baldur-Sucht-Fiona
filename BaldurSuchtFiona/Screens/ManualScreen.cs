using System;
using BaldurSuchtFiona.Controls;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Screens
{
    internal class ManualScreen : Screen
    {
        public ManualScreen (ScreenComponent manager) : base(manager, new Point(400, 300)) {
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 20, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Anleitung (Enter fuer mehr Informationen)", Position = new Rectangle (30, 30, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 80, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Esc = Spielmenue aufrufen // im Menue zurueck navigieren", Position = new Rectangle (30, 90, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 120, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Enter = Interagieren // im Menue vor navigieren", Position = new Rectangle (30, 130, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 160, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Pfeiltasten = Spielfigur steuern // im Menue navigieren", Position = new Rectangle (30, 170, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 200, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Leertaste = Attakieren", Position = new Rectangle (30, 210, 0, 0) });
            Controls.Add (new Panel (manager) { Position = new Rectangle (20, 240, 360, 30) });
            Controls.Add (new Label (manager) { Text = "Strg = Heiltrank trinken", Position = new Rectangle (30, 250, 0, 0) });

        }

        public override void Update(GameTime gametime) {
            if (!Manager.Game.Input.Handled) {
                if (Manager.Game.Input.Close) {
                    Manager.CloseScreen ();
                    Manager.Game.Input.Handled = true;
                }
                if (Manager.Game.Input.Interact)
                {
                    Manager.ShowScreen(new InfoScreen(Manager));
                    Manager.Game.Input.Handled = true;
                }
            }
        }
    }
}

