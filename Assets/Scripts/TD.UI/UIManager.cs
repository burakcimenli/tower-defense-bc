using UnityEngine;
using TD.Core;
using TMPro;
using System;

namespace TD.UI {
    public class UIManager : MonoBehaviour {

        [SerializeField] private GameObject panel_start, panel_complete, panel_fail;
        [SerializeField] private GameObject restartButton;
        [SerializeField] private TextMeshProUGUI scoreText;

        private void OnEnable() {
            GameEvents.OnLevelCompleted += OnLevelCompleted;
            GameEvents.OnScoreUpdated += OnScoreUpdated;
        }

        public void OnScoreUpdated(int score) {
            scoreText.SetText("<size=-20>SCORE</size> " + score);
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