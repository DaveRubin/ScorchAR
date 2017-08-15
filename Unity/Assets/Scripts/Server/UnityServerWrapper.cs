using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using ScorchEngine.Models;
using UnityEngine;

namespace Server {
    public class UnityServerWrapper :MonoBehaviour {
        public static UnityServerWrapper Instance;
        void Awake() {
            if (Instance != null) {
                if (Instance.gameObject != null ) {
                    GameObject.Destroy(Instance.gameObject);
                }
            }
            Instance = this;
        }

        void OnDestroy(){
            if (Instance == this) {
                Instance = null;
            }
        }

        public void GetGames(Action<List<GameInfo>> onDoneCallback) {
            StartCoroutine(GetGamesCoroutine(onDoneCallback));
        }

        public IEnumerator GetGamesCoroutine(Action<List<GameInfo>> onDoneCallback) {
            string url = ScorchEngine.Server.ServerRoutes.ServerBaseUri + "/" +ScorchEngine.Server.ServerRoutes.GetGamesApiUrl;
            WWW www = new WWW(url);
            yield return www;
            List<GameInfo> list = new List<GameInfo>();
            if (www.error == null) {
                //fill list
                Debug.LogFormat("Got: {0}",www.text);
                list = JsonConvert.DeserializeObject<List<GameInfo>>(www.text);
            }
            else {
                Debug.LogFormat("Error loading games {0}",www.error);
            }
            onDoneCallback(list);
        }
    }
}