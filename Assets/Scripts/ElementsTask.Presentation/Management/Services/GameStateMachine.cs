using System.Threading.Tasks;
using ElementsTask.Common.Components.View;
using ElementsTask.Data.PlayerProgression.Models;
using ElementsTask.Data.PlayerProgression.Services;
using ElementsTask.Presentation.BlockFieldCore.Views;
using ElementsTask.Presentation.Management.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace ElementsTask.Presentation.Management.Services
{
    public class GameStateMachine : MonoBehaviour
    {
        [SerializeField]
        private AspectRatioScaler _foregroundAspectRatioScaler;
        
        [Inject] 
        private IPlayerProgressService _progressService;
        [Inject] 
        private BlockFieldView _blockFieldView;

        private GameState _state = GameState.Loading;
        
        [Button]
        public async Task RestartAsync()
        {
            if (_state == GameState.Loading)
            {
                return;
            }

            _state = GameState.Loading;
            _blockFieldView.Cleanup();
            
            PlayerProgress playerProgress = await _progressService.GetPlayerProgressAsync();
            playerProgress.BlockFieldState.Clear();
            await playerProgress.SaveAsync();

            await _blockFieldView.InitializeAsync();
            _state = GameState.Playing;
        }

        [Button]
        public async Task NextAsync()
        {
            if (_state == GameState.Loading)
            {
                return;
            }

            _state = GameState.Loading;
            _blockFieldView.Cleanup();
            
            PlayerProgress playerProgress = await _progressService.GetPlayerProgressAsync();
            playerProgress.BlockFieldState.Clear();
            playerProgress.CurrentLevelIndex++;
            await playerProgress.SaveAsync();
            
            await _blockFieldView.InitializeAsync();
            _state = GameState.Playing;
        }

        private async void Awake()
        {
            await _blockFieldView.InitializeAsync();
            _foregroundAspectRatioScaler.Scale();
            
            _state = GameState.Playing;
        }

        private void OnEnable()
        {
            _blockFieldView.OnAllBlocksDestroyed.AddListener(OnAllBlocksDestroyed);
        }
        
        private async void OnAllBlocksDestroyed()
        {
            await NextAsync();
        }
        
        private void OnDisable()
        {
            _blockFieldView.OnAllBlocksDestroyed.RemoveListener(OnAllBlocksDestroyed);
        }
    }
}