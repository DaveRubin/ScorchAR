using System;
using ScorchEngine.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class LobbyItem :MonoBehaviour {

        private readonly Color defaultColor = Color.white;
        private readonly Color selectedColor = Color.green;

        private Text gameName;
        private Image baseImage;
        private GameInfo gameInfo;

        public GameInfo Info  {
            get {
                return gameInfo;
            }
            set {
                gameInfo = value;
                gameName.text = gameInfo.Name;
            }
        }

        public bool Selected {
            get {
                return baseImage.color == selectedColor;
            }
            set {
                Color color = value? selectedColor: defaultColor;
                baseImage.color = color;
            }
        }

        public event Action<LobbyItem> OnClicked;

        void Awake() {
            baseImage = GetComponent<Image>();
            gameName = transform.Find("Text").GetComponent<Text>();
            GetComponent<Button>().onClick.AddListener(()=>{
                if (OnClicked != null) {
                    OnClicked(this);
                }
            });
            Selected = false;
        }

    }
}