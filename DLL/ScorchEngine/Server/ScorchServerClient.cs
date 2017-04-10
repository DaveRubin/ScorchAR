using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScorchEngine.Server
{
    using RestSharp;

    using ScorchEngine.Models;

    public class ScorchServerClient
    {
        private const bool DebugMode = false;
        private readonly RestClient client = new RestClient((DebugMode ? ServerRoutes.LocalBaseUri :ServerRoutes.ServerBaseUri));

        public List<GameInfo> GetGames()
        {
            RestRequest request = new RestRequest(ServerRoutes.GetGamesApiUrl, Method.GET);
            return client.Execute<List<GameInfo>>(request).Data;
        }

        public void AddPlayerToGame(string id, PlayerInfo playerInfo)
        {
            RestRequest request = new RestRequest(ServerRoutes.AddPlayerToGameApiUrl.Replace("{id}",id), Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(playerInfo);
            client.Execute(request);
        }

        public GameInfo GetGame(string id)
        {
            RestRequest request = new RestRequest(ServerRoutes.GetGameApiUrl, Method.GET);
            request.AddParameter("id", id);
            return client.Execute<GameInfo>(request).Data;
        }
    }
}