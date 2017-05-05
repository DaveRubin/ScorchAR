using System.Collections.Generic;

namespace ScorchEngine.Models
{
    public class GameInfo
    {
        public string Name { get; set; }

        public string ID { get; set; }

        public int MaxPlayers { get; set; }

        private readonly List<PlayerInfo> players = new List<PlayerInfo>(); // should be a list of player

        public override string ToString()
        {
            return $@"Name:{Name}
Id:{ID}
MaxPlayers:{MaxPlayers}
NumberOfPlayers:{players.Count}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            GameInfo gameInfo = (GameInfo)obj;
            return ID == gameInfo.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public bool AddPlayer(PlayerInfo playerInfo)
        {
            bool result = false;
            if (players.Count < MaxPlayers)
            {
                players.Add(playerInfo);
                result = true;
            }
               return result;
        }

        public void RemovePlayer(PlayerInfo playerInfo)
        {
            players.Remove(playerInfo);
        }

        public IEnumerable<PlayerInfo> GetPlayers()
        {
            return players.AsReadOnly();
        }
    }
}