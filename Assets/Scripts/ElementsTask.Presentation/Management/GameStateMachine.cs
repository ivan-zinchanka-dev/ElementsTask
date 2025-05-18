using System.Threading.Tasks;
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
        public async Task RestartAsync()
        {
            await _blockFieldView.ReInitialize();
        }

        [Button]
        public async Task NextAsync()
        {
            _progressService.CurrentLevelIndex++;
            await RestartAsync();
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