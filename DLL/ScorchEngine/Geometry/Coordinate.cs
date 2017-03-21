namespace ScorchEngine.Geometry {
    public class Coordinate {
        public float X;
        public float Y;
        public float Z;

//////////////////////////////////////////////////////////////////////////
///OPERATORS
//////////////////////////////////////////////////////////////////////////

        public static Coordinate operator +(Coordinate a, Coordinate b) {
            return new Coordinate(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Coordinate operator -(Coordinate a, Coordinate b) {
            return new Coordinate(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Coordinate operator *(Coordinate a, float vector) {
            return new Coordinate(a.X * vector, a.Y * vector, a.Z * vector);
        }

        public static Coordinate operator /(Coordinate a, float vector) {
            return new Coordinate(a.X / vector, a.Y / vector, a.Z / vector);
        }


        public Coordinate() {
            X = Y = Z = 0;
        }

        public Coordinate(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }

        public Coordinate(Coordinate coordinate)
        {
            X = coordinate.X;
            Y = coordinate.Y;
            Z = coordinate.Z;
        }

        /// <summary>
        /// Calculates the distance between two coordinates
        /// </summary>
        /// <param name="p2"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static float Distance(Coordinate p2, Coordinate p1) {
            return 0;
        }

        override public string ToString() {
            return string.Format("({0},{1},{2})", X, Y, Z);
        }
    }
}

