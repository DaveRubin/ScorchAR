using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class OverlayControl : MonoBehaviour {
        public bool hideOnStart;
        public static OverlayControl Instance;
        private Image loading;
        private const float FADE_DURATION = 2f;

        void Awake() {
            if (Instance != null) {
                GameObject.Destroy(this);
            }
            else {
                Instance = this;
                loading = transform.Find("Loading").GetComponent<Image>();
            }

            if (hideOnStart) ToggleLoading(false,false);
        }

        public Tween ToggleLoading(bool show, bool animate = true) {
            Sequence sequence = DOTween.Sequence();
            if (show) gameObject.SetActive(true);
            sequence.Append(loading.DOFade(show?1:0, animate?FADE_DURATION:0));
            sequence.AppendCallback(()=> {
                if (!show) gameObject.SetActive(false);
            });
            return sequence;
        }

    }
}