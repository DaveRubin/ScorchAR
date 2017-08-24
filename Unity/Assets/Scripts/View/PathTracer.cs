using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace View {
    public class PathTracer :MonoBehaviour{

        public bool Enabled = false;

        private const int SEGMENTS = 20; //including root point
        private float gravity = 1; //should get its value from environment forces
        private Vector3[] positions;
        private LineRenderer lineRenderer;
        private Material lineRendererMaterial;
        Tween lineRendererLoop;

        public bool visible = false;

        void Awake() {
            gravity = MainGame.GameCore.GetEnvironmetForces().Y;
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null) {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }

            //create points
            positions = new Vector3[SEGMENTS];
            for (int i = 0; i < SEGMENTS; i++) {
                positions[i] = Vector3.zero;
            }
            InitLineRenderer();
        }

        void OnDestroy() {
            lineRendererLoop.Kill();
        }

        public void SetVisible(bool visible) {
            this.visible = visible;
            lineRendererMaterial.DOFade(visible?1:0,0.5f);
            //lineRenderer.material.color = visible? Color.white:Color.clear;
//            gameObject.SetActive(visible);
        }

        public void SetPath(Vector3 velocity) {
            //set path per point
            float density = 0.1f;
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < lineRenderer.positionCount; i++) {
                lineRenderer.SetPosition(i,pos);
                pos+=velocity*density;
                velocity.y += gravity*density;
            }
        }

        public void InitLineRenderer() {
            lineRenderer.material = Resources.Load("Materials/ProjectilePatheMaterial") as Material;
            lineRendererLoop = DOVirtual.Float(1,0,0.5f,val=>{
                lineRenderer.material.mainTextureOffset = new Vector2(val,0);
            }).SetEase(Ease.Linear).SetLoops(-1);
            lineRenderer.useWorldSpace = false;
            lineRenderer.receiveShadows = false;
            lineRenderer.shadowCastingMode =  ShadowCastingMode.Off;
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0;

            lineRenderer.positionCount = SEGMENTS;
            lineRenderer.SetPositions(positions);
            lineRendererMaterial = lineRenderer.material;
        }

    }
}