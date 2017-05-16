using System.Collections.Generic;
using ScorchEngine;
using ScorchEngine.Models;
using ScorchEngine.Server;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI {
    public class ScreenLobby : ScreenBase {

        RectTransform content;
        List<LobbyItem> lobbyItems;
        LobbyItem currentGameSelected;
        Text selectedGameInfo;
        Button buttonJoin;

        void Awake() {
            PrefabManager.Init();
            selectedGameInfo =transform.Find("MainPanel/Right/Panel/Text").GetComponent<Text>();
            transform.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(GoBack);
            buttonJoin = transform.Find("ButtonJoin").GetComponent<Button>();
            content = transform.Find("MainPanel/Left/Scroll View/Viewport/Content").GetComponent<RectTransform>();
            buttonJoin.onClick.AddListener(JoinRoom);
            ServerWrapper.GetGames(OnGamesFetched);
        }

        /// <summary>
        /// When games are fetched, clear list and repopuplate with data
        /// </summary>
        /// <param name="games"></param>
        private void OnGamesFetched(List<GameInfo> games ) {
            Transform container= content.Find("Container");
            float current = 0;
            float height = -1;
            lobbyItems = new List<LobbyItem>();
            foreach (GameInfo game in games) {

                GameObject tmp = PrefabManager.InstantiatePrefab("LobbyGameItem");
                tmp.name = game.Name;
                tmp.transform.SetParent(container);
                tmp.transform.localScale = Vector3.one;
                tmp.transform.localPosition = new Vector3(0, current);
                current -= tmp.GetComponent<RectTransform>().rect.height;
                LobbyItem lobbyItem =tmp.GetComponent<LobbyItem>();
                lobbyItem.Info = game;
                lobbyItem.OnClicked += OnGameSelected;
                lobbyItems.Add(lobbyItem);

                if (height == -1) {
                    height = tmp.GetComponent<RectTransform>().rect.height * games.Count;
                }
            }

            content.sizeDelta = new Vector2(content.rect.width,height);
            container.localPosition += new Vector3(0,height/2);

        }

        /// <summary>
        /// When game is selected, update
        /// </summary>
        /// <param name="selectedLobbyItem"></param>
        public void OnGameSelected(LobbyItem selectedLobbyItem) {
            Debug.LogFormat("Game {0} selected", selectedLobbyItem.Info.Name);
            //deselect all
            foreach (LobbyItem item in lobbyItems) {
                item.Selected = item==selectedLobbyItem;
            }
            currentGameSelected = selectedLobbyItem;
            UpdateSelectedGame();
            buttonJoin.interactable = currentGameSelected != null;
        }

        public void UpdateSelectedGame() {
            GameInfo info = currentGameSelected.Info;
            string playersString  = string.Empty;
            for (int i = 0; i < info.Players.Count; i++) {
                PlayerInfo i1 = info.Players[i];
                playersString += i1;
                if (i != info.Players.Count-1) {
                    playersString += ",";
                }
            }
            selectedGameInfo.text = string.Format(@"Game Name :{0}
Max Players : {1}
Players
-------------
{2}",
            info.Name,info.MaxPlayers,playersString);
        }

        public void JoinRoom() {
            Game.debugLog += (string obj) => Debug.LogError(obj);
            PlayerInfo myInfo = new PlayerInfo();
            myInfo.Name = MainUser.Instance.Name;
            myInfo.Id = MainUser.Instance.Name;
            ServerWrapper.Login(currentGameSelected.Info.Id,myInfo,AfterLogin);
        }

        public void AfterLogin(int index) {
            Debug.LogFormat("Got index {0}",index);
            MainUser.Instance.CurrentGame = currentGameSelected.Info;
            MainUser.Instance.Index = index;
            SceneManager.LoadScene("DLLTest");
        }
    }
}