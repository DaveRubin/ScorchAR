using UnityEngine;

namespace DllTest {

    public class ProjectileControl : MonoBehaviour {

        Vector3 force = Vector3.zero;
        static GameObject explosionPrefab;

        SphereCollider hitTest;
        void Awake() {
            hitTest = GetComponent<SphereCollider>();
            if (explosionPrefab == null) {
                explosionPrefab = Resources.Load<GameObject>("Prefabs/Explosion");
            }
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
            force.y += TestControl.game.GetEnvironmetForces().Y * t;
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
            //Debug.Log("Bam!");
            GameObject.Destroy(this.gameObject);

            GameObject explosion = GameObject.Instantiate(explosionPrefab);
            explosion.transform.position = transform.position;
            explosion.GetComponent<Explosion>().Set(3);
        }
    }
}