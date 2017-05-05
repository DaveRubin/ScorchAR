using System.Collections.Generic;

namespace ScorchEngine.Models
{
    using System.Runtime.InteropServices.ComTypes;

    public class GameInfo
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public int MaxPlayers { get; set; }

        public List<PlayerInfo> Players { get; set; }

        public GameInfo()
        {
            Players = new List<PlayerInfo>();
        }

        public override string ToString()
        {
            return $@"Name:{Name}
Id:{Id}
MaxPlayers:{MaxPlayers}
NumberOfPlayers:{Players.Count}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            GameInfo gameInfo = (GameInfo)obj;
            return Id == gameInfo.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool AddPlayer(PlayerInfo playerInfo)
        {
            bool result = false;
            if (Players.Count < MaxPlayers)
            {
                Players.Add(playerInfo);
                result = true;
            }
               return result;
        }

        public void RemovePlayer(PlayerInfo playerInfo)
        {
            Players.Remove(playerInfo);
        }

        public IEnumerable<PlayerInfo> GetPlayers()
        {
            return Players.AsReadOnly();
        }
    }
}