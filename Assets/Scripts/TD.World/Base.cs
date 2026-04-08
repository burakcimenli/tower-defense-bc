using UnityEngine;
using TD.Core;

namespace TD.World {
    public class Base : Unit {

        public static Base instance;
        private bool isDestroyed;

        private void Awake() {
            if(instance != null) {
                Destroy(this);
                return;
            }

            instance = this;
        }

        private void OnEnable() {
            GameEvents.OnGameStarted += Init;
        }

        private void Init() {
            hp = maxHP;
            UpdateHP();
        }

        public override void TakeDamage(int dmg) {
            base.TakeDamage(dmg);

            if(hp < 1 && !isDestroyed) {
                isDestroyed = true;
                gameObject.SetActive(false);
                GameEvents.OnLevelCompleted?.Invoke(false);
            }
        }

        private void OnDisable() {
            GameEvents.OnGameStarted -= Init;
        }
    } 
}
