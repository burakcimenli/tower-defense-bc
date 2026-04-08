using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TD.Data;
using TD.Util;
using UnityEditor;
using UnityEngine;

namespace TD.World {
    [SelectionBase]
    public class Tower : MonoBehaviour {

        public TowerData data;
        [SerializeField] private Transform weaponHead;
        [SerializeField] private GameObject boltPrefab;
        [SerializeField] private ParticleSystem[] muzzleFX;
        private List<Enemy> targets = new List<Enemy>();
        private Enemy bestEnemy;

        private float shootTimer; // update timer for shooting
        private float searchTimer; // update timer for searching (targets)
        private float targetUpdateInterval = 0.1f;  // update every 0.1 sec

        private List<Transform> boltPool = new List<Transform>();

        private void Update() {
            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0f) {
                UpdateTarget();
                searchTimer = targetUpdateInterval;
            }

            if(bestEnemy != null) {
                weaponHead.LookAt(bestEnemy.transform);

                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0) {
                    shootTimer = 1 / data.fireRate;
                    Shoot();
                }
            }
        }

        // Search and update targets
        private void UpdateTarget() {
            targets.Clear();

            foreach (var enemy in SpawnManager.instance.pathManager.activeEnemies) {
                float distSq = (enemy.transform.position - transform.position).sqrMagnitude;
                if (distSq <= data.range * data.range) {
                    targets.Add(enemy);
                }
            }

            // sort the list so targets[0] is always the best (furthest)
            if (targets.Count > 0) {
                targets.Sort((a, b) => b.pathProgress.CompareTo(a.pathProgress));
                bestEnemy = targets[0];
            }
            else {
                bestEnemy = null;
            }
        }


        private void Shoot() {
            int targetCount = data.isMultiTarget ? targets.Count : 1;

            for(int i = 0; i < targetCount; i++) {
                var bolt = GetBoltFromPool();
                bolt.position = weaponHead.position;
                bolt.rotation = weaponHead.rotation;
                StartCoroutine(ShootBolt(bolt, targets[i]));
            }

            foreach (var fx in muzzleFX)
                fx.Play();
        }

        // Pooling operations
        private Transform GetBoltFromPool() {
            var bolt = boltPool.Where(b => !b.gameObject.activeInHierarchy).FirstOrDefault();
            if(bolt == null) {
                bolt = SpawnBolt();
                boltPool.Add(bolt);
            }

            bolt.gameObject.SetActive(true);
            return bolt;
        }

        private Transform SpawnBolt() {
            return Instantiate(boltPrefab).transform;
        }

        private IEnumerator ShootBolt(Transform bolt, Enemy enemy) {
            Vector3 startPos = bolt.position;
            float speed = data.projectileSpeed;
            float duration = Vector3.Distance(transform.position, enemy.transform.position) / speed;
            float counter = 0;
            while(counter < duration) {
                float t = counter / duration;
                bolt.position = Vector3.Lerp(startPos, enemy.transform.position, t);
                counter += Time.deltaTime;
                yield return null;
            }

            if (data.aoeRange > 0) {
                SplashDamage(bolt.position);
            }
            else {
                FXManager.PlayFX("Bolt", bolt.position, 1);
                enemy.TakeDamage(data.damage);
            }
            bolt.gameObject.SetActive(false);
        }

        private void SplashDamage(Vector3 position) {
            FXManager.PlayFX("Explosion", position, 2);
            for(int i = SpawnManager.instance.pathManager.activeEnemies.Count - 1; i >= 0; i--) {
                var enemy = SpawnManager.instance.pathManager.activeEnemies[i];
                float distSq = (enemy.transform.position - position).sqrMagnitude;
                if (distSq <= data.aoeRange * data.aoeRange) {
                    enemy.TakeDamage(data.damage);
                }
            }
        }
    }

}