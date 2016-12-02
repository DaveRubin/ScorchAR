using System;
using System.Collections.Generic;
using ScortchEngine.Utils;

namespace ScortchEngine
{
	/// <summary>
	/// The tank class represents a tank object in the game
	/// </summary>
	class Tank
	{
		public TankStyle Type{ get; private set;}
		public Player Owner{ get; private set;}
		public int Health{ get; private set;}

		public Coordinate Position;
		public Coordinate BaseAngle;
		public Coordinate BarrelAngle;

		public Tank() {
			Position = new Coordinate();
			BaseAngle = new Coordinate();
			BarrelAngle = new Coordinate();
		}
	}

}

