using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using ScorchEngine.Config;
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
        public event Action MatchEnded;
        public event Action<Stack<TurnAction>, Action> ActionsExecuted;

        private int m_currentTurn;
        private readonly GameConfig r_gameConfig;
        private List<Player> m_players;
        private Stack<TurnAction> m_turnActionsStack;
        private Terrain m_terrain;
        private Coordinate m_environmentForces;
        private Timer m_turnTimer;

        public bool IsFull
        {
            get { return m_players.Count == r_gameConfig.MaxPlayers; }
        }

        /// <summary>
        /// Game c'tor
        /// </summary>
        public Game(GameConfig config)
        {
            r_gameConfig = config;
            m_players = new List<Player>();
            m_environmentForces = new Coordinate(0, -1, 0);
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
            m_currentTurn = 0;
            m_turnTimer = new Timer(r_gameConfig.TurnDuration);
            m_turnTimer.Elapsed += OnTurnTimerElapsed;
            GenerateTerrain();
            PositionPlayers();
            NextTurn();
        }

        /// <summary>
        /// When turn timer is done, stop the timer and end turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTurnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            m_turnTimer.Stop();
            EndTurn();
        }

        /// <summary>
        /// When turn has ended , lock all players actions and execute all actions
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void EndTurn()
        {
            foreach (Player player in m_players)
            {
                player.TurnEnded();
            }
            ExecuteActions();
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
                EndTurn();
            }
        }

        /// <summary>
        /// Execute all stacked actions, create projectiles and calculate damage
        /// </summary>
        private void ExecuteActions()
        {
            Stack<TurnAction> actionsClone = new Stack<TurnAction>(m_turnActionsStack);
            while (m_turnActionsStack.Count > 0)
            {
                TurnAction action = m_turnActionsStack.Pop();
                //create path and get collisions with terrain
                //for each collision damage the terrain
                //find all tanks effected and damage them
            }

            // if someone is listening for action executions,
            // then let it know that actions were executed, and send NextTurn as callback to be called by the listenr
            if (ActionsExecuted != null)
            {
                ActionsExecuted(actionsClone, NextTurn);
            }
            else
            {
                //if no one listens (meaning no external use for game) skip to next Turn
                NextTurn();
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
        private void NextTurn()
        {
            Console.WriteLine("NextTurn");
            if (CheckMatchEnd())
            {
                EndMatch();
            }
            else
            {
                if (TurnStarted != null)
                {
                    TurnStarted(m_currentTurn);
                }

                // Start turn timer again
                m_turnTimer.Start();

                //enable all tanks
                foreach (Player player in m_players)
                {
                    player.TurnStarted();
                }

                m_currentTurn++;
            }
        }

        private void EndMatch()
        {
            if (MatchEnded != null)
            {
                MatchEnded();
            }
        }

        /// <summary>
        /// Check following conditions to see if match has ended
        /// - if turns reached max
        /// - if no players or just 1 player is alive
        /// </summary>
        /// <returns></returns>
        private bool CheckMatchEnd()
        {
            bool result =
                m_currentTurn == r_gameConfig.MaxTurns ||
                PlayersAlive() < 2;

            return result;
        }

        /// <summary>
        /// Return the number of players with live tanks
        /// </summary>
        /// <returns></returns>
        private int PlayersAlive()
        {
            int aliveCount = 0;

            foreach (Player player in m_players)
            {
                if (player.Tank.Alive)
                {
                    aliveCount++;
                }
            }

            return aliveCount;
        }
    }
}