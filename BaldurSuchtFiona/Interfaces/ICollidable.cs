using System;

namespace BaldurSuchtFiona.Interfaces
{
	internal interface ICollidable
	{
		float Mass { get; }
		bool IsFixed { get; }
	}
}

