using System;
using System.Collections.Generic;
using ScorchEngine.Data;
using ScorchEngine.GameObjects;
using ScorchEngine.Geometry;

namespace ScorchEngine
{
    /// <summary>
    /// This is the main game class,entry point for the game
    /// </summary>
    public class Game
    {
        public event Action<int> TurnStarted;

        private int m_currentTurn;
        private readonly GameConfig r_gameConfig;
        private List<Player> m_players;
        private Stack<TurnAction> m_turnActionsStack;
        private Terrain m_terrain;
        private Coordinate m_gravity;

        public bool IsFull
        {
            get
            {
                return m_players.Count == r_gameConfig.MaxPlayers;
            }
        }

        /// <summary>
        /// Game c'tor
        /// </summary>
        public Game(GameConfig config)
        {
            r_gameConfig = config;
            m_players = new List<Player>();
            m_gravity = new Coordinate(0,-1,0);
            Console.WriteLine("game created");
        }

        /// <summary>
        /// Add player to game, when player count reached max m_players, start the game
        /// </summary>
        /// <param name="player"></param>
        /// <exception cref="Exception"></exception>
        public void AddPlayer(Player player)
        {
            if (m_players.Count == r_gameConfig.MaxPlayers)
            {
                throw new Exception("Trying to add player to a full game");
            }

            m_players.Add(player);

            if (m_players.Count == r_gameConfig.MaxPlayers)
            {
                StartGame();
            }
        }

        /// <summary>
        /// Commence
        /// </summary>
        private void StartGame()
        {
            GenerateTerrain();
            PositionPlayers();
            StartTurn();
        }


        /// <summary>
        /// Create terrain object using config defenition
        /// </summary>
        private void GenerateTerrain()
        {
            m_terrain = new Terrain();
        }

        private void PositionPlayers()
        {
            foreach (Player player in m_players)
            {
                player.Tank.Position = GetPosition();
                player.Ready += OnPlayerActionReady;
            }
        }

        /// <summary>
        /// When player is done, create a turnAction and add it to the actionsStack
        /// </summary>
        /// <param name="player"></param>
        private void OnPlayerActionReady(Player player)
        {
            TurnAction playersAction = TurnAction.CreateTurnActionFromPlayer(player);
            m_turnActionsStack.Push(playersAction);
            if (m_turnActionsStack.Count == m_players.Count)
            {
                ExecuteActions();
            }
        }

        /// <summary>
        /// Execute all stacked actions, create projectiles and calculate damage
        /// </summary>
        private void ExecuteActions()
        {
            while (m_turnActionsStack.Count > 0)
            {
                TurnAction action = m_turnActionsStack.Pop();
                //create path and get collisions with terrain
                //for each collision damage the terrain
                //find all tanks effected and damage them
            }
        }

        /// <summary>
        /// Return position far from other tanks
        /// TODO: needs implementation
        /// </summary>
        /// <returns></returns>
        private Coordinate GetPosition()
        {
            // X,Z selection logic
            Coordinate coordinate = new Coordinate();
            coordinate.Y = m_terrain.GetHeightAt(coordinate.X, coordinate.Z);
            return coordinate;
        }

        /// <summary>
        /// Tell all players that a turn has been started
        /// </summary>
        private void StartTurn()
        {
            if (TurnStarted != null)
            {
                TurnStarted(m_currentTurn);
            }

            //enable all tanks
            foreach (Player player in m_players)
            {
                player.StartTurn();
            }

            m_currentTurn++;
        }
    }
}