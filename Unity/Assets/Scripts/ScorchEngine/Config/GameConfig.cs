namespace ScorchEngine.Config
{
    /// <summary>
    /// Game configuration file should hold all static info for current game
    /// </summary>
    public class GameConfig
    {
        public int MaxPlayers = 3;
        public int MaxTurns = 2;
        public int[] Size = {5, 5};
        public double TurnDuration = 1000; //in millisescond
    }
}