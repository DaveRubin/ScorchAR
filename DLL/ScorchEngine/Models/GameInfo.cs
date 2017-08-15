using System.Collections.Generic;


namespace ScorchEngine.Models
{
    using System.Diagnostics;
    using System.Runtime.InteropServices.ComTypes;

    using SimpleJSON;

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

        public bool AddPlayer(PlayerInfo playerInfo,ref int playerIndex)
        {
            bool result = false;
            if (Players.Count < MaxPlayers)
            {
                Players.Add(playerInfo);
                playerIndex = Players.Count - 1;
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

        public bool IsGameFull()
        {
            return MaxPlayers == Players.Count;
        }

        public static List<GameInfo> ParseJsonAsList(string json)
        {
            List<GameInfo> res = new List<GameInfo>();
            SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse("{\"List\":" + json + "}");
            Debug.WriteLine(node);
            ;
            for (int i = 0; i < node["List"].Count; ++i)
            {
                GameInfo currentGame = new GameInfo();
                currentGame.Id = node["List"][i]["Id"].ToString();
                currentGame.Name = node["List"][i]["Name"].ToString();
                currentGame.MaxPlayers = node["List"][i]["MaxPlayers"].AsInt;
                currentGame.Players = new List<PlayerInfo>();
                for (int j = 0; j < node["List"][i]["Players"].Count; ++j)
                {
                    PlayerInfo player = new PlayerInfo();
                    player.Id = node["List"][i]["Players"][j]["Id"].ToString();
                    player.Name = node["List"][i]["Players"][j]["Name"].ToString();
                    currentGame.Players.Add(player);
                }
                res.Add(currentGame);
            }
            return res;
        } 
    }
}