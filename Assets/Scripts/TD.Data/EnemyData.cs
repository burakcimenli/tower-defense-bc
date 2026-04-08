using UnityEngine;

namespace TD.Data {
    public enum EnemyType {
        Croc,
        Armadillo
    }

    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Data/Enemy Data")]
	public class EnemyData : ScriptableObject {
        [Tooltip("This enemy unit's type")]
        public EnemyType type;

        [Tooltip("This enemy unit's max health")]
		public int maxHP = 100;

        [Tooltip("This enemy unit's damage")]
        public int damage = 1;

        [Tooltip("This enemy unit's movement speed")]
        public float moveSpeed = 1;
	}

}