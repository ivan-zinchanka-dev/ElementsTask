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
        
        private void Awake()
        {
            _blockFieldView.Initialize();
        }

        private void OnDestroy()
        {
            _blockFieldView.Cleanup();
        }
    }
}