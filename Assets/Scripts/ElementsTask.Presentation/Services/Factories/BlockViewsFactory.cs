using ElementsTask.Core.Models;
using ElementsTask.Presentation.Containers;
using ElementsTask.Presentation.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ElementsTask.Presentation.Services.Factories
{
    public class BlockViewsFactory : MonoBehaviour
    {
        [SerializeField] 
        private BlockViewsContainer _viewsContainer;
        
        [Inject] 
        private IObjectResolver _diContainer;
        
        public BlockView CreateBlockView(BlockType blockType, Transform parent = null, bool worldPositionStays = false)
        {
            BlockView originalView = _viewsContainer.GetViewByType(blockType);

            if (originalView != null)
            {
                return _diContainer.Instantiate(originalView, parent, worldPositionStays);
            }

            return null;
        }
    }
}