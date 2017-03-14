using System;
using System.Collections.Generic;
using DG.Tweening;
using ScorchEngine.Models;

namespace Utils {
    public class ServerWrapper {
        public static Action<List<GameInfo>> OnGamesFetched;


        /// <summary>
        /// Fetch Lobby games from server
        /// </summary>
        public static void GetGames() {
            GetDummyGames();
        }

        /// <summary>
        /// Simulate server fetch
        /// </summary>
        public static void GetDummyGames() {
            //create dummy games
            List<GameInfo> games = new List<GameInfo>();
            for (int i = 0; i < 6; i++) {
                GameInfo gameInfo = new GameInfo();
                gameInfo.maxPlayers = new Random().Next(1,4);
                gameInfo.name = "Game "+ i;
                gameInfo.ID = "id"+i;
                gameInfo.players = new List<PlayerInfo>();

                for (int j = 0; j < gameInfo.maxPlayers; j++) {
                    PlayerInfo playerInfo = new PlayerInfo();
                    playerInfo.name = "Player "+i+ " "+ j;
                    playerInfo.id = "player" + j;
                    gameInfo.players.Add(playerInfo);
                }
                games.Add(gameInfo);
            }

            DOVirtual.DelayedCall(1,()=>{
                if (OnGamesFetched!= null) {
                    OnGamesFetched(games);
                }
            });
        }

    }
}