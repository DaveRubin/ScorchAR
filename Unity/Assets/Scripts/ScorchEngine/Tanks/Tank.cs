using ScorchEngine.Geometry;
using ScorchEngine.Items;

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
	    public bool Alive { get { return Health > 0; }}

	    public Coordinate Position;
		public Coordinate BaseAngle;
		public Coordinate BarrelAngle;

		public Tank() {
			Position = new Coordinate();
			BaseAngle = new Coordinate();
			BarrelAngle = new Coordinate();
		}

	    /// <summary>
	    /// Take Damage from projectile hit
	    /// </summary>
	    /// <param name="weapon"></param>
	    /// <param name="damage"></param>
	    public void Damage(EWeaponType weapon, int damage)
	    {
	        Health -= damage;
	        //Needs to calculate additional damage or specific weapons
	    }
	}

}

