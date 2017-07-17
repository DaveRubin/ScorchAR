using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace View {
    public class PathTracer :MonoBehaviour{

        const int SEGMENTS = 20;
        private float gravity = 1;
        private LineRenderer lineRenderer;
        private Vector3[] positions;


        void Awake() {
            gravity = MainGame.GameCore.GetEnvironmetForces().Y;
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null) {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
            InitLineRenderer();

            //create points
            positions = new Vector3[SEGMENTS];
            for (int i = 0; i < SEGMENTS; i++) {
                positions[i] = Vector3.zero;
            }
            lineRenderer.positionCount = SEGMENTS;
            lineRenderer.SetPositions(positions);
        }

        public void SetPath(Vector3 velocity) {
            Debug.Log(gameObject.GetInstanceID());

            //set path per point
            float density = 0.1f;
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < SEGMENTS; i++) {
                pos+=velocity*density;
                velocity.y += gravity*density;
                lineRenderer.SetPosition(i,pos);
            }
        }

        public void InitLineRenderer() {
            lineRenderer.material = Resources.Load("Materials/ProjectilePatheMaterial") as Material;
            DOVirtual.Float(1,0,0.5f,val=>{
                lineRenderer.material.mainTextureOffset = new Vector2(val,0);
            }).SetEase(Ease.Linear).SetLoops(-1);
            lineRenderer.useWorldSpace = false;
            lineRenderer.receiveShadows = false;
            lineRenderer.shadowCastingMode =  ShadowCastingMode.Off;
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0;
        }

    }
}