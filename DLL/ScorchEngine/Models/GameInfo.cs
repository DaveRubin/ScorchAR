using System.Collections.Generic;


namespace ScorchEngine.Models
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices.ComTypes;



    public class GameInfo
    {
        public const int MAP_SIZE = 60;
        public string Name { get; set; }

        public string Id { get; set; }

        public int MaxPlayers { get; set; }

        public int Round { get; set; }

        public bool IsFull { get; set; }

        public List<PlayerInfo> Players { get; set; }

        public List<Point> PlayerPositions { get; set; }

        public GameInfo()
        {
            Players = new List<PlayerInfo>();
            PlayerPositions = new List<Point>();
            PlayerPositions.Add(new Point());
            PlayerPositions.Add(new Point());
            Round = 0;
            CreatePositionsForPlayers();
        }

        public override string ToString()
        {
            return $@"Name:{Name}
Id:{Id}
MaxPlayers:{MaxPlayers}
NumberOfPlayers:{Players.Count}
IsFull:{IsFull}";
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
            if (Players.Count == MaxPlayers)
            {
                IsFull = true;
            }
               return result;
        }


        public void CreatePositionsForPlayers()
        {
            Point firstPlayerPosition = new Point();
            Random random = new Random();
            firstPlayerPosition.X = random.Next(10, 55);
            firstPlayerPosition.Y = random.Next(10, 55);
            PlayerPositions[0] = firstPlayerPosition;
            Point secondplayerPosition = new Point();
            secondplayerPosition.X = Math.Min(Math.Max((firstPlayerPosition.X+ random.Next(15,25)) % MAP_SIZE,10),55);
            secondplayerPosition.Y = Math.Min(Math.Max((firstPlayerPosition.Y + random.Next(10, 20)) % MAP_SIZE, 10), 55);
            PlayerPositions[1] = secondplayerPosition;
            ++Round;
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
    }
}