using System;
using UnityEngine;

namespace TD.Core {
    public static class GameEvents {
        public static Action OnGameStarted;
        public static Action OnGameRestarted;
        public static Action<bool> OnLevelCompleted;
        public static Action<bool> OnGamePaused;
        public static Action<int> OnScoreUpdated;
    }
}
