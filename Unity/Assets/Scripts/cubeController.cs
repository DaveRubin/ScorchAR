using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class cubeController : MonoBehaviour {

    Transform body;
    Transform YAxis;
    Transform ZAxis;
    Slider LeftRightSlider;
    Slider UpDownSlider;


    void Awake() {
        body = GameObject.Find("Cube").transform;
        YAxis = GameObject.Find("YAxis").transform;
        ZAxis = GameObject.Find("ZAxis").transform;
        UpDownSlider = GameObject.Find("UpDownSlider").GetComponent<Slider>();
        LeftRightSlider = GameObject.Find("LeftRightSlider").GetComponent<Slider>();
        UpDownSlider.onValueChanged.AddListener(onUpDownChanged);
        LeftRightSlider.onValueChanged.AddListener(onLeftRightChanged);
    }


    void Update() {
        Vector3 pos = body.localPosition;
        Vector3 yRotation = YAxis.localRotation.eulerAngles;
        Vector3 zRotation = ZAxis.localRotation.eulerAngles;

//Vector3 rotation = YAxis.localRotation.To
        if (Input.GetKey(KeyCode.A)) {
            pos.z += 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            pos.z -= 1;
        }
        if (Input.GetKey(KeyCode.A)) {
            pos.x -= 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            pos.x += 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            yRotation.y -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            yRotation.y += 1;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            zRotation.z += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            zRotation.z -= 1;
        }

        YAxis.localRotation = Quaternion.Euler(yRotation);
        ZAxis.localRotation = Quaternion.Euler(zRotation);
        body.localPosition = pos;
    }

    void onLeftRightChanged(float arg0) {
        Vector3 yRotation = YAxis.localRotation.eulerAngles;
        yRotation.y = 360 * arg0;
        YAxis.localRotation = Quaternion.Euler(yRotation);
    }

    void onUpDownChanged(float arg0) {
        Vector3 zRotation = ZAxis.localRotation.eulerAngles;
        zRotation.z = 360 * arg0;
        ZAxis.localRotation = Quaternion.Euler(zRotation);
    }
}
