using System;
using ScorchEngine.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class LobbyItem :MonoBehaviour{
        private Text gameName;
        private GameInfo gameInfo;
        public event Action<GameInfo> OnClicked;


        void Awake() {
            gameName = transform.Find("Text").GetComponent<Text>();
            GetComponent<Button>().onClick.AddListener(()=>{
                if (OnClicked != null) {
                    OnClicked(gameInfo);
                }
            });
        }

        public void Set(GameInfo gameInfo) {
            this.gameInfo = gameInfo;
            gameName.text = gameInfo.name;
        }
    }
}