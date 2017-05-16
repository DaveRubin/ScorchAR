using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Timers;
using ScorchEngine.Config;
using ScorchEngine.GameObjects;
using ScorchEngine.Geometry;
using ScorchEngine.Items;
using ScorchEngine.Server;

namespace ScorchEngine
{
    /// <summary>
    /// This is the main game class,entry point for the game
    /// </summary>
    public class Game
    {
        public event Action<int> TurnStarted;
        public event Action MatchEnded;
        public event Action<string> debugLog;
        public event Action<Stack<TurnAction>, Action> ActionsExecuted;
        public event Action<List<PlayerState>> OnStateUpdate;

        private int m_currentTurn;
        private readonly GameConfig r_gameConfig;
        private List<Player> m_players;
        private Stack<TurnAction> m_turnActionsStack;
        private Terrain m_terrain;
        private Coordinate m_environmentForces;
        private Timer m_turnTimer;

        private Coordinate currentTankPosition = new Coordinate();// ONLY FOR DEBUG !

        /// <summary>
        /// TODO - wrap dictionaries inside weapons static class
        /// </summary>
        private Dictionary<EWeaponType, float> AreaOfEffectMap = new Dictionary<EWeaponType, float>()
        {
            {EWeaponType.Regular, 1},
            {EWeaponType.BabyMissile, 1},
            {EWeaponType.Missile, 1},
            {EWeaponType.BabyNuke, 1},
            {EWeaponType.Nuke, 1},
            {EWeaponType.Mirv, 1},
            {EWeaponType.SuperMirv, 1}
        };

        private Dictionary<EWeaponType, int> DamageMap = new Dictionary<EWeaponType, int>()
        {
            {EWeaponType.Regular, 1},
            {EWeaponType.BabyMissile, 1},
            {EWeaponType.Missile, 1},
            {EWeaponType.BabyNuke, 1},
            {EWeaponType.Nuke, 1},
            {EWeaponType.Mirv, 1},
            {EWeaponType.SuperMirv, 1}
        };

        public bool IsFull
        {
            get { return m_players.Count == r_gameConfig.MaxPlayers; }
        }

        public Terrain Terrain
        {
            get { return m_terrain; }
        }

        public Coordinate GetEnvironmetForces()
        {
            return m_environmentForces;
        }

        /// <summary>
        /// Game c'tor
        /// </summary>
        public Game(GameConfig config)
        {
            r_gameConfig = config;
            m_players = new List<Player>();
            m_environmentForces = new Coordinate(0, -1, 0);
            GenerateTerrain();
        }

        public void Poll(string gameID,PlayerState myState)
        {
             ServerWrapper.GetState(gameID,myState,OnPollResult);
        }

        private void OnPollResult(List<PlayerState> list)
        {

            ProcessPoll(list);
            OnStateUpdate?.Invoke(list);
        }

        private void ProcessPoll(List<PlayerState> updatesList)
        {
            foreach (PlayerState state in updatesList)
            {
                m_players[state.Id].Process(state);
            }
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
            Log("playerAdd" + player.Name);

            if (m_players.Count == r_gameConfig.MaxPlayers)
            {
                StartGame();
            }
        }

        private void Log(string message)
        {
            debugLog?.Invoke(message);
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
            m_terrain = new Terrain(r_gameConfig.Size[0],r_gameConfig.Size[1]);
        }

        private void PositionPlayers()
        {
            foreach (Player player in m_players)
            {
                Log((player == null).ToString());
                player.ControlledTank.Position = GetPosition();
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
                ProjectilePath path = new ProjectilePath(action.Player.ControlledTank.Position,action.Force);
                Coordinate collisionPoint = m_terrain.GetCollisionPoint(path);
                //for each collision damage the terrain
                m_terrain.DoDamange(collisionPoint, action.Weapon);
                //find all tanks effected and damage them
                TestTanksCollisionAt(collisionPoint,action.Weapon);
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
        /// Iterate through all tanks and check if in area of effect for the given weapon
        /// </summary>
        /// <param name="collisionPoint"></param>
        /// <param name="weapon"></param>
        private void TestTanksCollisionAt(Coordinate collisionPoint, EWeaponType weapon)
        {
            float aoe = AreaOfEffectMap[weapon];
            foreach (Player player in m_players)
            {
                Coordinate tankPos = player.ControlledTank.Position;
                float distance = Coordinate.Distance(tankPos, collisionPoint);
                if (distance < aoe)
                {
                    player.ControlledTank.Damage(weapon,DamageMap[weapon]);
                }
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
            Coordinate coordinate = new Coordinate(currentTankPosition);
            coordinate.Y = m_terrain.GetHeightAt(coordinate.X, coordinate.Z);
            currentTankPosition += new Coordinate(20,0,5);
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

                // StartListening turn timer again
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
                if (player.ControlledTank.Alive)
                {
                    aliveCount++;
                }
            }

            return aliveCount;
        }
    }
}