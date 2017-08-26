using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class CameraGUI :MonoBehaviour {

        public event Action OnShootClicked;
        public event Action<float> OnXAngleChange;
        public event Action<float> OnYAngleChange;
        public event Action<float> OnForceChange;
        public event Action<bool> OnShowPath;

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

        CanvasGroup controls;
        CanvasGroup errorOverlay;

        private Text textY;
        private Text textX;
        private Text textForce;

        private Scroller sliderY;
        private Scroller sliderX;
        private Scroller sliderForce;

        private Button fireButton;
        private ExtendedButton showPathButton;
        private Tween errorToggleTween;

        private CanvasGroup endGameScreen;


        void Awake() {
            GetRelevantComponents();
            RegisterEvents();
            UpdateTexts();
        }

        void Start() {
            DispatchInitValues();
        }

        /// <summary>
        /// Get components from children
        /// </summary>
        private void GetRelevantComponents() {
            controls = transform.Find("Controls").GetComponent<CanvasGroup>();
            errorOverlay = transform.Find("ErrorOvelay").GetComponent<CanvasGroup>();
            endGameScreen = transform.Find("EndGameScreen").GetComponent<CanvasGroup>();
            endGameScreen.alpha = 0;
            endGameScreen.gameObject.SetActive(false);
            GetControlComponents();
            DispatchInitValues();
        }

        public Button.ButtonClickedEvent ShowEndGame(bool won) {
            endGameScreen.gameObject.SetActive(true);
            endGameScreen.DOFade(1,1);
            string text = won? "YOU WON":  "YOU LOST";
            endGameScreen.transform.Find("Panel/Result").GetComponent<Text>().text = text;
            return endGameScreen.GetComponentInChildren<Button>().onClick;
        }

        public void GetControlComponents() {
            fireButton = controls.transform.Find("FireButton").GetComponent<Button>();
            showPathButton = controls.transform.Find("ShowPathButton").GetComponent<ExtendedButton>();
            sliderForce = controls.transform.Find("ScrollerForce").GetComponent<Scroller>();
            sliderX = controls.transform.Find("ScrollerX").GetComponent<Scroller>();
            sliderY = controls.transform.Find("ScrollerY").GetComponent<Scroller>();
            textY = controls.transform.Find("AnglesY").GetComponent<Text>();
            textX = controls.transform.Find("AnglesX").GetComponent<Text>();
            textForce = controls.transform.Find("Force").GetComponent<Text>();
        }

        /// <summary>
        /// register events to their handlers
        /// </summary>
        private void RegisterEvents() {
            sliderForce.onValueChanged += OnForceSliderChange;
            sliderY.onValueChanged += OnYChanged;
            sliderX.onValueChanged += OnXChanged;
            fireButton.onClick.AddListener(OnFireClicked);
            showPathButton.onPointerDown.AddListener(()=>TogglePath(true));
            showPathButton.onPointerUp.AddListener(()=>TogglePath(false));
            showPathButton.onPointerExit.AddListener(()=>TogglePath(false));
        }

        public void TogglePath(bool val) {
            if (OnShowPath != null) {
                OnShowPath(val);
            }
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

        public void DisableErrors() {
            errorOverlay.gameObject.SetActive(false);
        }

        /// <summary>
        /// When tracked is undetected,show "TRACKER NOT DETECTED"
        /// </summary>
        /// <param name="detected"></param>
        public void ToggleTrackerDetection(bool detected) {
            if (errorToggleTween != null) errorToggleTween.Kill();
            float fadeTime = 0.5f;
            CanvasGroup canvasIn = detected? errorOverlay:controls;
            CanvasGroup canvasOut = !detected? errorOverlay:controls;
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0,canvasIn.DOFade(1,fadeTime));
            sequence.Insert(0,canvasOut.DOFade(0,fadeTime));
            sequence.InsertCallback(0,()=>{
                if (!detected) errorOverlay.gameObject.SetActive(true);
                if (detected) controls.gameObject.SetActive(true);
            });

        }

    }
}