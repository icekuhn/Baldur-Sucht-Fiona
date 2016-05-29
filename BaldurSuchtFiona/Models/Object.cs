using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BaldurSuchtFiona.Interfaces;

namespace BaldurSuchtFiona.Models
{
	public abstract class Object : ICollidable
	{
        public bool DrawAll { get; set; }
        public int DrawX { get; set; }
        public int DrawY { get; set; }
        public int DrawWidth { get; set; }
        public int DrawHeight { get; set; }
        internal Vector2 move = Vector2.Zero;
        public Texture2D Texture;
		public string Name { get; set; }
		public string Icon { get; set; }
		public float Mass { get; set; }
		public bool IsFixed { get; set; }
		public Vector2 Position { get; set; }
		public float Radius { get; set; }

		public Object ()
		{
			IsFixed = true;
			Mass = 1f;
			Radius = 0.25f;
			Name = "Item";
		}

        public Object (string name,string icon,float mass,bool isFixed, Vector2 position,float radius)
        {
            DrawAll = true;
            Name = name;
            Icon = icon;
            Mass = mass;
            IsFixed = isFixed;
            Position = position;
            Radius = radius;
        }

        internal void LoadTexture(Game1 game,string texture){
            Texture = game.Content.Load<Texture2D>(texture);
        }

        internal void LoadTexture(Game1 game,string texture,int drawX,int drawY,int drawWidth,int drawHeiht){
            DrawAll = false;
            DrawX = drawX;
            DrawY = drawY;
            DrawWidth = drawWidth;
            DrawHeight = drawHeiht;
            Texture = game.Content.Load<Texture2D>(texture);
        }
	}
}

