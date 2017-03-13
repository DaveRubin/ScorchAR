using System;
using UnityEngine;

namespace UI {
    public abstract class ScreenBase : MonoBehaviour{

        protected void GoBack() {
            MenusScene.GoTo(EScreenType.MainMenu);
        }
    }
}