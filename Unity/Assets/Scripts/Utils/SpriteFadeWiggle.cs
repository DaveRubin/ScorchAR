using UnityEngine;
using DG.Tweening;

namespace Utils {
    public class SpriteFadeWiggle :MonoBehaviour{
        private float nextTarget = 1;
        private float current = 1;
        public float max = 1;
        public float min = 0;
        public float duration = 1;
        SpriteRenderer s;
        void Awake() {
            s = GetComponent<SpriteRenderer>();
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(Fade);
            sequence.AppendInterval(duration);
            sequence.SetLoops(-1);
        }

        public void Fade() {
            float delta = max-min;
            float half = min + delta/2;
            current = nextTarget;
            nextTarget = Random.Range(half - delta/2 ,half+ delta/2);
            s.DOFade(nextTarget,duration).SetEase(Ease.InOutSine);
        }
    }
}