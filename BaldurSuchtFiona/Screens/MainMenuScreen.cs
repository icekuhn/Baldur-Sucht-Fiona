using System;
using BaldurSuchtFiona.Components;
using BaldurSuchtFiona.Controls;
using Microsoft.Xna.Framework;


namespace BaldurSuchtFiona.Screens
{
	internal class MainMenuScreen : Screen
	{
		private MenuList menu;

		private ListItem ManualItem = new ListItem() { Text = "Anleitung" };
		private ListItem newGameItem = new ListItem() { Text = "Neues Spiel" };
		private ListItem optionsItem = new ListItem() { Text = "Optionen", Enabled = false };
		private ListItem exitGameItem = new ListItem() { Text = "Beenden" };

		public MainMenuScreen (ScreenComponent manager) : base(manager, new Point(400, 300)) {
			Controls.Add (new Panel (manager) { Position = new Rectangle (20, 20, 360, 40) });
			Controls.Add (new Label (manager) { Text = "Hauptmenue", Position = new Microsoft.Xna.Framework.Rectangle (40, 30, 0, 0) });
			Controls.Add (menu = new MenuList (manager) { Position = new Rectangle (20, 70, 360, 200) });

			menu.Items.Add (ManualItem);
			menu.Items.Add (newGameItem);
			menu.Items.Add (optionsItem);
			menu.Items.Add (exitGameItem);

			menu.SelectedItem = newGameItem;

			menu.OnInteract += OnInteract;
		}

		void OnInteract (ListItem item)
		{
			if (item == newGameItem) {
				Manager.Game.Simulation.NewGame ();
				Manager.CloseScreen ();
			}

			if (item == exitGameItem) {
				Manager.Game.Exit ();
				Manager.CloseScreen ();
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

