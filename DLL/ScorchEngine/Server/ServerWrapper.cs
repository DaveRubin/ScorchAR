using System;
using System.Collections.Generic;
using ScorchEngine.Models;

namespace ScorchEngine.Server
{
    public class ServerWrapper
    {
        private static ScorchServerClient client = new ScorchServerClient();

        public static void Test()
        {
            Game.Log("In Test()");
            //client.AddPlayerToGame("12", null);
            Game.Log("Out Test()");
        }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static void GetState(string gameId,PlayerState myState, Action<List<PlayerState>> i_OnComplete)
        {
            //i_OnComplete(client.UpdatePlayerState(gameId, myState));
            client.UpdatePlayerStateAsync(gameId,myState,i_OnComplete);
         
        }

        /// <summary>
        /// Login to server, get player id
        /// </summary>
        /// <param name="i_Callback"></param>
        public static void Login(string gameId, PlayerInfo playerInfo,Action<int> i_Callback)
        {
            Game.Log("Login "+gameId);
            int a = client.AddPlayerToGame(gameId, playerInfo);
            Game.Log("Login after client... "+a);
            i_Callback(a);
        }



        /// <summary>
        /// Fetch Lobby games from server
        /// </summary>
        /// <param name="i_CallBack"></param>
        public static void GetGames(Action<List<GameInfo>> i_CallBack) {
            i_CallBack(client.GetGames());
        }


        public static void RemovePlayerFromGame(string gameId, int playerIndex)
        {
            client.RemovePlayerFromGame(gameId,playerIndex);
        }

        public static void ResetGames()
        {
            client.ResetGames();
        }
    }
}