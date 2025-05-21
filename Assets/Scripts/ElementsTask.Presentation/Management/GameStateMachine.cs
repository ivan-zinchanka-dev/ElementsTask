using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ElementsTask.Data.PlayerProgression;
using ElementsTask.Data.PlayerProgression.Models;
using ElementsTask.Data.PlayerProgression.Services;
using ElementsTask.Presentation.Balloons;
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
        [Inject] 
        private BalloonsManager _balloonsManager;
        
        private CancellationTokenSource _balloonsStoppingTokenSource = new ();
        
        
        [Button]
        public async Task RestartAsync()
        {
            /*_balloonsStoppingTokenSource.Cancel();
            _balloonsStoppingTokenSource.Dispose();
            _balloonsManager.Dispose();*/
            
            PlayerProgress playerProgress = await _progressService.GetPlayerProgressAsync();
            playerProgress.BlockFieldState.Clear();
            await playerProgress.SaveAsync();
            
            await _blockFieldView.ReInitialize();
            
            /*_balloonsStoppingTokenSource = new CancellationTokenSource();
            _balloonsManager.StartAsync(transform, _balloonsStoppingTokenSource.Token).Forget();*/
        }

        [Button]
        public async Task NextAsync()
        {
            PlayerProgress playerProgress = await _progressService.GetPlayerProgressAsync();
            playerProgress.CurrentLevelIndex++;
            await playerProgress.SaveAsync();
            
            await RestartAsync();
        }

        private async void Awake()
        {
            await _blockFieldView.InitializeAsync();

            //_balloonsManager.StartAsync(transform, _balloonsStoppingTokenSource.Token).Forget();
        }

        private void OnDestroy()
        {
            /*_balloonsStoppingTokenSource.Cancel();
            _balloonsStoppingTokenSource.Dispose();*/
            
            _blockFieldView.Cleanup();
        }
    }
}