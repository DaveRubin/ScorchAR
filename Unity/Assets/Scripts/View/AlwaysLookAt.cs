using UnityEngine;

namespace View {
    public class AlwaysLookAt :MonoBehaviour{
        public Transform target;
        void Update() {
            if (target != null) {
                transform.LookAt(target,Vector3.up);
            }
        }

        public void SetTarget(Transform target) {
            this.target = target;
        }
    }
}