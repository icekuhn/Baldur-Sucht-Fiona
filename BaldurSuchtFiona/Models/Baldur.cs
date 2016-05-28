using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona
{
	public class Baldur : Player
    {
        public Baldur () : base()
        {
            InitializeData ();
        }

        public Baldur (Vector2 position) : base()
        {
            Position = position;
            InitializeData ();
        }

		public void InitializeData (){
			Name = "Baldur";
		}
	}
}

