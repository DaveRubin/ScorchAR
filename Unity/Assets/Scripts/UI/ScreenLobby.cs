using UnityEngine.UI;

namespace UI {
    public class ScreenLobby : ScreenBase {

        void Awake() {
            transform.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(GoBack);
        }
    }
}