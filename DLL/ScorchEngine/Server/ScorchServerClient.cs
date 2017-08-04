using System;
using System.Collections.Generic;

using RestSharp;

using ScorchEngine.Models;

namespace ScorchEngine.Server
{
    public class ScorchServerClient
    {
        private readonly RestClient client =
            new RestClient(DebugMode ? ServerRoutes.LocalBaseUri : ServerRoutes.ServerBaseUri);

        private const bool DebugMode = false;

        public List<GameInfo> GetGames()
        {
            RestRequest request = new RestRequest(ServerRoutes.GetGamesApiUrl, Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            IRestResponse<List<GameInfo>> response = client.Execute<List<GameInfo>>(request);
            return response.Data;
        }

        public int AddPlayerToGame(string gameId, PlayerInfo playerInfo)
        {
            Game.Log("AddPlayerToGame GAME ID " + ServerRoutes.AddPlayerToGameApiUrl.Replace("{id}", gameId));
            RestRequest request = new RestRequest(
                ServerRoutes.AddPlayerToGameApiUrl.Replace("{id}", gameId), 
                Method.POST);
            request.RequestFormat = DataFormat.Json;
            Game.Log("GAME ID " + gameId);
            request.AddJsonBody(playerInfo);
            return client.Execute<int>(request).Data;
        }

        public GameInfo GetGame(string id)
        {
            RestRequest request = new RestRequest(ServerRoutes.GetGameApiUrl.Replace("{id}", id), Method.GET);
            return client.Execute<GameInfo>(request).Data;
        }

        public List<PlayerState> UpdatePlayerState(string id, PlayerState playerState)
        {
            RestRequest request = new RestRequest(ServerRoutes.UpdatePlayerStateUrl.Replace("{id}", id), Method.PUT);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(playerState);
            return client.Execute<List<PlayerState>>(request).Data;
        }

        public void UpdatePlayerStateAsync(string id, PlayerState playerState, Action<List<PlayerState>> callback)
        {
            RestRequest request = new RestRequest(ServerRoutes.UpdatePlayerStateUrl.Replace("{id}", id), Method.PUT);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(playerState);
            client.ExecuteAsync<List<PlayerState>>(request, r => callback(r.Data));
        }

        public void ResetGames()
        {
            RestRequest request = new RestRequest(ServerRoutes.ClearGamesUrl, Method.GET);
            client.Execute(request);
        }

        public string createGame(string name, int maxPlayers, PlayerInfo playerInfo)
        {
            RestRequest request = new RestRequest(ServerRoutes.CreateGameUrl, Method.POST);
            request.AddJsonBody(playerInfo);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.AddQueryParameter("name", name);
            request.AddQueryParameter("maxPlayers", maxPlayers.ToString());
            return client.Execute(request).Content.Replace("\"", "");
        }
    }
}