using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TD.Util {
    public class FXManager : MonoBehaviour {

        [HideInInspector] public List<FXPrefab> fxPrefabs;
        private Dictionary<string, List<GameObject>> fxPoolDict;

        private static FXManager instance;
        private Transform fxParent;


        private void Awake() {
            instance = this;
            GenerateIenumerables();
        }

        void GenerateIenumerables() {
            GeneratePool();
        }

        void GeneratePool() {
            // Create a parent that will hold all effects
            fxParent = new GameObject("Effects Parent").transform;

            // Create pool dictionary
            fxPoolDict = new Dictionary<string, List<GameObject>>();

            // Get All Types
            List<string> fxTypes = fxPrefabs.Select(prefab => prefab.prefabType).Distinct().ToList();

            // Initialize pool dictionary
            foreach (var fxType in fxTypes) {
                fxPoolDict.Add(fxType, new List<GameObject>());
            }

            foreach (FXPrefab fxPrefab in fxPrefabs) {
                CreateFX(fxPrefab);
            }
        }

        /// <summary>
        /// Play FX from the pool.
        /// </summary>
        /// <param name="fxName">Name of the FX</param>
        /// <param name="pos">Position of the FX</param>
        /// <param name="selfDestructTime">Self destruct time</param>
        public static void PlayFX(string fxName, Vector3 pos, float selfDestructTime) {
            GameObject fx = instance.GetFxFromPool(fxName);
            fx.SetActive(true);
            fx.transform.position = pos;
            instance.StartCoroutine(instance.SelfDestruct(fx, selfDestructTime));
        }

        public static void PlayFX(string fxName, Vector3 pos, Vector3 scale, float selfDestructTime, bool parent = false) {
            GameObject fx = instance.GetFxFromPool(fxName);
            fx.SetActive(true);
            fx.transform.position = pos;
            fx.transform.localScale = scale;
            instance.StartCoroutine(instance.SelfDestruct(fx, selfDestructTime));
        }

        public static void PlayFX(string fxName, Vector3 pos, Quaternion rot, float selfDestructTime, bool parent = false) {
            GameObject fx = instance.GetFxFromPool(fxName);
            fx.SetActive(true);
            fx.transform.position = pos;
            fx.transform.rotation = rot;
            instance.StartCoroutine(instance.SelfDestruct(fx, selfDestructTime));
        }

        public static void PlayFX(string fxName, Transform parent, float selfDestructTime) {
            GameObject fx = instance.GetFxFromPool(fxName);
            fx.SetActive(true);
            fx.transform.SetParent(parent);
            fx.transform.localPosition = Vector3.zero;
            fx.transform.localRotation = Quaternion.identity;
            instance.StartCoroutine(instance.SelfDestruct(fx, selfDestructTime));
        }

        public static void PlayFX(string fxName, Transform parent) {
            GameObject fx = instance.GetFxFromPool(fxName);
            instance.RemoveFxFromPool(fx, fxName);
            fx.SetActive(true);
            fx.transform.SetParent(parent);
            fx.transform.localPosition = Vector3.zero;
            fx.transform.localRotation = Quaternion.identity;
        }

        public static void PlayFX(string fxName, Transform parent, Vector3 localPos, float selfDestructTime) {
            GameObject fx = instance.GetFxFromPool(fxName);
            instance.RemoveFxFromPool(fx, fxName);
            fx.SetActive(true);
            fx.transform.SetParent(parent);
            fx.transform.localPosition = localPos;
            fx.transform.localRotation = Quaternion.identity;
            instance.StartCoroutine(instance.SelfDestruct(fx, selfDestructTime));
        }

        public static void PlayFX(string fxName, Transform parent, Vector3 localPos, Vector3 localScale, float selfDestructTime) {
            GameObject fx = instance.GetFxFromPool(fxName);
            instance.RemoveFxFromPool(fx, fxName);
            fx.SetActive(true);
            fx.transform.SetParent(parent);
            fx.transform.localPosition = localPos;
            fx.transform.localScale = localScale;
            fx.transform.localRotation = Quaternion.identity;
            instance.StartCoroutine(instance.SelfDestruct(fx, selfDestructTime));
        }

        public static void PlayCompleteFX() {

        }

        private GameObject GetFxFromPool(string fxName) {
            foreach (GameObject fx in fxPoolDict[fxName]) {
                if (!fx.activeInHierarchy)
                    return fx;
            }

            try {
                CreateFX(fxPrefabs.Find(fx => fx.prefabType == fxName));
                return GetFxFromPool(fxName);
            }
            catch (System.OverflowException e) {
                Debug.LogWarning("stack overflow : " + e);
                throw;
            }
        }

        private void RemoveFxFromPool(GameObject fx, string fxName) {
            fxPoolDict[fxName].Remove(fx);
        }

        private void CreateFX(FXPrefab fxPrefab) {
            GameObject fx = Instantiate(fxPrefab.prefabObj, fxParent);
            fx.SetActive(false);
            fxPoolDict[fxPrefab.prefabType].Add(fx);
        }

        private IEnumerator SelfDestruct(GameObject fx, float time) {
            yield return new WaitForSeconds(time);
            fx.SetActive(false);
            fx.transform.parent = fxParent;
        }

    } 
}
