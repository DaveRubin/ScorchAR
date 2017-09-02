using UnityEngine;

namespace Utils {
    public class TankColorer : MonoBehaviour{

        private Material tankMetal;
        private Material primary;
        private Material secondary;

        void  Awake() {
            foreach (MeshRenderer mRend in GetComponentsInChildren<MeshRenderer>()) {
                Debug.Log(mRend.material.name);
                if (mRend.material.name.Contains("Tank_Metal") ) {
                    if (tankMetal == null) {
                        tankMetal = new Material(mRend.material);
                    }
                    mRend.material = tankMetal;
                }
                if (mRend.material.name.Contains("Tank_Secondary")) {
                    if (secondary == null) {
                        secondary = new Material(mRend.material);
                    }
                    mRend.material = secondary;
                }
                if (mRend.material.name.Contains("Tank_Primary") ) {
                    if (primary == null) {
                        primary = new Material(mRend.material);
                    }
                    mRend.material = primary;
                }

            }
        }

        public void Color(Color primaryColor,Color secondaryColor,Color metalColor) {
            primary.color = primaryColor;
            secondary.color = secondaryColor;
            tankMetal.color = metalColor;
        }
    }
}