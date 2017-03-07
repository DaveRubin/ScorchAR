using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class CameraGUI :MonoBehaviour {
        private Text textY;
        private Text textX;
        private Text textForce;
        private Slider sliderY;
        private Slider sliderX;
        private Slider sliderForce;

        public event Action OnShoot;
        public event Action<float> OnXAngleChange;
        public event Action<float> OnYAngleChange;
        public event Action<float> OnForceChange;

        void Awake() {

            Button fireButton = GameObject.Find("FireButton").GetComponent<Button>();
            sliderForce = GameObject.Find("SliderForce").GetComponent<Slider>();
            sliderX = GameObject.Find("SliderX").GetComponent<Slider>();
            sliderY = GameObject.Find("SliderY").GetComponent<Slider>();
            textY = GameObject.Find("AnglesY").GetComponent<Text>();
            textX = GameObject.Find("AnglesX").GetComponent<Text>();
            textForce = GameObject.Find("Force").GetComponent<Text>();
            sliderForce.onValueChanged.AddListener(OnForceSliderChange);
            sliderY.onValueChanged.AddListener(OnYChanged);
            sliderX.onValueChanged.AddListener(OnXChanged);
            fireButton.onClick.AddListener(OnFireClicked);
            UpdateTexts();
        }


        private void OnFireClicked() {
            if (OnShoot != null) {
                OnShoot();
            }
            //tank.Shoot();
        }

        private void OnForceSliderChange(float value) {
            if (OnForceChange != null) {
                OnForceChange(value);
            }
            UpdateTexts();
            //tank.force = arg0;
        }

        private void OnYChanged(float value) {
            if (OnYAngleChange != null) {
                OnYAngleChange(value);
            }
            UpdateTexts();
            //tank.onUpDownChanged(arg0);
        }

        private void OnXChanged(float value) {
            if (OnXAngleChange != null) {
                OnXAngleChange(value);
            }
            UpdateTexts();
            //tank.onLeftRightChanged(arg0);
        }

        private void UpdateTexts() {
            textY.text = sliderY.value.ToString();
            textX.text = sliderX.value.ToString();
            textForce.text = sliderForce.value.ToString();
        }
    }
}