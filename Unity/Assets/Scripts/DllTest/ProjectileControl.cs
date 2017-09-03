using DG.Tweening;
using UnityEngine;
using Utils;

namespace DllTest {

    public class ProjectileControl : MonoBehaviour {

        const float TIME_FACTOR = 3;
        Vector3 force = Vector3.zero;
        Transform origin;
        private GameObject a, terrain;
        private TerrainDeform terrainDeformScript;

        SphereCollider hitTest;
        float x,y, terrainHeight;


        void Awake() {
            hitTest = GetComponent<SphereCollider>();
            a = MainGame.GetTerrain();
            terrain = MainGame.GetTerrain();
            terrainDeformScript = terrain.GetComponent<TerrainDeform>();
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
            x = transform.position.x;
            y = transform.position.z;
            terrainHeight = a.GetComponentInChildren<Terrain>().SampleHeight(new Vector3(x,0,y));

            if(transform.position.y < terrainHeight){
                Explode();
            }

            if (transform.position.y < 0) {
                //Debug.Log("Ground");
                Explode();
            }

            transform.localPosition += force*Time.deltaTime*TIME_FACTOR;
            force.y += MainGame.GameCore.GetEnvironmetForces().Y*Time.deltaTime*TIME_FACTOR;
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
            float damage = 25;
            GameObject.Destroy(this.gameObject);
            GameObject radius = PrefabManager.InstantiatePrefab("ExplosionRadius");
            GameObject fire  = PrefabManager.InstantiatePrefab("ExplosionFX");
            radius.transform.position = transform.position;
            fire.transform.position = transform.position;
            //terrainDeformScript.DeformMesh((int)transform.position.x,(int)transform.position.z,5,5000);
            terrainDeformScript.DeformMesh((int)transform.position.x,(int)transform.position.z,10,1);


            radius.GetComponent<Explosion>().Damage = damage;
            MeshRenderer meshRenderer = radius.GetComponent<MeshRenderer>();
            radius.transform.localScale = Vector3.zero;

            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0,radius.transform.DOScale(5,0.5f));
            sequence.Insert(0.8f,meshRenderer.material.DOFade(0,1));
            sequence.OnComplete(()=>{
                GameObject.Destroy(fire);
                GameObject.Destroy(radius);
            });
            Debug.Log("explosion at: " + (int)transform.position.x + " " + (int)transform.position.z);
        }
    }
}