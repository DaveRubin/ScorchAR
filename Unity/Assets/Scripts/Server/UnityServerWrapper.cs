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
                            if (string.IsNullOrEmpty(www.error))
                            {

                                //Debug.LogErrorFormat("got games {0}", www.text);
                                List<GameInfo> list = JsonConvert.DeserializeObject<List<GameInfo>>(www.text);
                                onDoneCallback(list);
                            }
                            else
                            {
                                Debug.LogFormat("Error loading games {0}", www.error);
                            }
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
                            if (string.IsNullOrEmpty(www.error))
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
                                OnError(www);
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
                            if (string.IsNullOrEmpty(www.error))
                            {
                                //Debug.LogFormat("Got {0}", www.text);
                                onDoneCallback(int.Parse(www.text));
                            }
                            else
                            {
                                OnError(www);
                            }
                        }, 
                    postData));
        }

        public void UpdatePlayerState(string gameId, PlayerState playerState, Action<List<PlayerState>> onDoneCallback)
        {
            //Debug.Log("get player state " + ServerRoutes.UpdatePlayerStateUrl.Replace("{id}", gameId));
            string url = GetURL(ServerRoutes.UpdatePlayerStateUrl.Replace("{id}", gameId));
            string json = JsonConvert.SerializeObject(playerState);
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());
            StartCoroutine(
                PostCoroutine(
                    url, 
                    www =>
                        {
                            if (string.IsNullOrEmpty(www.error))
                            {
                                // Debug.LogFormat("Got playerState: {0}", www.text);
                                List<PlayerState> list = JsonConvert.DeserializeObject<List<PlayerState>>(www.text);
                                onDoneCallback(list);
                            }
                            else
                            {
                                OnError(www);
                            }
                        }, 
                    postData));
        }

        public void CreateGame(string name, int maxPlayers, int rounds, PlayerInfo playerInfo, Action<GameInfo> onDoneCallback)
        {
            string urlHash = ServerRoutes.CreateGameUrl + "?name=" + name + "&maxPlayers=" + maxPlayers + "&rounds=" + rounds;
            Debug.Log("creating GAME " + urlHash);
            string url = GetURL(urlHash);
            string json = JsonConvert.SerializeObject(playerInfo);
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());
            StartCoroutine(
                PostCoroutine(
                    url, 
                    www =>
                        {
                            if (string.IsNullOrEmpty(www.error))
                            {
                                Debug.LogFormat("Got {0}", www.text);
                                JsonConvert.DeserializeObject<GameInfo>(www.text);
                                onDoneCallback(JsonConvert.DeserializeObject<GameInfo>(www.text));
                            }
                            else
                            {
                                OnError(www);
                            }
                        }, 
                    postData));
        }

        public void CreateUser(string name, Action<PlayerInfo> onDoneCallback)
        {
            string urlHash = ServerRoutes.CreateUserUrl;
            Debug.Log("creating User " + urlHash);
            string url = GetURL(urlHash);
            string json = JsonConvert.SerializeObject(name);
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());
            StartCoroutine(
                PostCoroutine(
                    url,
                    www =>
                    {
                        if (string.IsNullOrEmpty(www.error))
                        {
                           // Debug.LogFormat("Got {0}", www.text);
                            JsonConvert.DeserializeObject<GameInfo>(www.text);
                            onDoneCallback(JsonConvert.DeserializeObject<PlayerInfo>(www.text));
                        }
                        else
                        {
                            Debug.LogErrorFormat("Error creating user {0}", www.error);
                        }
                    },
                    postData));
        }

        public void UpdateUser(PlayerInfo playerInfo, Action<PlayerInfo> onDoneCallback)
        {
            string urlHash = ServerRoutes.UpdateUserNameUrl;
            Debug.Log("Updateing User Name " + urlHash);
            string url = GetURL(urlHash);
            string json = JsonConvert.SerializeObject(playerInfo);
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());
            StartCoroutine(
                PostCoroutine(
                    url,
                    www =>
                    {
                        if (string.IsNullOrEmpty(www.error))
                        {
                            //Debug.LogFormat("Got {0}", www.text);
                            JsonConvert.DeserializeObject<GameInfo>(www.text);
                            onDoneCallback(JsonConvert.DeserializeObject<PlayerInfo>(www.text));
                        }
                        else
                        {
                            Debug.LogErrorFormat("Error updateing user {0}",www.error);
                        }
                    },
                    postData));
        }

        public void GetGame(string id, Action<GameInfo> onDoneCallback)
        {
            string urlHash = ServerRoutes.GetGameApiUrl.Replace("{id}",id);
            string url = GetURL(urlHash);
            StartCoroutine(
                GetCoroutine(
                    url,
                    www =>
                    {
                        if (string.IsNullOrEmpty(www.error))
                        {
                            //Debug.LogError(www.text);
                            GameInfo gameinfo = JsonConvert.DeserializeObject<GameInfo>(www.text);
                            onDoneCallback(gameinfo);
                        }
                        else
                        {
                            Debug.LogFormat("Error getting game {0} {1}",id, www.error);
                        }
                    }));
        }

        public void CancelGame(string id, Action onDoneCallback)
        {
            string urlHash = ServerRoutes.CancelGame.Replace("{id}", id);
            string url = GetURL(urlHash);
            StartCoroutine(
                GetCoroutine(
                    url,
                    www =>
                    {
                        if (string.IsNullOrEmpty(www.error))
                        {
                            onDoneCallback();
                        }
                        else
                        {
                            Debug.LogFormat("Error getting game {0} {1}", id, www.error);
                        }
                    }));
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

        public void OnError(WWW www10) {
            Debug.LogErrorFormat("GOT ERROR ON {0} ,WITH TEXT : {1} :",www10.url,www10.error);
        }
    }
}