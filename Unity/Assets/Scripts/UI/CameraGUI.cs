using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class CameraGUI :MonoBehaviour {

        public event Action OnShootClicked;
        public event Action<float> OnXAngleChange;
        public event Action<float> OnYAngleChange;
        public event Action<float> OnForceChange;

        public float Force {
            get {
                if (sliderForce  == null)  return float.MinValue;
                else return sliderForce.value;
            }
        }
        public float XAngle {
            get {
                if (sliderX  == null)  return float.MinValue;
                else return sliderX.value;
            }
        }
        public float YAngle{
            get {
                if (sliderY == null)  return float.MinValue;
                else return sliderY.value;
            }
        }

        private Text textY;
        private Text textX;
        private Text textForce;

        private Slider sliderY;
        private Slider sliderX;
        private Slider sliderForce;

        private Button fireButton;


        void Awake() {
            GetRelevantComponents();
            RegisterEvents();
            UpdateTexts();
        }

        /// <summary>
        /// Get components from children
        /// </summary>
        private void GetRelevantComponents() {
            fireButton = transform.Find("FireButton").GetComponent<Button>();
            sliderForce = transform.Find("SliderForce").GetComponent<Slider>();
            sliderX = transform.Find("SliderX").GetComponent<Slider>();
            sliderY = transform.Find("SliderY").GetComponent<Slider>();
            textY = transform.Find("AnglesY").GetComponent<Text>();
            textX = transform.Find("AnglesX").GetComponent<Text>();
            textForce = transform.Find("Force").GetComponent<Text>();
            DispatchInitValues();
        }

        /// <summary>
        /// register events to their handlers
        /// </summary>
        private void RegisterEvents() {
            sliderForce.onValueChanged.AddListener(OnForceSliderChange);
            sliderY.onValueChanged.AddListener(OnYChanged);
            sliderX.onValueChanged.AddListener(OnXChanged);
            fireButton.onClick.AddListener(OnFireClicked);
        }


        private void OnFireClicked() {
            if (OnShootClicked != null) {
                OnShootClicked();
            }
        }

        private void OnForceSliderChange(float value) {
            if (OnForceChange != null) {
                OnForceChange(value);
            }
            UpdateTexts();
        }

        private void OnYChanged(float value) {
            if (OnYAngleChange != null) {
                OnYAngleChange(value);
            }
            UpdateTexts();
        }

        private void OnXChanged(float value) {
            if (OnXAngleChange != null) {
                OnXAngleChange(value);
            }
            UpdateTexts();
        }

        private void UpdateTexts() {
            textY.text = YAngle.ToString();
            textX.text = XAngle.ToString();
            textForce.text = Force.ToString();
        }

        public void DispatchInitValues() {
            if (OnXAngleChange != null) OnXAngleChange(XAngle);
            if (OnYAngleChange != null) OnYAngleChange(YAngle);
            if (OnForceChange != null) OnForceChange(Force);
        }
    }
}