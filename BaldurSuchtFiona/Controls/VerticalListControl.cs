using System;
using BaldurSuchtFiona.Components;
using Microsoft.Xna.Framework;
using System.Linq;

namespace BaldurSuchtFiona.Controls
{
	internal abstract class VerticalListControl<T> : ListControl<T> where T : ListItem {

		public VerticalListControl (ScreenComponent manager) : base(manager) {
		}

		public override void Update(GameTime gameTime) {

			var availableItems = Items.Where(i => i.Enabled && i.Visible).ToList();

			if (Manager.Game.Input.Up) {
				if (SelectedItem == null) {
					SelectedItem = availableItems.LastOrDefault ();
				} else {
					int index = availableItems.IndexOf (SelectedItem);
					index = Math.Max (0, index - 1);
					SelectedItem = availableItems [index];
				}
				Manager.Game.Input.Handled = true;
			}

			if (Manager.Game.Input.Down) {
				if (SelectedItem == null) {
					SelectedItem = availableItems.FirstOrDefault ();
				} else {
					int index = availableItems.IndexOf (SelectedItem);
					index = Math.Min (availableItems.Count - 1, index + 1);
					SelectedItem = availableItems [index];
				}
				Manager.Game.Input.Handled = true;
			}

			base.Update (gameTime);
		}
	}
}

