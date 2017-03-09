using ScorchEngine.Geometry;
using ScorchEngine.Items;

namespace ScorchEngine.GameObjects
{
    public class Terrain
    {
        private float[,] height;
        public readonly int SizeX;
        public readonly int SizeZ;

        public Terrain(int sizeX,int sizeZ)
        {
            height = new float[sizeX,sizeZ];
            SizeX = sizeX;
            SizeZ = sizeZ;
        }

        /// <summary>
        /// Needs to get the interpolated height at a given position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public float GetHeightAt(float x, float z)
        {
            return 0;
        }

        /// <summary>
        /// Return the future collisionPoint point of the terrain object with given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Coordinate GetCollisionPoint(ProjectilePath path)
        {
            //TODO - implement when terrain object is done
            return new Coordinate();
        }

        /// <summary>
        /// Set Damage to given terrain according to hitting weapon
        /// </summary>
        /// <param name="collisionPoint"></param>
        /// <param name="weapon"></param>
        public void DoDamange(Coordinate collisionPoint, EWeaponType weapon)
        {
            //TODO - needs implementations
        }
    }
}