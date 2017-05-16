using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp;

using ScorchEngine.Models;

namespace ScorchEngine.Server
{
    public class ScorchServerClient
    {
        private readonly RestClient client = new RestClient(DebugMode ? ServerRoutes.LocalBaseUri : ServerRoutes.ServerBaseUri);

        private const bool DebugMode = false;

        // private readonly RestClient client = new RestClient((DebugMode ? ServerRoutes.LocalBaseUri :ServerRoutes.ServerBaseUri));
        public List<GameInfo> GetGames()
        {
            RestRequest request = new RestRequest(ServerRoutes.GetGamesApiUrl, Method.GET);
            return client.Execute<List<GameInfo>>(request).Data;
        }

        public int AddPlayerToGame(string gameId, PlayerInfo playerInfo)
        {
            Game.Log("AddPlayerToGame GAME ID "+ ServerRoutes.AddPlayerToGameApiUrl.Replace("{id}", gameId));
            RestRequest request = new RestRequest(ServerRoutes.AddPlayerToGameApiUrl.Replace("{id}", gameId), Method.POST);
            request.RequestFormat = DataFormat.Json;
            Game.Log("GAME ID "+gameId);
            request.AddJsonBody(playerInfo);
            return client.Execute<int>(request).Data;
        }

        public GameInfo GetGame(string id)
        {
            RestRequest request = new RestRequest(ServerRoutes.GetGameApiUrl, Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.AddParameter("id", id);
            return client.Execute<GameInfo>(request).Data;
        }

        public List<PlayerState> UpdatePlayerState(string id,PlayerState playerState)
        {
            RestRequest request = new RestRequest(ServerRoutes.UpdatePlayerStateUrl.Replace("{id}", id), Method.PUT);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(playerState);
            return client.Execute<List<PlayerState>>(request).Data;
        }

        public void ResetGames()
        {
            RestRequest request = new RestRequest(ServerRoutes.ClearGamesUrl, Method.GET);
            client.Execute(request);
        }
    }
}