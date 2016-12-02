using ScorchEngine.Geometry;

namespace ScorchEngine.Data
{
    public class TurnAction
    {
        public readonly Coordinate Force;
        public readonly WeaponType Weapon;
        public readonly bool Tracer;

        private TurnAction(Coordinate force, WeaponType weapon, bool tracer = false)
        {
            Force = force;
            Weapon = weapon;
            Tracer = tracer;
        }

        /// <summary>
        /// Extract Data from player and create action
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static TurnAction CreateTurnActionFromPlayer(Player player)
        {
            //TODO - implement extraction
            return new TurnAction(new Coordinate(), WeaponType.Missile, true);
        }
    }
}