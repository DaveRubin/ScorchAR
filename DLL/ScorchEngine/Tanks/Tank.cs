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
		public float AngleHorizontal;
		public float AngleVertical;
	    public float Force;

	    public bool IsReady { get; set; }
        public bool IsActive { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }

        public Tank() {
			Position = new Coordinate();
			AngleHorizontal = 0;
			AngleVertical = 0;
		    Force = 0;
			Health = 100;
            IsActive = true;

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

