using DG.Tweening;
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
            float damage = 3;
            GameObject.Destroy(this.gameObject);
            GameObject radius = PrefabManager.InstantiatePrefab("ExplosionRadius");
            GameObject fire  = PrefabManager.InstantiatePrefab("ExplosionFX");
            radius.transform.position = transform.position;
            fire.transform.position = transform.position;

            radius.GetComponent<Explosion>().Damage = damage;
            MeshRenderer meshRenderer = radius.GetComponent<MeshRenderer>();
            radius.transform.localScale = Vector3.zero;

            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0,radius.transform.DOScale(3,0.5f));
            sequence.Insert(0.8f,meshRenderer.material.DOFade(0,1));
            sequence.OnComplete(()=>{
                GameObject.Destroy(fire);
                GameObject.Destroy(radius);
            });
        }
    }
}