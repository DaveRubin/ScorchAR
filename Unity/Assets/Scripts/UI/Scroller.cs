using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    public class Scroller :MonoBehaviour,IDragHandler{
        private List<Transform> transforms = new List<Transform>();
        private Text text;
        float singleWidth;
        float min = 0;
        float max = 360;
        public float value = 0;
        public ScrollerEvent onValueChanged;

        void Awake() {
            onValueChanged = new ScrollerEvent();
            transforms.Add(transform.FindChild("ScrollContainer/a"));
            transforms.Add(transform.FindChild("ScrollContainer/b"));
            transforms.Add(transform.FindChild("ScrollContainer/c"));
            text = transform.FindChild("TextContainer/Text").GetComponent<Text>();
            text.text = value.ToString();
            singleWidth = transforms[0].GetComponent<RectTransform>().sizeDelta.x;

        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.LogFormat("I'm being dragged! {0}",eventData.delta);
            float currentX = 0;
            float extra = 0;
            foreach (Transform transform1 in transforms) {
                transform1.localPosition += new Vector3(eventData.delta.x,0);
                currentX = transform1.localPosition.x;

                if (currentX >= singleWidth*2) {
                    extra = currentX - singleWidth*2;
                    transform1.localPosition = new Vector3(-singleWidth + extra,0,0);
                }
                if (currentX <= -singleWidth*2) {
                    extra = currentX + singleWidth*2;
                    transform1.localPosition = new Vector3(singleWidth +extra,0,0);
                }
            }
            value += eventData.delta.x;
            if (value >= max) value -= max;
            else if ( value < min ) value += max;
            text.text = value.ToString();
            onValueChanged.Invoke(value);
        }

    }
}