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
        private const bool DebugMode = true;
       // private readonly RestClient client = new RestClient((DebugMode ? ServerRoutes.LocalBaseUri :ServerRoutes.ServerBaseUri));

        public List<GameInfo> GetGames()
        {
            RestClient client = new RestClient((DebugMode ? ServerRoutes.LocalBaseUri : ServerRoutes.ServerBaseUri));
            RestRequest request = new RestRequest(ServerRoutes.GetGamesApiUrl, Method.GET);
            return client.Execute<List<GameInfo>>(request).Data;
        }

        public int AddPlayerToGame(string id, PlayerInfo playerInfo)
        {
            RestClient client = new RestClient((DebugMode ? ServerRoutes.LocalBaseUri : ServerRoutes.ServerBaseUri));
            RestRequest request = new RestRequest(ServerRoutes.AddPlayerToGameApiUrl.Replace("{id}",id), Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(playerInfo);
            return client.Execute<int>(request).Data;
        }

        public GameInfo GetGame(string id)
        {
            RestClient client = new RestClient((DebugMode ? ServerRoutes.LocalBaseUri : ServerRoutes.ServerBaseUri));
            RestRequest request = new RestRequest(ServerRoutes.GetGameApiUrl, Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.AddParameter("id", id);
            IRestResponse<GameInfo> response = client.Execute<GameInfo>(request);
            GameInfo res = response.Data;
            return client.Execute<GameInfo>(request).Data;
        }
    }
}