using UnityEngine;
using Utils;

namespace DllTest {

    public class ProjectileControl : MonoBehaviour {

        Vector3 force = Vector3.zero;

        SphereCollider hitTest;
        void Awake() {
            hitTest = GetComponent<SphereCollider>();
        }

        void OnCollisionEnter(Collision collision) {
            // Check if collided with cube
            //Debug.LogFormat("Collided with {0} ",collision.gameObject.name);
            Explode();
        }

        /// <summary>
        /// Itterate the projectile and check if hit the board
        /// </summary>
        void Update() {
            float t = Time.deltaTime * 8;
            force.y += TestControl.GameCore.GetEnvironmetForces().Y * t;
            transform.localPosition += (force * t);

            //set rotation
            if (force != Vector3.zero)
                transform.localRotation = Quaternion.LookRotation(force);

            // check if hit tank
            // check if hit ground
            if (transform.position.y < 0) {
                Explode();
            }
            // check if passed its life
        }

        /// <summary>
        /// Set projectile's force
        /// </summary>
        /// <param name="force"></param>
        public void SetForce(Vector3 force) {
            this.force = force;
        }

        public void Explode() {
            GameObject.Destroy(this.gameObject);
            GameObject explosion = PrefabManager.InstantiatePrefab("Explostion");
            explosion.transform.position = transform.position;
            explosion.GetComponent<Explosion>().Set(3);
        }
    }
}