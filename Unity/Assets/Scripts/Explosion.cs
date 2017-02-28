using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    float life = 0;

    // Update is called once per frame
    void Awake() {
        transform.position = Vector3.zero;
    }

    void Update() {
        life += Time.deltaTime;
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 5, life);
        if (life > 1) {
            GameObject.Destroy(this.gameObject);
        }
    }
}
