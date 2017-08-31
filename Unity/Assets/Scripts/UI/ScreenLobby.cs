using System.Collections.Generic;
using DG.Tweening;
using ScorchEngine;
using ScorchEngine.Models;
using ScorchEngine.Server;
using Server;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UI {
    public class ScreenLobby : ScreenBase {

        private const bool TEST = false;
//        public const string SCENE_NAME = "DLLTest";
        public const string SCENE_NAME = "MainGame";

        RectTransform content;
        List<LobbyItem> lobbyItems;
        LobbyItem currentGameSelected;
        Text selectedGameInfo;
        Button buttonJoin;

        void Awake() {
            //Test();
            base.Awake();
            PrefabManager.Init();
            selectedGameInfo =transform.Find("MainPanel/Right/Panel/Text").GetComponent<Text>();
            transform.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(GoBack);
            buttonJoin = transform.Find("ButtonJoin").GetComponent<Button>();
            content = transform.Find("MainPanel/Left/Scroll View/Viewport/Content").GetComponent<RectTransform>();
            buttonJoin.onClick.AddListener(JoinRoom);
            //transform.Find("ButtonReset").GetComponent<Button>().onClick.AddListener(ResetPressed);
            transform.Find("ButtonCreate").GetComponent<Button>().onClick.AddListener(Create);
            onEnter += UpdateLobby;
        }

        void Start() {
            UpdateLobby();
        }

        /// <summary>
        /// When games are fetched, clear list and repopuplate with data
        /// </summary>
        /// <param name="games"></param>
        private void OnGamesFetched(List<GameInfo> games ) {
            if (lobbyItems != null) {
                Debug.Log("Clearing lobby");
                foreach (LobbyItem lobbyItem1 in lobbyItems) {
                    GameObject.Destroy(lobbyItem1.gameObject);
                }
            }

            Transform container= content.Find("Container");
            container.localPosition = new Vector3(0,0,0);
            content.sizeDelta = new Vector2(content.rect.width,0);
            float current = 0;
            float height = -1;
            lobbyItems = new List<LobbyItem>();
            if (games == null) {
                height = 0;
            }
            else {
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
            }


            content.sizeDelta = new Vector2(content.rect.width,height);
            container.localPosition = new Vector3(0,0);
            Debug.LogFormat(@"height -{0} | games.Count - {1} (-1 if null) | content.sizeDelta.ToString() -{2} | container.localPosition -{3}",
            height,
            games == null? -1:games.Count,
            content.sizeDelta.ToString(),
            container.localPosition);

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
            OverlayControl.Instance.ToggleLoading(true).OnComplete(()=> {
                UnityServerWrapper.Instance.AddPlayerToGame(currentGameSelected.Info.Id,MainUser.Instance.GetPLayerInfo(),AfterLogin);
            });
        }

        public void AfterLogin(int index) {
            MainUser.Instance.CurrentGame = currentGameSelected.Info;
            MainUser.Instance.Index = index;
            SceneManager.LoadScene(SCENE_NAME);
        }

        public void TestDummyGames() {
            List<GameInfo> listGameInfo = new List<GameInfo>();
            for (int i = 0; i < 10; i++) {
                GameInfo gi = new GameInfo();
                gi.Id = i.ToString();
                gi.Name = "Game "+i;
                gi.MaxPlayers = 2;
                gi.Players = new List<PlayerInfo>();;
                for (int j = 0; j < gi.MaxPlayers; j++) {
                    PlayerInfo p = new PlayerInfo();
                    p.Name = "Player " + j;
                    p.Id = j.ToString();
                    gi.Players.Add(p);
                }
                listGameInfo.Add(gi);
            }
            OnGamesFetched(listGameInfo);
        }

        public void ResetPressed() {
            Debug.Log("ResetGames");
            UnityServerWrapper.Instance.ResetGames();
            UpdateLobby();
        }

        public void UpdateLobby() {
            if (TEST) {
                TestDummyGames();
            }
            else {
                UnityServerWrapper.Instance.GetGames(OnGamesFetched);
            }
        }

        public void Test() {
            string s = "[{\"PlayerStates\":[{\"LastUpdateTime\":\"0001-01-01T00:00:00\",\"Id\":0,\"IsReady\":false,\"AngleHorizontal\":0.0,\"AngleVertical\":0.0,\"Force\":0.0},{\"LastUpdateTime\":\"0001-01-01T00:00:00\",\"Id\":0,\"IsReady\":false,\"AngleHorizontal\":0.0,\"AngleVertical\":0.0,\"Force\":0.0}],\"Name\":\"Game0 \",\"Id\":\"id0\",\"MaxPlayers\":2,\"Players\":[]},{\"PlayerStates\":[{\"LastUpdateTime\":\"0001-01-01T00:00:00\",\"Id\":0,\"IsReady\":false,\"AngleHorizontal\":0.0,\"AngleVertical\":0.0,\"Force\":0.0},{\"LastUpdateTime\":\"0001-01-01T00:00:00\",\"Id\":0,\"IsReady\":false,\"AngleHorizontal\":0.0,\"AngleVertical\":0.0,\"Force\":0.0}],\"Name\":\"Game0 \",\"Id\":\"id0\",\"MaxPlayers\":2,\"Players\":[]}]";
            //string s = "{\"PlayerStates\":[{\"LastUpdateTime\":\"0001-01-01T00:00:00\",\"Id\":0,\"IsReady\":false,\"AngleHorizontal\":0.0,\"AngleVertical\":0.0,\"Force\":0.0},{\"LastUpdateTime\":\"0001-01-01T00:00:00\",\"Id\":0,\"IsReady\":false,\"AngleHorizontal\":0.0,\"AngleVertical\":0.0,\"Force\":0.0}],\"Name\":\"Game0 \",\"Id\":\"id0\",\"MaxPlayers\":2,\"Players\":[]},{\"PlayerStates\":[{\"LastUpdateTime\":\"0001-01-01T00:00:00\",\"Id\":0,\"IsReady\":false,\"AngleHorizontal\":0.0,\"AngleVertical\":0.0,\"Force\":0.0},{\"LastUpdateTime\":\"0001-01-01T00:00:00\",\"Id\":0,\"IsReady\":false,\"AngleHorizontal\":0.0,\"AngleVertical\":0.0,\"Force\":0.0}],\"Name\":\"Game0 \",\"Id\":\"id0\",\"MaxPlayers\":2,\"Players\":[]}";
            var c = JsonConvert.DeserializeObject<List<GameInfo>>(s);
            Debug.Log(c);
        }

        public void Create() {
            MenusScene.GoTo(EScreenType.CreateGame);
        }

        public void ShowCreateGameMenu() {

        }

        public void onGameCreated(GameInfo gameInfo) {

        }

    }
}