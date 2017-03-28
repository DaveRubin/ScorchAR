using System;
using System.Collections.Generic;
using ScorchEngine.Models;

namespace ScorchEngine.Server
{
    public class ServerWrapper
    {
        private static int debugCounter = 0;


        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static void GetState(Action<List<PlayerState>> i_OnComplete)
        {
            List<PlayerState> list = new List<PlayerState>
            {
                new PlayerState(){ID = 0,Force = debugCounter/0.1f,AngleHorizontal = debugCounter,AngleVertical = debugCounter},
                new PlayerState(){ID = 1,Force = -debugCounter/0.1f,AngleHorizontal = -debugCounter,AngleVertical = -debugCounter},
            };

            i_OnComplete(list);
            debugCounter++;
            if (debugCounter == 100)
            {
                debugCounter = 0;
            }
        }

        /// <summary>
        /// Login to server, get player id
        /// </summary>
        /// <param name="i_Callback"></param>
        public static void Login(string i_GameID,Action<int> i_Callback)
        {
            i_Callback(1);
        }



        /// <summary>
        /// Fetch Lobby games from server
        /// </summary>
        /// <param name="i_CallBack"></param>
        public static void GetGames(Action<List<GameInfo>> i_CallBack) {
            i_CallBack(GetDummyGames());
        }

        /// <summary>
        /// Simulate server games fetch
        /// </summary>
        private static List<GameInfo> GetDummyGames() {
            //create dummy games
            List<GameInfo> games = new List<GameInfo>();
            for (int i = 0; i < 6; i++) {
                GameInfo gameInfo = new GameInfo();
                gameInfo.MaxPlayers = new Random().Next(1,4);
                gameInfo.Name = "Game "+ i;
                gameInfo.ID = "id"+i;

                for (int j = 0; j < gameInfo.MaxPlayers; j++) {
                    PlayerInfo playerInfo = new PlayerInfo();
                    playerInfo.name = "Player "+i+ " "+ j;
                    playerInfo.id = "player" + j;
                    gameInfo.AddPlayer(playerInfo);
                }
                games.Add(gameInfo);
            }
            return games;
        }

    }
}