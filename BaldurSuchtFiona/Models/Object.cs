﻿using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona
{
	public abstract class Object : ICollidable
	{
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
			Name = name;
			Icon = icon;
			Mass = mass;
			IsFixed = isFixed;
			Position = position;
			Radius = radius;
		}
	}
}
