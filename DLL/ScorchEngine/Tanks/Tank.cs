using ScorchEngine.Geometry;

namespace ScorchEngine
{
	/// <summary>
	/// The tank class represents a tank object in the game
	/// </summary>
	public class Tank
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

