using System;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

using ScorchEngine.Models;
using ScorchEngine.Server;

using UnityEngine;

namespace Server
{
    public class UnityServerWrapper : MonoBehaviour
    {
        public static UnityServerWrapper Instance;

        void Awake()
        {
            if (Instance != null)
            {
                if (Instance.gameObject != null)
                {
                    GameObject.Destroy(Instance.gameObject);
                }
            }

            Instance = this;
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void GetGames(Action<List<GameInfo>> onDoneCallback)
        {
            string url = GetURL(ServerRoutes.GetGamesApiUrl);
            StartCoroutine(
                GetCoroutine(
                    url, 
                    www =>
                        {
                            List<GameInfo> list = new List<GameInfo>();
                            if (www.error == null)
                            {
                                list = JsonConvert.DeserializeObject<List<GameInfo>>(www.text);
                            }
                            else
                            {
                                Debug.LogFormat("Error loading games {0}", www.error);
                            }

                            onDoneCallback(list);
                        }));
        }

        public void RemovePlayerFromGame(string gameID, int playerIndex, Action onDoneCallback)
        {
            string url =
                GetURL(
                    ServerRoutes.SetPlayerInActiveUrl.Replace("{id}", gameID).Replace("{index}", playerIndex.ToString()));
            Debug.Log("remove player from game " + url);
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(" ");
            StartCoroutine(
                PostCoroutine(
                    url, 
                    www =>
                        {
                            if (www.error == null)
                            {
                                Debug.LogFormat("Got {0}", www.text);
                                onDoneCallback();
                            }
                            else
                            {
                                Debug.LogError("Error");
                            }
                        }, 
                    postData));
        }

        public void ResetGames()
        {
            StartCoroutine(
                GetCoroutine(
                    GetURL(ServerRoutes.ClearGamesUrl), 
                    www =>
                        {
                            if (www.error != null)
                            {
                                Debug.LogFormat("Error {0}", www.error);
                            }
                            else
                            {
                                Debug.LogFormat("Success!");
                            }
                        }));
        }

        public void AddPlayerToGame(string gameId, PlayerInfo playerInfo, Action<int> onDoneCallback)
        {
            Debug.Log("AddPlayerToGame GAME ID " + ServerRoutes.AddPlayerToGameApiUrl.Replace("{id}", gameId));
            string url = GetURL(ServerRoutes.AddPlayerToGameApiUrl.Replace("{id}", gameId));
            string json = JsonConvert.SerializeObject(playerInfo);
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());
            StartCoroutine(
                PostCoroutine(
                    url, 
                    www =>
                        {
                            if (www.error == null)
                            {
                                Debug.LogFormat("Got {0}", www.text);
                                onDoneCallback(int.Parse(www.text));
                            }
                            else
                            {
                                Debug.LogError("Error");
                            }
                        }, 
                    postData));
        }

        public GameInfo GetGame(string id)
        {
            return null;

            // RestRequest request = new RestRequest(ServerRoutes.GetGameApiUrl.Replace("{id}", id), Method.GET);
            // return client.Execute<GameInfo>(request).Data;
        }

        public void UpdatePlayerState(string gameId, PlayerState playerState, Action<List<PlayerState>> onDoneCallback)
        {
            Debug.Log("get player state " + ServerRoutes.UpdatePlayerStateUrl.Replace("{id}", gameId));
            string url = GetURL(ServerRoutes.UpdatePlayerStateUrl.Replace("{id}", gameId));
            string json = JsonConvert.SerializeObject(playerState);
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());
            StartCoroutine(
                PostCoroutine(
                    url, 
                    www =>
                        {
                            if (www.error == null)
                            {
                                // Debug.LogFormat("Got playerState: {0}", www.text);
                                List<PlayerState> list = JsonConvert.DeserializeObject<List<PlayerState>>(www.text);
                                onDoneCallback(list);
                            }
                            else
                            {
                                Debug.LogError("Error");
                            }
                        }, 
                    postData));
        }

        public void CreateGame(string name, int maxPlayers, PlayerInfo playerInfo, Action<GameInfo> onDoneCallback)
        {
            string urlHash = ServerRoutes.CreateGameUrl + "?name=" + name + "&maxPlayers=" + maxPlayers;
            Debug.Log("creating GAME " + urlHash);
            string url = GetURL(urlHash);
            string json = JsonConvert.SerializeObject(playerInfo);
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());
            StartCoroutine(
                PostCoroutine(
                    url, 
                    www =>
                        {
                            if (www.error == null)
                            {
                                Debug.LogFormat("Got {0}", www.text);
                                JsonConvert.DeserializeObject<GameInfo>(www.text);
                                onDoneCallback(JsonConvert.DeserializeObject<GameInfo>(www.text));
                            }
                            else
                            {
                                Debug.LogError("Error");
                            }
                        }, 
                    postData));
        }

        public void RemovePlayerFromGame(string gameId, int playerIndex)
        {
            // RestRequest request = new RestRequest(ServerRoutes.SetPlayerInActiveUrl.Replace("{id}", gameId).Replace("{index}",playerIndex.ToString()), Method.PUT);
            // client.Execute(request);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Private
        private string GetURL(string api)
        {
            return ServerRoutes.ServerBaseUri + "/" + api;
        }

        private IEnumerator GetCoroutine(string url, Action<WWW> onDoneCallback)
        {
            WWW www = new WWW(url);
            yield return www;
            onDoneCallback(www);
        }

        private IEnumerator PostCoroutine(string url, Action<WWW> onDoneCallback, byte[] postData)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            WWW www = new WWW(url, postData, headers);
            yield return www;
            onDoneCallback(www);
        }
    }
}