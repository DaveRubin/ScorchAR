using DG.Tweening;
using UnityEngine;

namespace View {

    public class PositionMarker :MonoBehaviour{
        Tween motionTween = null;
        private Vector3 initialLocalPosition = Vector3.zero;

        void Awake() {
            initialLocalPosition = transform.localPosition;
            gameObject.SetActive(false);
        }

        public void Enable() {
            ResetPosition();
            gameObject.SetActive(true);
            motionTween = transform.DOMoveY(initialLocalPosition.y - 1,0.5f).SetLoops(-1,LoopType.Yoyo);
        }

        public void Disable() {
            gameObject.SetActive(false);
            ResetPosition();
        }

        public void ResetPosition() {
            if (motionTween != null) {
                motionTween.Kill();
                transform.position = initialLocalPosition;
            }
        }
    }
}