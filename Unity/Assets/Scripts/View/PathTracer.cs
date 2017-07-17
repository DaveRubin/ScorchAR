using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace View {
    public class PathTracer :MonoBehaviour{

        const int SEGMENTS = 20;
        private float gravity = 1;
        private List<Transform> points = new List<Transform>();

        void Awake() {
            gravity = MainGame.GameCore.GetEnvironmetForces().Y;
            //create points
            for (int i = 0; i < SEGMENTS; i++) {
                Transform cube = PrefabManager.InstantiatePrefab("Cube").transform;
                cube.SetParent(transform);
                cube.localScale = Vector3.one*0.2f;
                cube.localPosition = Vector3.zero;
                points.Add(cube);
            }
        }

        public void SetPath(Vector3 velocity) {

            //set path per point
            float density = 0.1f;
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < SEGMENTS; i++) {
                pos+=velocity*density;
                velocity.y += gravity*density;
                points[i].localPosition = pos;
            }
        }

    }
}