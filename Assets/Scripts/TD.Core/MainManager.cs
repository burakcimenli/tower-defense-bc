using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TD.Core {
    public class MainManager : MonoBehaviour {
        private void OnEnable() {
            GameEvents.OnGameStarted += StartGame;
            GameEvents.OnGameRestarted += Restart;
            GameEvents.OnLevelCompleted += CompleteLevel;
            GameEvents.OnGamePaused += SetPause;
        }

        public void StartGame() {
            // Game started
        }

        public void Restart() {
            SceneManager.LoadScene(0);
        }

        public void CompleteLevel(bool isWin) {
            // Game completed
        }

        public void SetPause(bool isPaused) {
            Time.timeScale = isPaused ? 0 : 1;
        }

        private void OnDisable() {
            GameEvents.OnGameStarted -= StartGame;
            GameEvents.OnGameRestarted -= Restart;
            GameEvents.OnLevelCompleted -= CompleteLevel;
            GameEvents.OnGamePaused -= SetPause;
        }
    }

}