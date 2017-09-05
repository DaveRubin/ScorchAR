using System;
using System.Collections.Generic;
using DG.Tweening;
using ScorchEngine.Models;
using Server;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

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
        public event Action<Vector2> OnMove;

        CanvasGroup controls;
        CanvasGroup errorOverlay;

        private Text textY;
        private Text textX;
        private Text textFuel;
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
        private Image joystick;


        private CanvasGroup endGameScreen;
        private CanvasGroup endRoundScreen;

        private Image hitOverlay;

        public int angleTick = 1;
        public int forceTick= 1;
        public const float MAX_FUEL = 200;
        public float fuel = 200;


        void Awake() {
            locked = false;
            fuel = MAX_FUEL;
            GetRelevantComponents();
            RegisterEvents();
            UpdateTexts();
        }


        public void ResetGUI() {
            fuel = MAX_FUEL;
            angleVertical = 0;
            angleHorizontal = 0;
            force = 5;
            SetLocked(false);
            UpdateTexts();
        }

        void Start() {
            DispatchInitValues();
        }

        void Update() {
            Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),CrossPlatformInputManager.GetAxis("Vertical"));
            if (moveVec != Vector2.zero && OnMove != null) {
                float fuelBurn = moveVec.magnitude;
                fuel = Mathf.Clamp(fuel - fuelBurn,0,MAX_FUEL);
                if (fuel > 0 ) {
                    OnMove(moveVec);
                }
                UpdateFuelText();
            }
        }

        public void SetLocked(bool isLocked) {
            locked = isLocked;
            joystick.color = isLocked? new Color(1,1,1,0.2f): Color.white;
            joystick.GetComponent<Joystick>().MovementRange = isLocked? 0:100;
            controls.blocksRaycasts = !isLocked;
        }

        /// <summary>
        /// Get components from children
        /// </summary>
        private void GetRelevantComponents() {
            controls = transform.Find("Controls").GetComponent<CanvasGroup>();
            errorOverlay = transform.Find("ErrorOvelay").GetComponent<CanvasGroup>();
            endGameScreen = transform.Find("EndGameScreen").GetComponent<CanvasGroup>();
            endRoundScreen = transform.Find("EndRoundScreen").GetComponent<CanvasGroup>();
            endRoundScreen.alpha = endGameScreen.alpha = 0;
            endGameScreen.gameObject.SetActive(false);
            hitOverlay = transform.Find("HitOverlay").GetComponent<Image>();
            hitOverlay.gameObject.SetActive(false);
            GetControlComponents();
            DispatchInitValues();
        }

        public Button.ButtonClickedEvent ShowEndGame(bool won) {
            endGameScreen.gameObject.SetActive(true);
            endGameScreen.DOFade(1,1);

            List<PlayerInfo> playerInfos = MainUser.Instance.CurrentGame.Players;
            PlayerInfo oppponent =  MainUser.Instance.Id != playerInfos[0].Id ? playerInfos[0]:playerInfos[1];
            string text = string.Format("{0} WON", won?"YOU":oppponent.Name);
            endGameScreen.transform.Find("Panel/Result").GetComponent<Text>().text = text;
            return endGameScreen.GetComponentInChildren<Button>().onClick;
        }

        public void ShowEndRound(bool won,int scoreP1, int scoreP2,List<TankControl> tanks,Action onComplete) {
            endRoundScreen.gameObject.SetActive(true);
            endRoundScreen.DOFade(1,1);
            endRoundScreen.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            endRoundScreen.GetComponentInChildren<Button>().onClick.AddListener(()=>WaitTillStatusChange(onComplete));


            List<PlayerInfo> playerInfos = MainUser.Instance.CurrentGame.Players;
            PlayerInfo oppponent =  MainUser.Instance.Id != playerInfos[0].Id ? playerInfos[0]:playerInfos[1];

            endRoundScreen.transform.Find("Panel/Result").GetComponent<Text>().text =  string.Format("{0} WON",won?"YOU":oppponent.Name);
            endRoundScreen.transform.Find("Panel/Player1Name").GetComponent<Text>().text =  playerInfos[0].Name;
            endRoundScreen.transform.Find("Panel/Player2Name").GetComponent<Text>().text =  playerInfos[1].Name;
            endRoundScreen.transform.Find("Panel/Player1Score").GetComponent<Text>().text =  scoreP1.ToString();
            endRoundScreen.transform.Find("Panel/Player2Score").GetComponent<Text>().text =  scoreP2.ToString();

        }

        public void GetControlComponents() {
            fireButton = controls.transform.Find("FireButton").GetComponent<Button>();
            showPathButton = controls.transform.Find("ShowPathButton").GetComponent<ExtendedButton>();
            textY = controls.transform.Find("AnglesY").GetComponent<Text>();
            textX = controls.transform.Find("AnglesX").GetComponent<Text>();
            textForce = controls.transform.Find("Force").GetComponent<Text>();
            textFuel = controls.transform.Find("Fuel").GetComponent<Text>();

            angleUp = controls.transform.Find("AngleControl/Up").GetComponent<LongPressButton>();
            angleLeft = controls.transform.Find("AngleControl/Left").GetComponent<LongPressButton>();
            angleRight = controls.transform.Find("AngleControl/Right").GetComponent<LongPressButton>();
            angleDown = controls.transform.Find("AngleControl/Down").GetComponent<LongPressButton>();

            forceUp = controls.transform.Find("ForceControl/Up").GetComponent<LongPressButton>();
            forceDown = controls.transform.Find("ForceControl/Down").GetComponent<LongPressButton>();

            joystick = controls.transform.Find("MobileJoystick").GetComponent<Image>();
        }



        /// <summary>
        /// register events to their handlers
        /// </summary>
        private void RegisterEvents() {
            controls.transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(()=>ToggleExitPopup(true));
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
            UpdateFuelText();
        }

        public void DispatchInitValues() {
            if (OnXAngleChange != null) OnXAngleChange(angleHorizontal);
            if (OnYAngleChange != null) OnYAngleChange(angleVertical);
            if (OnForceChange != null) OnForceChange(force);
        }

        public void DisableErrors() {
            //errorOverlay.gameObject.SetActive(false);
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

        public void UpdateFuelText() {
            textFuel.text = fuel > 0 ? string.Format("FUEL : {0}%", (int)((fuel / MAX_FUEL) * 100)):"No Fuel";
        }

        public void ToggleExitPopup(bool val) {
            Transform exitPopup = controls.transform.Find("ExitPopup");
            exitPopup.gameObject.SetActive(val);
            if (val) {
                exitPopup.Find("Exit").GetComponent<Button>().onClick.AddListener(()=>RemovePlayer());
                exitPopup.Find("Back").GetComponent<Button>().onClick.AddListener(()=>ToggleExitPopup(false));
            }
            else {
                exitPopup.Find("Exit").GetComponent<Button>().onClick.RemoveAllListeners();
                exitPopup.Find("Back").GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }

        public void RemovePlayer() {
            UnityServerWrapper.Instance.RemovePlayerFromGame(MainGame.gameID,MainGame.PlayerIndex, () => {
                SceneManager.LoadScene("Menus");
            });
        }

        public void WaitTillStatusChange(Action onComplete) {
            endRoundScreen.GetComponentInChildren<Button>().interactable = false;
            if (MainGame.currentStatus == EGameStatus.PLAYING) {
                endRoundScreen.DOFade(0,1).OnComplete(()=> {
                    endRoundScreen.gameObject.SetActive(false);
                    endRoundScreen.GetComponentInChildren<Button>().interactable = true;
                });
                onComplete();
                MainGame.statusChanged = null;
            }
            else {
                MainGame.statusChanged += status => CheckGameStatus(status,onComplete);
            }
        }

        public void CheckGameStatus(EGameStatus status,Action onComplete) {
            if (status == EGameStatus.PLAYING) {
                WaitTillStatusChange(onComplete);
            }
        }



    }
}