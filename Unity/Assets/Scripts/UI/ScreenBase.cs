using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public abstract class ScreenBase : MonoBehaviour{

        protected Tween motion;
        protected CanvasGroup canvasGroup;

        public float FADE_DURATION = 0.5f;

        protected void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public Tween Enter() {
            if (motion != null)
                motion.Kill(true);
            
            canvasGroup.alpha = 1;
            transform.position = MenusScene.EnterStartPos.position;
            gameObject.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0,transform.DOMove(MenusScene.EnterEndPos.position,FADE_DURATION).SetEase(Ease.OutExpo));
            sequence.Insert(0,transform.DORotate(MenusScene.EnterEndPos.rotation.eulerAngles,FADE_DURATION).SetEase(Ease.OutExpo));
            sequence.AppendCallback(()=> {
            });
            motion = sequence;
            return sequence;
        }

        protected void GoBack() {
            MenusScene.GoTo(EScreenType.MainMenu);
        }

        public void Exit() {
            if (motion != null)
                motion.Kill(true);

            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0,DOVirtual.Float(canvasGroup.alpha,0,FADE_DURATION,val=> {
                canvasGroup.alpha = val;
            }));
            sequence.Insert(0,transform.DOMove(MenusScene.ExitEndPos.position,FADE_DURATION));
            sequence.OnComplete(()=>gameObject.SetActive(false));
            motion = sequence;
        }
    }
}