using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    public class LongPressButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

        private float rapidHoldTime = 0.5f;
        private float superRapidHoldTime = 1.2f;
        private float rapidDuration = 0.2f;
        private float superRapidDuration = 0.02f;

        public UnityEvent continuesClick = new UnityEvent();
        private int clicks = 0;

        public void OnPointerDown(PointerEventData eventData) {
            clicks = 0;
            clicks++;
            continuesClick.Invoke();
            Debug.Log("Click");
            //held = false;
            Debug.Log("OnPointerDown");
            InvokeRepeating("RapidClick", rapidHoldTime, rapidDuration);
            InvokeRepeating("SuperRapidClick", superRapidHoldTime, superRapidDuration);
        }

        public void OnPointerUp(PointerEventData eventData) {
            CancelInvoke();
        }

        public void OnPointerExit(PointerEventData eventData) {
            CancelInvoke();
        }

        void RapidClick() {
            clicks++;
            //Debug.Log("Rapid Click" + clicks);
            continuesClick.Invoke();
        }

        void SuperRapidClick() {
            clicks++;
            CancelInvoke("RapidClick");
            //Debug.Log("Super Click" + clicks);
            continuesClick.Invoke();
        }

    }
}