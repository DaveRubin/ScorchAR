namespace ScorchEngine.Geometry
{
	public class Coordinate
	{
		public float X;
		public float Y;
		public float Z;

	    public Coordinate()
	    {
	        X = Y = Z = 0;
	    }

		public Coordinate(float x,float y ,float z )
		{
		    X = x;
		    Y = y;
		    Z = z;
		}

	    public static float Distance(Coordinate p2, Coordinate p1)
	    {
	        return 0;
	    }
	}
}

