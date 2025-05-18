using ElementsTask.Presentation.Management;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace ElementsTask.Presentation.UI
{
    public class HeadUpDisplay : MonoBehaviour
    {
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Button _nextButton;
        
        [Inject]
        private GameStateMachine _gameStateMachine;
        
        private void OnEnable()
        {
            _restartButton.onClick.AddListener(RestartLevel);
            _nextButton.onClick.AddListener(NextLevel);
        }

        private async void RestartLevel() => await _gameStateMachine.RestartAsync();
        private async void NextLevel() => await _gameStateMachine.NextAsync();
        
        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(RestartLevel);
            _nextButton.onClick.RemoveListener(NextLevel);
        }
    }
}