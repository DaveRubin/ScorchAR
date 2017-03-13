using ScorchEngine.GameObjects;


namespace ScorchEngine.Geometry {

    public class ProjectilePath {

        private readonly Coordinate startPos;
        private readonly Coordinate startForce;
        private Coordinate currentForce;

        //TODO - remove after use of static environment
        private readonly float r_gravity = 0.5f;

        //static constructor will get
        static ProjectilePath() {

        }

        public ProjectilePath(Coordinate startingPosition, Coordinate Force) {
            startPos = startingPosition;
            startForce = Force;
        }

        public Coordinate GetTerrainCollision(Terrain terrain) {
            bool found = false;
            Coordinate last, current;
            last = current = startPos;

            while (terrain.GetHeightAt(current.X,current.Z) > current.Y ) {
                last = current;
                current =  current + currentForce;
                currentForce.Y  += r_gravity;
            }

            //when current is under the terrain start getting find the most accurate point close to the terrain

            return last;
        }

    }
}