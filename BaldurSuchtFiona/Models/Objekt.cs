using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BaldurSuchtFiona.Interfaces;

namespace BaldurSuchtFiona.Models
{
	public abstract class Objekt : ICollidable
	{
        public bool DrawAll { get; set; }
        public int DrawX { get; set; }
        public int DrawY { get; set; }
        public int DrawWidth { get; set; }
        public int DrawHeight { get; set; }
        internal Vector2 move = Vector2.Zero;
        //public Texture2D Texture;
        public string Texture { get; set; }
        public string TextureName { get; set; }
		public string Name { get; set; }
		public string Icon { get; set; }
		public float Mass { get; set; }
		public bool IsFixed { get; set; }
		public Vector2 Position { get; set; }
		public float Radius { get; set; }

        public Action<Game1, Area, Object, GameTime> Update { get; set; }

		public Objekt ()
        {
            DrawAll = true;
			IsFixed = true;
			Mass = 1f;
			Radius = 0.5f;
			Name = "Item";
		}

        public Objekt (string name,string icon,float mass,bool isFixed, Vector2 position,float radius)
        {
            DrawAll = true;
            Name = name;
            Icon = icon;
            Mass = mass;
            IsFixed = isFixed;
            Position = position;
            Radius = radius;
        }

      //  internal void LoadTexture(Game1 game,string texture){
       //     Texture = game.Content.Load<Texture2D>(texture);
       // }

        internal void LoadTexture(Game1 game,string texture,int drawX,int drawY,int drawWidth,int drawHeiht){
            DrawAll = false;
            DrawX = drawX;
            DrawY = drawY;
            DrawWidth = drawWidth;
            DrawHeight = drawHeiht;
            //Texture = game.Content.Load<Texture2D>(texture);
            Texture = texture;
            if (!Texture.EndsWith(".png"))
                TextureName = Texture + ".png";
            else
                TextureName = Texture;
        }
	}
}

