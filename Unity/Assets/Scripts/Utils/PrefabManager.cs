using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {

    /// <summary>
    /// Handles all prefab loading and instanstiaing
    /// </summary>
    public class PrefabManager {
        private static PrefabManager Instance;
        private static Dictionary<string,GameObject> prefabsMap = new Dictionary<string, GameObject>();

        private PrefabManager() {

        }

        static PrefabManager() {
            if (Instance == null) {
                Instance = new PrefabManager();
            }
        }

        /// <summary>
        /// Load resources folder prefabs, and map them for future use
        /// </summary>
        public static void Init() {
            object[] objects = Resources.LoadAll("Prefabs");
            foreach (GameObject o in objects) {
                if (prefabsMap.ContainsKey(o.name)) {
                    throw new Exception(string.Format("An object nameed {0} is already exists in map."));
                }
                prefabsMap.Add(o.name,o);
            }
        }

        /// <summary>
        /// Get prefab
        /// </summary>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        public static GameObject InstantiatePrefab(string prefabName) {
            return GameObject.Instantiate(prefabsMap[prefabName]);
        }
//
//        /// <summary>
//        /// Instantiate prefab and get component out of it
//        /// </summary>
//        /// <param name="prefabName"></param>
//        /// <returns></returns>
//        public static GameObject InstantiatePrefab<T>(string prefabName) {
//            return prefabsMap[prefabName].GetComponent<T>();
//        }
    }
}