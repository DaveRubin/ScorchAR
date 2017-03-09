using System;

namespace ScorchEngine.Geometry
{
    public class ProjectilePath
    {
        private readonly Coordinate startPos;
        private readonly Coordinate startForce;
        private Coordinate currentForce;

        //TODO - remove after use of static environment
        private float tmpForce = 0.5f;

        public ProjectilePath(Coordinate startingPosition, Coordinate Force)
        {
            startPos = startingPosition;
            startForce = Force;
        }

        /// <summary>
        /// TODO - needs to get environment forces from Match
        /// Calculates the ticks needed to reach peak of this given path
        /// If no peek then return value is -1
        /// </summary>
        /// <returns></returns>
        public float GetTicksTillPeak()
        {
            float result = 0;
            float startYForce = startForce.Y;

            //ther will be no peak for arch when no Y force is present
            if (startYForce <= 0)
            {
                //x
                result = -1;
            }
            else
            {
                //y
                float forceReductionPerTick = startYForce / tmpForce;
                result = startYForce / forceReductionPerTick;
            }

            return result;
        }
    }
}