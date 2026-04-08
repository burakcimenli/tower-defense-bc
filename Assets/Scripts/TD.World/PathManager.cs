using UnityEngine;
using System.Collections.Generic;
using TD.Core;

namespace TD.World {
    public class PathManager : MonoBehaviour {
        [SerializeField] private LineRenderer line;
        private Vector3[] points;
        private float[] segmentLengths;
        private float totalPathLength;
        [HideInInspector] public List<Enemy> activeEnemies = new List<Enemy>();

        public void Init() {
            SetupPath();
        }

        public void AddToActive(Enemy enemy) {
            activeEnemies.Add(enemy);
        }

        public void RemoveFromActive(Enemy enemy) {
            activeEnemies.Remove(enemy);

            if(activeEnemies.Count < 1 && SpawnManager.instance.hasSpawnFinished) {
                GameEvents.OnLevelCompleted?.Invoke(true);
            }
        }

        private void SetupPath() {
            points = new Vector3[line.positionCount];
            Vector3[] localPoints = new Vector3[line.positionCount];
            line.GetPositions(localPoints);

            segmentLengths = new float[localPoints.Length - 1];
            totalPathLength = 0;

            for (int i = 0; i < localPoints.Length; i++) {
                // convert local to world
                points[i] = line.transform.TransformPoint(localPoints[i]);
            }

            for (int i = 0; i < points.Length - 1; i++) {
                segmentLengths[i] = Vector3.Distance(points[i], points[i + 1]);
                totalPathLength += segmentLengths[i];
            }
        }

        private void Update() {
            // iterate backwards so we can safely remove enemies if they reach the end
            for (int i = activeEnemies.Count - 1; i >= 0; i--) {
                UpdateEnemyMovement(activeEnemies[i]);
            }
        }

        private void UpdateEnemyMovement(Enemy enemy) {
            // enemy's progress (0 -> 1)
            enemy.pathProgress += (enemy.data.moveSpeed * Time.deltaTime) / totalPathLength;

            // sample the LineRenderer points to find new 3D position
            enemy.transform.position = GetPositionOnPath(enemy.pathProgress);

            // get look direction
            Vector3 nextPos = GetPositionOnPath(enemy.pathProgress + 0.01f); // Look slightly ahead
            Vector3 direction = (nextPos - enemy.transform.position).normalized;
            if (direction != Vector3.zero)
                enemy.transform.rotation = Quaternion.LookRotation(direction);

            // check for completion
            if (enemy.pathProgress >= 1f) {
                EnemyAttack(enemy);
            }
        }

        private Vector3 GetPositionOnPath(float progress) {
            float targetDist = progress * totalPathLength;
            float cumulativeDist = 0;

            for (int i = 0; i < segmentLengths.Length; i++) {
                if (cumulativeDist + segmentLengths[i] >= targetDist) {
                    // segment found -> calculate how far along THIS segment we are.
                    float localDist = targetDist - cumulativeDist;
                    float t = localDist / segmentLengths[i];

                    // return interpolated position
                    return Vector3.Lerp(points[i], points[i + 1], t);
                }
                cumulativeDist += segmentLengths[i];
            }
            return points[points.Length - 1];
        }

        private void EnemyAttack(Enemy enemy) {
            enemy.Die();
            Base.instance.TakeDamage(enemy.data.damage);
        }
    } 
}
