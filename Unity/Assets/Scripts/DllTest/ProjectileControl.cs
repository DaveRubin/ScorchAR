using UnityEditor;
using UnityEngine;
using Utils;

namespace DllTest {

    public class ProjectileControl : MonoBehaviour {

        Vector3 force = Vector3.zero;
        Transform origin;

        SphereCollider hitTest;
        
        void Awake() {
            hitTest = GetComponent<SphereCollider>();
        }

        void OnCollisionEnter(Collision collision) {
            // Check if collided with cube
            Debug.LogFormat("Collided with {0} {1}",collision.gameObject.name,origin.gameObject);
            if (collision.gameObject != origin.gameObject) {
                Explode();
            }
        }

        /// <summary>
        /// Itterate the projectile and check if hit the board
        /// </summary>
        void Update() {
            // check if hit ground
            if (transform.position.y < 0) {
                //Debug.Log("Ground");
                Explode();
            }
            transform.localPosition += force*Time.deltaTime;
            force.y += MainGame.GameCore.GetEnvironmetForces().Y*Time.deltaTime;
            // check if passed its life
        }

        /// <summary>
        /// Set projectile's force
        /// </summary>
        /// <param name="force"></param>
        public void SetForce(Transform origin, Vector3 force) {
            this.force = force;
            this.origin = origin;
        }

        public void Explode() {
            GameObject.Destroy(this.gameObject);
            GameObject explosion = PrefabManager.InstantiatePrefab("Explosion");
            explosion.transform.position = transform.position;
            explosion.GetComponent<Explosion>().SetDamage(3);
        }
    }
}