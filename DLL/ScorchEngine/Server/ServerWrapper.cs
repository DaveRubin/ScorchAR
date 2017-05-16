using System;
using System.Collections.Generic;
using ScorchEngine.Models;

namespace ScorchEngine.Server
{
    public class ServerWrapper
    {
        private static int debugCounter = 0;
        private static ScorchServerClient clinet = new ScorchServerClient();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static void GetState(PlayerState myState, Action<List<PlayerState>> i_OnComplete)
        {
            List<PlayerState> list = new List<PlayerState>
            {
                new PlayerState(){Id = 0,Force = debugCounter/0.1f,AngleHorizontal = debugCounter,AngleVertical = debugCounter},
                new PlayerState(){Id = 1,Force = -debugCounter/0.1f,AngleHorizontal = -debugCounter,AngleVertical = -debugCounter},
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
            i_CallBack(clinet.GetGames());
        }

    }
}