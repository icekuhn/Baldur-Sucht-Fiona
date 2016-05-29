using System;
using BaldurSuchtFiona.Components;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Controls
{
	internal abstract class ListControl<T> : Control where T : ListItem {

		private T selectedItem = null;

		public List<T> Items { get; private set; }

		public T SelectedItem {
			get { return selectedItem; }
			set {
				if (selectedItem != value) {
					selectedItem = value;
					if (OnSelectionChanged != null)
						OnSelectionChanged (selectedItem);
				}
			}
		}

		public ListControl (ScreenComponent manager) : base(manager) {
			Items = new List<T> ();
			SelectedItem = default(T);
		}

		public override void Update(GameTime gameTime) {
    			if (Manager.Game.Input.Interact) {
    				if (SelectedItem != null && OnInteract != null)
    					OnInteract (SelectedItem);

    				Manager.Game.Input.Handled = true;
    			}

			base.Update (gameTime);
		}

		public event ItemDelegate<T> OnSelectionChanged;

		public event ItemDelegate<T> OnInteract;

		public delegate void ItemDelegate<V>(V item);
	}

	internal class ListItem {
		public object Tag { get; set; }

		public bool Enabled { get; set; }

		public bool Visible { get; set; }

		public string Text { get; set; }

		public ListItem() {
			Enabled = true;
			Visible = true;
		}

		public override string ToString () {
			return Text ?? string.Empty;
		}
	}
}

