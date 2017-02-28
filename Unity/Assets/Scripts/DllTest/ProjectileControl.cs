using UnityEngine;

namespace DllTest {

    public class ProjectileControl : MonoBehaviour {

        Vector3 force = Vector3.zero;
        static GameObject explosionPrefab;

        void Awake() {
            if (explosionPrefab == null) {
                explosionPrefab = Resources.Load<GameObject>("Prefabs/Explosion");
            }
        }

        /// <summary>
        /// Itterate the projectile and check if hit the board
        /// </summary>
        void Update() {
            float t = Time.deltaTime * 8;
            force.y += TestControl.game.GetEnvironmetForces().Y * t;
            transform.localPosition += (force * t);

            //set rotation
            transform.localRotation = Quaternion.LookRotation(force);

            // check if hit tank
            // check if hit ground
            if (transform.position.y < 0) {
                Debug.Log("Bam!");
                GameObject.Destroy(this.gameObject);

                GameObject explosion = GameObject.Instantiate(explosionPrefab);
                explosion.transform.position = transform.position;
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
    }
}