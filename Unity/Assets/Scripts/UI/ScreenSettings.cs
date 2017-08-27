using UnityEngine.UI;

namespace UI {
    public class ScreenSettings : ScreenBase {
        void Awake() {
            base.Awake();
            transform.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(GoBack);
        }
    }
}