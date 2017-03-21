using ScorchEngine.Geometry;
using UnityEngine;

namespace Extensions {
    public class Vector3Extension {
        public static Vector3 FromCoordinate(Coordinate coordinate) {
            return new Vector3(coordinate.X,coordinate.Y,coordinate.Z);
        }
    }
}