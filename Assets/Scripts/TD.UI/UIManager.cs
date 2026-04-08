using UnityEngine;
using TD.Core;
using TMPro;

namespace TD.UI {
    public class UIManager : MonoBehaviour {

        [SerializeField] private GameObject panel_start, panel_complete, panel_fail;
        [SerializeField] private GameObject restartButton;

        private void OnEnable() {
            GameEvents.OnLevelCompleted += OnLevelCompleted;
        }

        public void OnStartClicked() {
            GameEvents.OnGameStarted?.Invoke();
            panel_start.SetActive(false);
        }

        public void OnRestartClicked() {
            GameEvents.OnGameRestarted?.Invoke();
        }

        public void OnLevelCompleted(bool isWin) {
            if (isWin)
                panel_complete.SetActive(true);
            else
                panel_fail.SetActive(true);

            restartButton.SetActive(true);
        }

        private void OnDisable() {
            GameEvents.OnLevelCompleted -= OnLevelCompleted;
        }
    }
}