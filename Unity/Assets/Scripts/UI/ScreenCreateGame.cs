using DG.Tweening;
using ScorchEngine.Models;
using Server;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class ScreenCreateGame :ScreenBase {

        InputField inputField;
        Button createButton;
        string gameName = "";
        Tween beatingTween;

        private bool SKIP_WAITING = false;

        void Awake() {
            //Test();
            base.Awake();
            createButton = transform.Find("ButtonCreate").GetComponent<Button>();
            inputField = transform.Find("InputField").GetComponent<InputField>();
            transform.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(BackToLobby);
            createButton.onClick.AddListener(CreateGame);
            createButton.interactable = false;
            inputField.onValueChanged.AddListener(OnValueChanged);
        }


        public void OnValueChanged(string val) {
            createButton.interactable = val != "" && val != "Enter Room Name";
            gameName = val;
        }

        public void CreateGame() {
            ShowWaitingForOpponent();
            UnityServerWrapper.Instance.CreateGame(gameName,2,MainUser.Instance.GetPLayerInfo(),onGameCreated);
        }

        public void onGameCreated(GameInfo gameInfo) {
            MainUser.Instance.CurrentGame = gameInfo;
            MainUser.Instance.Index = 0;
            UpdateRoomTitle(gameInfo);

            if (SKIP_WAITING) {
                SceneManager.LoadScene(ScreenLobby.SCENE_NAME);
            }
            else {
                InvokeRepeating("WaitForGameToFill", 1, 1f);
            }

        }

        public void WaitForGameToFill()
        {
            UnityServerWrapper.Instance.GetGame(MainUser.Instance.CurrentGame.Id, onWaitForGameToFillPoll);
           
        }

        public void onWaitForGameToFillPoll(GameInfo gameInfo)
        {
            MainUser.Instance.CurrentGame = gameInfo;
            if (gameInfo.IsFull)
            {
                CancelInvoke("WaitForGameToFill");
                OverlayControl.Instance.ToggleLoading(true).OnComplete(()=> {
                    SceneManager.LoadScene(ScreenLobby.SCENE_NAME);
                });

            }

        }

        public void BackToLobby() {
            MenusScene.GoTo(EScreenType.Lobby);
        }

        public void ShowWaitingForOpponent() {
            GameObject waitingBase = transform.Find("WaitingForPlayer").gameObject;
            waitingBase.SetActive(true);
            CanvasGroup canvasGroup = waitingBase.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1,1);
            Transform beatingTitle = waitingBase.transform.Find("Image");
            float size = 0.1f;
            Sequence s = DOTween.Sequence();
            s.Append(beatingTitle.DOPunchScale(Vector3.one * size, 2, 1));
            s.AppendInterval(1);
            s.SetLoops(-1);
            beatingTween = s;
        }

        public void UpdateRoomTitle(GameInfo gameInfo) {
            GameObject waitingBase = transform.Find("WaitingForPlayer").gameObject;
            waitingBase.GetComponentInChildren<Text>().text = string.Format("GAME \"{0}\" CREATED",MainUser.Instance.CurrentGame.Name);

        }

        void OnDestroy() {
            if (beatingTween!= null) beatingTween.Kill();
        }
    }
}