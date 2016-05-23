using System;

namespace BaldurSuchtFiona
{
	internal interface ICollidable
	{
		float Mass { get; }
		bool IsFixed { get; }
	}
}

