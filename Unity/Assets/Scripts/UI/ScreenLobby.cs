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

//        public const string SCENE_NAME = "DLLTest";
        public const string SCENE_NAME = "MainGame";

        RectTransform content;
        List<LobbyItem> lobbyItems;
        LobbyItem currentGameSelected;
        //Text selectedGameInfo;
        Button buttonJoin;

        Image flash;
        Transform leftPlayer;
        Transform rightPlayer;
        Transform vs;
        Transform roomName;
        Tween popTween;
        Tween loopTween;

        void Awake() {
            //Test();
            base.Awake();
            PrefabManager.Init();
            GetInfoComponents();
            //selectedGameInfo =transform.Find("MainPanel/Right/Panel/Text").GetComponent<Text>();
            transform.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(GoBack);
            buttonJoin = transform.Find("ButtonJoin").GetComponent<Button>();
            content = transform.Find("MainPanel/Left/Scroll View/Viewport/Content").GetComponent<RectTransform>();
            buttonJoin.onClick.AddListener(JoinRoom);
            //transform.Find("ButtonReset").GetComponent<Button>().onClick.AddListener(ResetPressed);
            transform.Find("ButtonCreate").GetComponent<Button>().onClick.AddListener(Create);
            onEnter += StartGetGamesPoll;
        }

        void Start() {
            StartGetGamesPoll();
        }

        void StartGetGamesPoll()
        {
            InvokeRepeating("UpdateLobby",0f,5f);
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
            if (popTween != null) popTween.Kill(true);
            if (loopTween != null) loopTween.Kill(true);

            GameInfo info = currentGameSelected.Info;
            Sequence sequence = DOTween.Sequence();
            roomName.GetComponent<Text>().text = info.Name;
            if (info.Players.Count>0)  leftPlayer.GetComponentInChildren<Text>().text = info.Players[0].Name;
            flash.color = Color.white;
            vs.localScale = Vector3.zero;
            rightPlayer.localScale = Vector3.zero;
            leftPlayer.localScale = Vector3.zero;
            float bounceDuration = 1;
            sequence.Insert(0,flash.DOFade(0,0.5f));
            sequence.Insert(0,leftPlayer.DOScale(1,bounceDuration).SetEase(Ease.OutElastic,0.2f));
            sequence.Insert(0.25f,rightPlayer.DOScale(1,bounceDuration).SetEase(Ease.OutElastic,0.2f));
            sequence.Insert(0.5f,vs.DOScale(1,bounceDuration).SetEase(Ease.OutElastic,0.2f));
            sequence.AppendCallback(()=>{
                float halfSine = 0.5f;
                float size = 1.1f;
                Sequence loopSequence = DOTween.Sequence();
                loopSequence.Insert(0,leftPlayer.DOScale(size,halfSine).SetEase(Ease.InOutSine));
                loopSequence.Insert(0.3f,vs.DOScale(size,halfSine).SetEase(Ease.InOutSine));
                loopSequence.Insert(0.6f,rightPlayer.DOScale(size,halfSine).SetEase(Ease.InOutSine));
                loopSequence.Insert(halfSine,leftPlayer.DOScale(1,halfSine).SetEase(Ease.InOutSine));
                loopSequence.Insert(halfSine +0.3f,vs.DOScale(1,halfSine).SetEase(Ease.InOutSine));
                loopSequence.Insert(halfSine +0.6f,rightPlayer.DOScale(1,halfSine).SetEase(Ease.InOutSine));
                loopSequence.Insert(0,buttonJoin.transform.DOPunchScale(new Vector3(0.05f,0.05f,0.05f),1,2));
                loopSequence.InsertCallback(2,()=>{});

                loopSequence.SetLoops(-1);
                loopTween = loopSequence;
            });
            popTween = sequence;
        }

        public void JoinRoom() {
            Game.debugLog += (string obj) => Debug.LogError(obj);
            OverlayControl.Instance.ToggleLoading(true).OnComplete(()=> {
                UnityServerWrapper.Instance.AddPlayerToGame(currentGameSelected.Info.Id,MainUser.Instance.GetPLayerInfo(),AfterLogin);
            });
        }

        public void AfterLogin(int index) {
            MainUser.Instance.CurrentGame = currentGameSelected.Info;
            //TODO Not DO Hack
            MainUser.Instance.CurrentGame.Players.Add(MainUser.Instance.GetPLayerInfo());
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
            UnityServerWrapper.Instance.GetGames(OnGamesFetched);
        }


        public void Create() {
            MenusScene.GoTo(EScreenType.CreateGame);
        }

        public void ShowCreateGameMenu() {

        }

        public void onGameCreated(GameInfo gameInfo) {

        }

        public void GetInfoComponents() {
            Transform panel = transform.Find("MainPanel/Right/Panel");
            flash = panel.Find("Flash").GetComponent<Image>();
            leftPlayer = panel.Find("P1");
            rightPlayer = panel.Find("P2");
            vs = panel.Find("VS");
            roomName = panel.Find("RoomName");
        }

    }
}