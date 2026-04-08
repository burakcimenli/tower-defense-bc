using System.Collections.Generic;
using System.Linq;
using TD.Core;
using TD.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace TD.World {
    public class SpawnManager : MonoBehaviour {

        public static SpawnManager instance;

        [Tooltip("Path manager that handles movement of the spawned enemies")]
        public PathManager pathManager;

        [Tooltip("Spawn rate of the spawner")]
        [SerializeField] private float spawnRate;

        [Tooltip("Spawn order and number of the enemies")]
        [SerializeField] private List<SpawnData> spawnOrder;

        [Tooltip("Enemy prefabs")]
        [SerializeField] private List<Enemy> enemyPrefabs;

        [SerializeField] private List<Enemy> enemyPool;
        private bool isSpawnActive;
        private float timer;

        private void Awake() {
            if(instance != null) {
                Destroy(this);
                return;
            }

            instance = this;
        }

        private void OnEnable() {
            GameEvents.OnGameStarted += StartSpawn;
        }

        private void Start() {
            pathManager.Init();
        }

        private void StartSpawn() {
            print("Starting to spawn");
            enemyPool = new List<Enemy>();
            isSpawnActive = true;
        }


        void Update() {
            if (isSpawnActive) {
                timer -= Time.deltaTime;
                if(timer <= 0) {
                    float cooldown = 1 / spawnRate;
                    timer = cooldown;
                    SpawnNext();
                }
            }
        }

        private void SpawnNext() {
            if(spawnOrder.Count > 0) {
                var spawnData = spawnOrder[0];

                var enemy = GetFromPool(spawnData.enemyType);
                PrepEnemy(enemy);
                Debug.Log("Spawned", enemy);
                pathManager.AddToActive(enemy);

                spawnData.count--;
                if(spawnData.count < 1) {
                    spawnOrder.Remove(spawnOrder[0]);
                }
            }

            else {
                // finished
                isSpawnActive = false;
            }
        }

        private Enemy GetFromPool(EnemyType type) {
            var availableEnemy = enemyPool.Where(e => e.visualType == type && !e.gameObject.activeInHierarchy).FirstOrDefault();
            if(availableEnemy == null) {
                var enemy = SpawnEnemy(type);
                enemyPool.Add(enemy);
                return enemy;
            }

            availableEnemy.gameObject.SetActive(true);
            return availableEnemy;
        }

        private void PrepEnemy(Enemy enemy) {
            enemy.Init(DataManager.GetEnemyData(enemy.visualType));
        }

        private Enemy SpawnEnemy(EnemyType type) {
            Enemy prefab = enemyPrefabs.Where(e => e.visualType == type).FirstOrDefault();
            if(prefab == null) {
                Debug.LogError("---- CANNOT FIND ENEMY PREFAB, Check your enemy prefabs list to confirm that your requested type exists. ----");
                return null;
            }

            Enemy spawned = Instantiate(prefab);
            return spawned;
        }

        private void OnDisable() {
            GameEvents.OnGameStarted -= StartSpawn;

        }
    }
}