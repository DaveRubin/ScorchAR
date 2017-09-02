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

        private bool SKIP_WAITING = true;

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
                InvokeRepeating("WaitForGameToFill", 1f, 1f);
            }

        }

        private void WaitForGameToFill()
        {
            UnityServerWrapper.Instance.GetGame(MainUser.Instance.CurrentGame.Id, onWaitForGameToFillPoll);
           
        }

        private void onWaitForGameToFillPoll(GameInfo gameInfo)
        {
            MainUser.Instance.CurrentGame = gameInfo;
            if (gameInfo.IsGameFull())
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

            //Animation
            float size = 0.1f;
            Sequence s = DOTween.Sequence();
            GameObject waitingBase = transform.Find("WaitingForPlayer").gameObject;
            CanvasGroup canvasGroup = waitingBase.GetComponent<CanvasGroup>();
            Transform beatingTitle = waitingBase.transform.Find("Image");
            waitingBase.SetActive(true);
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1,1);
            s.Append(beatingTitle.DOPunchScale(Vector3.one * size, 2, 1));
            s.AppendInterval(1);
            s.SetLoops(-1);
            beatingTween = s;

            //cancel button
            canvasGroup.GetComponentInChildren<Button>().onClick.AddListener(OnCancelPressed);
        }

        public void UpdateRoomTitle(GameInfo gameInfo) {
            Text titleText = transform.Find("WaitingForPlayer/Text").GetComponent<Text>();
            titleText.text = string.Format("GAME \"{0}\" CREATED",MainUser.Instance.CurrentGame.Name);
        }

        void OnDestroy() {
            if (beatingTween!= null) beatingTween.Kill();
        }

        public void OnCancelPressed() {
            UnityServerWrapper.Instance.CancelGame(MainUser.Instance.CurrentGame.Id, onCancelGame);
        }

        private void onCancelGame()
        {
            if (beatingTween != null) beatingTween.Kill();
            GameObject waitingBase = transform.Find("WaitingForPlayer").gameObject;
            CanvasGroup canvasGroup = waitingBase.GetComponent<CanvasGroup>();
            canvasGroup.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            canvasGroup.DOFade(0, 1).OnComplete(() => {
                waitingBase.SetActive(false);
                CancelInvoke("WaitForGameToFill");
                BackToLobby();
            });
        }
    }
}