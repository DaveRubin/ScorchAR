using DG.Tweening;
using ScorchEngine.Models;
using Server;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class ScreenCreateGame :ScreenBase {

        InputField inputField;
        Button createButton;
        string gameName = "";

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
            OverlayControl.Instance.ToggleLoading(true).OnComplete(()=> {
                UnityServerWrapper.Instance.CreateGame(gameName,2,MainUser.Instance.GetPLayerInfo(),onGameCreated);
            });
        }

        public void onGameCreated(GameInfo gameInfo) {
            MainUser.Instance.CurrentGame = gameInfo;
            MainUser.Instance.Index = 0;
            InvokeRepeating("WaitForGameToFill", 1, 1f);
            
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
                SceneManager.LoadScene(ScreenLobby.SCENE_NAME);
            }


        }

        public void BackToLobby() {
            MenusScene.GoTo(EScreenType.Lobby);
        }
    }
}