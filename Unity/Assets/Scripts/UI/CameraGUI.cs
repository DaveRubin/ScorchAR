using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class CameraGUI :MonoBehaviour {

        private float angleHorizontal = 0;
        private float angleVertical = 0;
        private float force = 0;

        private bool locked = false;
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

        private LongPressButton up;
        private LongPressButton left;
        private LongPressButton right;
        private LongPressButton down;

        private CanvasGroup endGameScreen;

        public int angleTick = 1;


        void Awake() {
            locked = false;
            GetRelevantComponents();
            RegisterEvents();
            UpdateTexts();
        }

        void Start() {
            DispatchInitValues();
        }

        public void SetLocked(bool isLocked) {
            locked = isLocked;
            controls.blocksRaycasts = !isLocked;
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

            up = controls.transform.Find("AngleControl/Up").GetComponent<LongPressButton>();
            left = controls.transform.Find("AngleControl/Left").GetComponent<LongPressButton>();
            right = controls.transform.Find("AngleControl/Right").GetComponent<LongPressButton>();
            down = controls.transform.Find("AngleControl/Down").GetComponent<LongPressButton>();
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

            up.continuesClick.AddListener(()=>AngleChanged(0,angleTick));
            left.continuesClick.AddListener(()=>AngleChanged(-angleTick,0));
            right.continuesClick.AddListener(()=>AngleChanged(angleTick,0));
            down.continuesClick.AddListener(()=>AngleChanged(0,-angleTick));
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

        public void AngleChanged(int horizontal, int vertical) {
            angleHorizontal += horizontal;
            angleVertical += vertical;
            Debug.LogFormat("A {0} {1}",angleHorizontal,angleVertical);
            if (horizontal != 0 && OnXAngleChange != null) {
                OnXAngleChange(angleHorizontal);
            }
            if (vertical != 0 && OnYAngleChange != null) {
                OnYAngleChange(angleVertical);
            }
        }

        private void UpdateTexts() {
            textY.text = angleVertical.ToString();
            textX.text = angleHorizontal.ToString();
            textForce.text = Force.ToString();
        }

        public void DispatchInitValues() {
            if (OnXAngleChange != null) OnXAngleChange(angleHorizontal);
            if (OnYAngleChange != null) OnYAngleChange(angleVertical);
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
            Sequence sequence = DOTween.Sequence();
            if (detected) {
                sequence.Insert(0,errorOverlay.DOFade(0,fadeTime));
                sequence.Insert(0,controls.DOFade(1,fadeTime));
                sequence.OnComplete(()=> {
                    errorOverlay.gameObject.SetActive(false);
                });
            }
            else {
                errorOverlay.gameObject.SetActive(true);
                sequence.Insert(0,errorOverlay.DOFade(1,fadeTime));
                sequence.Insert(0,controls.DOFade(0,fadeTime));
            }

            errorToggleTween = sequence;

        }


    }
}