using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ScreenSettings : ScreenBase {
        void Awake() {
            base.Awake();
            Toggle toggle = transform.Find("Toggle").GetComponent<Toggle>();
            transform.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(GoBack);
            AudioListener.pause = false;
            toggle.onValueChanged.AddListener(OnValueChange);
        }

        public void OnValueChange(bool val) {
            AudioListener.pause = !val;
        }
    }
}