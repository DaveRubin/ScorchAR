using UnityEngine.UI;

namespace UI {
    public class ScreenSettings : ScreenBase {
        void Awake() {
            transform.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(GoBack);
        }
    }
}