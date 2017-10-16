using System.Collections.Generic;

namespace ScorchEngine.Models
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices.ComTypes;

    [System.Serializable]
    public class GameInfo
    {
        public const int MAP_SIZE = 60;

        public const int MAX_NUMBER_OF_DESTRUCTABLE_OBJECTS = 10;
        public const int DESTRUCTABLE_OBJECT_MIN_DISTANCE_FROM_OTHER_OBJECTS = 5;

        public string Name { get; set; }

        public string Id { get; set; }

        public int MaxPlayers { get; set; }

        public int Rounds { get; set; }

        public bool IsFull { get; set; }

        public List<PlayerInfo> Players { get; set; }

        public List<Point>[] PlayerPositions { get; set; }

        public List<Point>[] DestructableObjectPositions { get; set; }

        public int RoundWinnerIndex { get; set; }

        public GameInfo()
        {
        }

        public GameInfo(int rounds)
        {
            RoundWinnerIndex = -1;
            Rounds = rounds;
            Players = new List<PlayerInfo>();
            PlayerPositions = new List<Point>[rounds];
            DestructableObjectPositions = new List<Point>[rounds];
            Random random = new Random();
            for (int i = 0; i < rounds; ++i)
            {
                PlayerPositions[i] = new List<Point>();
                createPositionsForPlayers(i, random);
                DestructableObjectPositions[i] = new List<Point>();
                createPositionsForDestructableObjects(i, random);
            }
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

        public bool AddPlayer(PlayerInfo playerInfo, ref int playerIndex)
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

        private void createPositionsForPlayers(int roundNumber, Random random)
        {
            Point firstPlayerPosition = new Point();

            firstPlayerPosition.X = random.Next(10, 55);
            firstPlayerPosition.Y = random.Next(10, 55);
            PlayerPositions[roundNumber].Add(firstPlayerPosition);
            Point secondplayerPosition = new Point();
            secondplayerPosition.X = Math.Min(
                Math.Max((firstPlayerPosition.X + random.Next(15, 25)) % MAP_SIZE, 10), 
                55);
            secondplayerPosition.Y = Math.Min(
                Math.Max((firstPlayerPosition.Y + random.Next(10, 20)) % MAP_SIZE, 10), 
                55);
            PlayerPositions[roundNumber].Add(secondplayerPosition);
        }

        private void createPositionsForDestructableObjects(int roundNumber, Random random)
        {
            for (int i = 0; i < MAX_NUMBER_OF_DESTRUCTABLE_OBJECTS; ++i)
            {
                Point desutractableObjectPossiblePosition = new Point();
                desutractableObjectPossiblePosition.X = random.Next(10, 55);
                desutractableObjectPossiblePosition.Y = random.Next(10, 55);
                if (desutractableObjectDoesNotColideWithPlayerTanks(roundNumber, desutractableObjectPossiblePosition)
                    && desutractableObjectDoesNotColideWithOtherDestructableObjects(
                        roundNumber, 
                        desutractableObjectPossiblePosition))
                {
                    DestructableObjectPositions[roundNumber].Add(desutractableObjectPossiblePosition);
                }
            }
        }

        private bool desutractableObjectDoesNotColideWithPlayerTanks(
            int roundNumber, 
            Point desutractableObjectPossiblePosition)
        {
            bool res = true;

            for (int i = 0; i < PlayerPositions[roundNumber].Count; ++i)
            {
                Point platerPosition = PlayerPositions[roundNumber][i];
                if (Math.Abs(platerPosition.X - desutractableObjectPossiblePosition.X) <= DESTRUCTABLE_OBJECT_MIN_DISTANCE_FROM_OTHER_OBJECTS
                    || Math.Abs(platerPosition.Y - desutractableObjectPossiblePosition.Y) <= DESTRUCTABLE_OBJECT_MIN_DISTANCE_FROM_OTHER_OBJECTS)
                {
                    res = false;
                    break;

                }
            }

            return res;
        }

        private bool desutractableObjectDoesNotColideWithOtherDestructableObjects(
            int roundNumber, 
            Point desutractableObjectPossiblePosition)
        {
            bool res = true;

            for (int i = 0; i < DestructableObjectPositions[roundNumber].Count; ++i)
            {
                Point otherDestructableObjectPosition = DestructableObjectPositions[roundNumber][i];
                if (Math.Abs(otherDestructableObjectPosition.X - desutractableObjectPossiblePosition.X) <= DESTRUCTABLE_OBJECT_MIN_DISTANCE_FROM_OTHER_OBJECTS
                    || Math.Abs(otherDestructableObjectPosition.Y - desutractableObjectPossiblePosition.Y) <= DESTRUCTABLE_OBJECT_MIN_DISTANCE_FROM_OTHER_OBJECTS)
                {
                    res = false;
                    break;

                }
            }

            return res;
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