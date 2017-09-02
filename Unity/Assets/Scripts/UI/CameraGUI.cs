using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class CameraGUI :MonoBehaviour {

        private const int MIN_VERTICAL = 0;
        private const int MAX_VERTICAL = 90;

        private const int MIN_FORCE = 0;
        private const int MAX_FORCE = 30;

        private float angleHorizontal = 132;
        private float angleVertical = 53;
        private float force = 20;

        private bool locked = false;
        public event Action OnShootClicked;
        public event Action<float> OnXAngleChange;
        public event Action<float> OnYAngleChange;
        public event Action<float> OnForceChange;
        public event Action<bool> OnShowPath;

        CanvasGroup controls;
        CanvasGroup errorOverlay;

        private Text textY;
        private Text textX;
        private Text textForce;

        private Button fireButton;
        private ExtendedButton showPathButton;
        private Tween errorToggleTween;

        private LongPressButton angleUp;
        private LongPressButton angleLeft;
        private LongPressButton angleRight;
        private LongPressButton angleDown;

        private LongPressButton forceUp;
        private LongPressButton forceDown;


        private CanvasGroup endGameScreen;

        private Image hitOverlay;

        public int angleTick = 1;
        public int forceTick= 1;


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
            hitOverlay = transform.Find("HitOverlay").GetComponent<Image>();
            hitOverlay.gameObject.SetActive(false);
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
            textY = controls.transform.Find("AnglesY").GetComponent<Text>();
            textX = controls.transform.Find("AnglesX").GetComponent<Text>();
            textForce = controls.transform.Find("Force").GetComponent<Text>();

            angleUp = controls.transform.Find("AngleControl/Up").GetComponent<LongPressButton>();
            angleLeft = controls.transform.Find("AngleControl/Left").GetComponent<LongPressButton>();
            angleRight = controls.transform.Find("AngleControl/Right").GetComponent<LongPressButton>();
            angleDown = controls.transform.Find("AngleControl/Down").GetComponent<LongPressButton>();

            forceUp = controls.transform.Find("ForceControl/Up").GetComponent<LongPressButton>();
            forceDown = controls.transform.Find("ForceControl/Down").GetComponent<LongPressButton>();
        }

        /// <summary>
        /// register events to their handlers
        /// </summary>
        private void RegisterEvents() {
            fireButton.onClick.AddListener(OnFireClicked);
            showPathButton.onPointerDown.AddListener(()=>TogglePath(true));
            showPathButton.onPointerUp.AddListener(()=>TogglePath(false));
            showPathButton.onPointerExit.AddListener(()=>TogglePath(false));

            angleUp.continuesClick.AddListener(()=>AngleChanged(0,angleTick,0));
            angleLeft.continuesClick.AddListener(()=>AngleChanged(-angleTick,0,0));
            angleRight.continuesClick.AddListener(()=>AngleChanged(angleTick,0,0));
            angleDown.continuesClick.AddListener(()=>AngleChanged(0,-angleTick,0));
            forceUp.continuesClick.AddListener(()=>AngleChanged(0,0,forceTick));
            forceDown.continuesClick.AddListener(()=>AngleChanged(0,0,-forceTick));
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

        public void AngleChanged(int horizontal, int vertical,int forceDelta) {
            angleHorizontal += horizontal;
            angleVertical += vertical;
            force += forceDelta;

            if (angleHorizontal > 360) angleHorizontal = angleHorizontal % 360;
            if (angleHorizontal < 0) angleHorizontal += 360;
            angleVertical = Mathf.Clamp(angleVertical,MIN_VERTICAL,MAX_VERTICAL);
            force = Mathf.Clamp(force,MIN_FORCE,MAX_FORCE);

            if (horizontal != 0 && OnXAngleChange != null) {
                OnXAngleChange(angleHorizontal);
            }
            if (vertical != 0 && OnYAngleChange != null) {
                OnYAngleChange(angleVertical);
            }
            if (force != 0 && OnForceChange!= null) {
                OnForceChange(force);
            }
            UpdateTexts();
        }

        private void UpdateTexts() {
            Debug.LogFormat("A {0} {1}",angleHorizontal,angleVertical);
            textY.text = string.Format("{0}º",angleVertical);
            textX.text = string.Format("{0}º",angleHorizontal);
            textForce.text = force.ToString();
        }

        public void DispatchInitValues() {
            if (OnXAngleChange != null) OnXAngleChange(angleHorizontal);
            if (OnYAngleChange != null) OnYAngleChange(angleVertical);
            if (OnForceChange != null) OnForceChange(force);
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

        public void DoOnHitAnimation()
        {
            hitOverlay.gameObject.SetActive(true);
            hitOverlay.color = Color.red;
            hitOverlay.DOFade(0, 1.5f).OnComplete(()=> { hitOverlay.gameObject.SetActive(false); });  
        }

    }
}