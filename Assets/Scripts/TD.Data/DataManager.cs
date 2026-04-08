using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TD.Core;

namespace TD.Data {
    public class DataManager : MonoBehaviour {
        private static EnemyData[] enemyData;

        private void OnEnable() {
            GameEvents.OnGameStarted += LoadData;
        }

        public void LoadData() {
            if(enemyData == null) {
                enemyData = Resources.LoadAll<EnemyData>("Data/Enemies");
                print("FOUND enemy data? count : " + enemyData.Length);
            }
        }

        public static EnemyData GetEnemyData(EnemyType type) {
            var data = enemyData.Where(d => d.type == type).FirstOrDefault();
            if (data == null)
                Debug.LogError("--- Enemy data " + type.ToString() + " CANNOT be found, check if you have enemy data in your Data folder.");
            return data;

        }

        private void OnDisable() {
            GameEvents.OnGameStarted -= LoadData;
        }
    }

}