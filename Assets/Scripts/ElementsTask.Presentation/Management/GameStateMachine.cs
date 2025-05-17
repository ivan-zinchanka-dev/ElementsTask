using ElementsTask.Presentation.Views;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace ElementsTask.Presentation.Management
{
    public class GameStateMachine : MonoBehaviour
    {
        [Inject] 
        private BlockFieldView _blockFieldView;

        [Button]
        private void Restart()
        {
            _blockFieldView.ReInitialize();
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