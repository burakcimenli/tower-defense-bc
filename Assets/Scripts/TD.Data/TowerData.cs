using UnityEngine;

namespace TD.Data {
	[CreateAssetMenu(fileName = "TowerData", menuName = "Data/TowerData")]
	public class TowerData : ScriptableObject {
        [Tooltip("Damage of the tower")]
        public int damage = 100;

        [Tooltip("Range of the tower")]
        public float range = 2;

		[Tooltip("Is this tower can target multiple enemies at once?")]
		public bool isMultiTarget;

        [Tooltip("Splash damage range (0 means no area of effect/splash damage")]
        public float aoeRange = 0;

        [Tooltip("Shot per second")]
		public float fireRate = 1;
    } 
}
