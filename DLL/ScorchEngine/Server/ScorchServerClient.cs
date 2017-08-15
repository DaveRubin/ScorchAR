﻿using System;
using System.Collections.Generic;

using RestSharp;

using ScorchEngine.Models;

namespace ScorchEngine.Server
{
    using System.Diagnostics;

    public class ScorchServerClient
    {

        private const bool DebugMode = false;
        private readonly RestClient client =
            new RestClient(DebugMode ? ServerRoutes.LocalBaseUri : ServerRoutes.ServerBaseUri);

        

        public List<GameInfo> GetGames()
        {
            RestRequest request = new RestRequest(ServerRoutes.GetGamesApiUrl, Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            return client.Execute<List<GameInfo>>(request).Data;
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

        public string CreateGame(string name, int maxPlayers, PlayerInfo playerInfo)
        {
            RestRequest request = new RestRequest(ServerRoutes.CreateGameUrl, Method.POST);
            request.AddJsonBody(playerInfo);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.AddQueryParameter("name", name);
            request.AddQueryParameter("maxPlayers", maxPlayers.ToString());
            return client.Execute(request).Content.Replace("\"", "");
        }

        public void RemovePlayerFromGame(string gameId, int playerIndex)
        {
            RestRequest request = new RestRequest(ServerRoutes.RemovePlayerFromGameUrl.Replace("{id}", gameId).Replace("{index}",playerIndex.ToString()), Method.PUT);
            client.Execute(request);
        }
    }
}