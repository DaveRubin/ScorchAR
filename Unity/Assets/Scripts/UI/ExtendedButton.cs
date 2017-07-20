using UnityEngine.UI;

namespace UI {
    public class ExtendedButton :Button{

        public ButtonClickedEvent onPointerDown;
        public ButtonClickedEvent onPointerExit;
        public ButtonClickedEvent onPointerUp;

        public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData) {
            onPointerDown.Invoke();
        }

        public override void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData) {
            onPointerExit.Invoke();
        }

        public override void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData) {
            onPointerUp.Invoke();
        }
    }
}