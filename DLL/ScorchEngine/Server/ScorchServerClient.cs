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
        private readonly RestClient client = new RestClient(ServerRoutes.ServerBaseUri);

        public List<GameInfo> GetGames()
        {
            RestRequest request = new RestRequest(ServerRoutes.GetGamesApiUrl, Method.GET);
            return client.Execute<List<GameInfo>>(request).Data;
        }

        public void AddPlayerToGame(string i_GameId, PlayerInfo i_PlayerInfo)
        {
            RestRequest request = new RestRequest(ServerRoutes.AddPlayerToGameApiUrl, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(i_PlayerInfo);
            request.AddParameter("id", i_GameId);
            client.Execute(request);
        }

        public GameInfo GetGame(string i_GameId)
        {
            RestRequest request = new RestRequest(ServerRoutes.GetGameApiUrl, Method.GET);
            request.AddParameter("id", i_GameId);
            return client.Execute<GameInfo>(request).Data;
        }
    }
}