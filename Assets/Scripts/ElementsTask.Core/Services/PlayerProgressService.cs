using UnityEngine;

namespace ElementsTask.Core.Services
{
    public class PlayerProgressService : IPlayerProgressService
    {
        private const string CurrentLevelKey = "current_level";
        private const int DefaultLevelIndex = 0;
        private int? _currentLevelIndex;
        
        public int CurrentLevelIndex
        {
            get
            {
                _currentLevelIndex ??= PlayerPrefs.GetInt(CurrentLevelKey, DefaultLevelIndex);
                return _currentLevelIndex.Value;
            }

            set
            {
                _currentLevelIndex = value;
                PlayerPrefs.SetInt(CurrentLevelKey, _currentLevelIndex.Value);
                PlayerPrefs.Save();
            }

        }
    }
}