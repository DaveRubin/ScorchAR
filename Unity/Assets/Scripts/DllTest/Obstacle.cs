using UnityEngine;

public class Obstacle : MonoBehaviour {

    float life = 10;

    void OnTriggerEnter(Collider collider) {
        Explosion explosion = collider.gameObject.GetComponent<Explosion>();
        if (explosion != null) {
            Debug.Log("ONNNNN");
            life -= explosion.Damage;
            if ( life <= 0 ) GameObject.Destroy(gameObject);
        }

    }
}
