using System;
using UnityEngine;

namespace TD.Util {
    [Serializable]
    public class FXPrefab {
        public GameObject prefabObj;
        public int prefabPoolCount;
        public string prefabType;

        public FXPrefab(GameObject prefabObj, int prefabPoolCount, string prefabType) {
            this.prefabObj = prefabObj;
            this.prefabPoolCount = prefabPoolCount;
            this.prefabType = prefabType;
        }
    }

}