using UnityEngine;
using TD.Core;
using TD.UI;

namespace TD.Core {
	public class Unit : Entity {
		[SerializeField] protected HPBar hpBar;
        [HideInInspector] public int hp;
        public int maxHP;

        public virtual void TakeDamage(int dmg) {
			hp -= dmg;
			UpdateHP();
		}

		public void UpdateHP() {
            hpBar.SetFill((float)hp / maxHP);
        }
    }
}