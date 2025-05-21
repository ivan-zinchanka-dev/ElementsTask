using ElementsTask.Data.BlockFieldCore.Models;
using ElementsTask.Presentation.BlockFieldCore.Containers;
using ElementsTask.Presentation.BlockFieldCore.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ElementsTask.Presentation.BlockFieldCore.Services.Factories
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