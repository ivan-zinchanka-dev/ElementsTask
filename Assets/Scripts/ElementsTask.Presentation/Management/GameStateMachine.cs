using ElementsTask.Core.Services;
using ElementsTask.Presentation.Views;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace ElementsTask.Presentation.Management
{
    public class GameStateMachine : MonoBehaviour
    {
        [Inject] 
        private IPlayerProgressService _progressService;
        
        [Inject] 
        private BlockFieldView _blockFieldView;

        [Button]
        private void Restart()
        {
            _blockFieldView.ReInitialize();
        }

        [Button]
        private void Next()
        {
            _progressService.CurrentLevelIndex++;
            Restart();
        }

        private async void Awake()
        {
            await _blockFieldView.InitializeAsync();
        }

        private void OnDestroy()
        {
            _blockFieldView.Cleanup();
        }
    }
}