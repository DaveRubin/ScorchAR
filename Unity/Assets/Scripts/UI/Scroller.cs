using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class Scroller :MonoBehaviour,IDragHandler{

        public bool horizontal =  true;
        public bool loop = true;
        public float min = 0;
        public float max = 360;

        private List<Transform> transforms = new List<Transform>();
        float singleWidth;
        public float value = 0;
//        public ScrollerEvent onValueChanged;
        public Action<float> onValueChanged;
        void Awake() {
            //onValueChanged = new ScrollerEvent();
            transforms.Add(transform.FindChild("ScrollContainer/a"));
            transforms.Add(transform.FindChild("ScrollContainer/b"));
            transforms.Add(transform.FindChild("ScrollContainer/c"));
            singleWidth = transforms[0].GetComponent<RectTransform>().sizeDelta.x;
            //MusicScript musicScript = GetComponent<MusicScript>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            float delta = horizontal?eventData.delta.x:eventData.delta.y;
            value += delta;
            bool update = true;
            if (value >= max) {
                if (loop) value -= max;
                else {
                    value = max;
                    update = false;
                }
            }
            else if ( value < min ) {
                if (loop) value += max;
                else {
                    value = min;
                    update = false;
                }
            }
            if (update) UpdateDials(delta);
            Debug.LogFormat("Update on drag {0} ",value);
            if (onValueChanged != null)
                onValueChanged.Invoke(value);
        }

        public void UpdateDials(float delta) {
            float currentX = 0;
            float extra = 0;
            foreach (Transform transform1 in transforms) {
                transform1.localPosition += new Vector3(delta,0);
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
        }

    }
}