using System;
using System.Threading.Tasks;
using ElementsTask.Data.PlayerProgression.Models;
using ElementsTask.Data.PlayerProgression.Services;
using ElementsTask.Presentation.BlockFieldCore.Views;
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
            _blockFieldView.Cleanup();
            
            PlayerProgress playerProgress = await _progressService.GetPlayerProgressAsync();
            playerProgress.BlockFieldState.Clear();
            await playerProgress.SaveAsync();

            await _blockFieldView.InitializeAsync();
        }

        [Button]
        public async Task NextAsync()
        {
            _blockFieldView.Cleanup();
            
            PlayerProgress playerProgress = await _progressService.GetPlayerProgressAsync();
            playerProgress.BlockFieldState.Clear();
            playerProgress.CurrentLevelIndex++;
            await playerProgress.SaveAsync();
            
            await _blockFieldView.InitializeAsync();
        }

        private async void Awake()
        {
            await _blockFieldView.InitializeAsync();
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

        private void OnDestroy()
        {
            _blockFieldView.Cleanup();
        }
    }
}