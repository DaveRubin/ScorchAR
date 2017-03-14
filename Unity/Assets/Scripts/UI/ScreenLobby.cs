using System.Collections.Generic;
using ScorchEngine.Models;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI {
    public class ScreenLobby : ScreenBase {

        RectTransform content;
        GameObject lobbyItemPrefab;

        void Awake() {
            lobbyItemPrefab = Resources.Load<GameObject>("Prefabs/LobbyGameItem");
            transform.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(GoBack);
            content = transform.Find("MainPanel/Left/Scroll View/Viewport/Content").GetComponent<RectTransform>();
            ServerWrapper.OnGamesFetched += OnGamesFetched;
            ServerWrapper.GetGames();

        }

        /// <summary>
        /// When games are fetched, clear list and repopuplate with data
        /// </summary>
        /// <param name="games"></param>
        private void OnGamesFetched(List<GameInfo> games ) {
            Transform container= content.Find("Container");
            float current = 0;
            float height = -1;

            foreach (GameInfo game in games) {

                GameObject tmp = Instantiate(lobbyItemPrefab);
                tmp.name = game.name;
                tmp.transform.SetParent(container);
                tmp.transform.localScale = Vector3.one;
                tmp.transform.localPosition = new Vector3(0, current);
                current -= tmp.GetComponent<RectTransform>().rect.height;
                tmp.GetComponent<LobbyItem>().Set(game);
                tmp.GetComponent<LobbyItem>().OnClicked += OnGameSelected;

                if (height == -1) {
                    height = tmp.GetComponent<RectTransform>().rect.height * games.Count;
                }
            }

            content.sizeDelta = new Vector2(content.rect.width,height);
            container.localPosition += new Vector3(0,height/2);

        }

        public void OnGameSelected(GameInfo obj) {
            Debug.LogFormat("Game {0} selected",obj.name);
        }
    }
}