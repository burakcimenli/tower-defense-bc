using UnityEngine;
using TD.Core;
using TD.Data;
using TD.Util;
using TD.UI;

namespace TD.World {
    public class Enemy : Unit {
        public EnemyType visualType;

        // progress on the path to the ally base
        [HideInInspector] public float pathProgress = 0;

        // data for this enemy
        [HideInInspector] public EnemyData data;

        // Init enemy upon spawning
        public void Init(EnemyData data) {
            pathProgress = 0;
            maxHP = data.maxHP;
            hp = maxHP;
            this.data = data;

            UpdateHP();
        }

        public override void TakeDamage(int dmg) {
            base.TakeDamage(dmg);

            if(hp < 1) {
                // Died to a weapon
                MainManager.AddScore(5);
                Die();
            }

        }

        public void Die() {
            SpawnManager.instance.pathManager.RemoveFromActive(this);
            gameObject.SetActive(false);
            FXManager.PlayFX("Death", transform.position, 2);
        }
    } 
}
