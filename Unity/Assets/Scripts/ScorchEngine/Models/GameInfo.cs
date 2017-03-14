using System.Collections.Generic;

namespace ScorchEngine.Models {
    public struct GameInfo {
        public string name;
        public string ID;
        public int maxPlayers;
        public List<PlayerInfo> players;//should be a list of player
    }
}