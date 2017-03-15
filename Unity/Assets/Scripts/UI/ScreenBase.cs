using UnityEngine;

namespace UI {
    public abstract class ScreenBase : MonoBehaviour{

        protected void GoBack() {
            Debug.Log("A");
            MenusScene.GoTo(EScreenType.MainMenu);
        }
    }
}