using UnityEngine;

public class Explosion : MonoBehaviour {
    public float Damage = 0;

    void OnCollisionEnter(Collision collision) {
        // Check if collided with cube

        Debug.LogFormat("Collided with {0} {1}", collision.gameObject.name, gameObject.name);
        TankControl tank = collision.gameObject.GetComponent<TankControl>();

        if (tank != null) {
            tank.Hit(Damage);
            Destroy(GetComponent<SphereCollider>());// remove collider
        }
    }
}
