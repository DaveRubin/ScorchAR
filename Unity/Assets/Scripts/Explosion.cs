using UnityEngine;

public class Explosion : MonoBehaviour {
    public float Damage = 10;
    void OnTriggerEnter(Collider collider) {
        // Check if collided with cube
        TankControl tank = collider.gameObject.GetComponent<TankControl>();
        if (tank != null) {
            tank.Hit(Damage);
            Destroy(GetComponent<SphereCollider>());// remove collider
        }
    }
}
