using System;

namespace BaldurSuchtFiona
{
	public class Baldur : Player
	{
        public int Flowers { get; set; }

        public int Ores { get; set; }

        public int Potions { get; set; }

        public int Weapons { get; set; }

        public int Keycards { get; set; }

        public Baldur () : base()
		{
			InitializeData ();
            Flowers = 0;
            Ores = 0;
            Potions = 0;
            Weapons = 1;
            Keycards = 1;
		}

		public void InitializeData (){
			Name = "Baldur";
		}
	}
}

