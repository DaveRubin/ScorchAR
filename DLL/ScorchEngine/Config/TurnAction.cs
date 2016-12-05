using ScorchEngine.Geometry;
using ScorchEngine.Items;

namespace ScorchEngine.Config
{
    public class TurnAction
    {
        public readonly Coordinate Force;
        public readonly EWeaponType Weapon;
        public readonly bool Tracer;
        public readonly Player Player;

        private TurnAction(Player executingPlayer,Coordinate force, EWeaponType weapon, bool tracer)
        {
            Player = executingPlayer;
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
            //TODO - implement proper data extraction
            return new TurnAction(player,new Coordinate(), EWeaponType.Missile, true);
        }
    }
}