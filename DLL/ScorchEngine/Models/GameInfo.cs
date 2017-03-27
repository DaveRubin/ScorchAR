using System.Collections.Generic;

namespace ScorchEngine.Models {
    public class GameInfo {
        public string Name { get; set; }
        public string ID { get; set; }
        public int MaxPlayers { get; set; }
        public List<PlayerInfo> Players { get; } = new List<PlayerInfo>();//should be a list of player
    }
}