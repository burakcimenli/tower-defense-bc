using UnityEngine;
using TD.Core;
using TD.Data;

namespace TD.World {
    public class Enemy : Unit {
        public EnemyType visualType;

        // progress on the path to the ally base
        [HideInInspector] public float pathProgress = 0;

        // data for this enemy
        [HideInInspector] public EnemyData data;

        // Init enemy upon spawning
        public void Init(EnemyData data) {
            maxHP = data.maxHP;
            hp = maxHP;
            this.data = data;

            UpdateHP();
        }
    } 
}
