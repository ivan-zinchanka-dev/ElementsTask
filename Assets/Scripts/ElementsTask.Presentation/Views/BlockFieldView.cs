using UnityEngine;

namespace ElementsTask.Presentation.Views
{
    public class BlockFieldView : MonoBehaviour
    {
        private BlockView[,] _blockViews;

        public BlockFieldView Initialize(BlockView[,] blockViews)
        {
            _blockViews = blockViews;

            foreach (BlockView blockView in _blockViews)
            {
                blockView.OnSelected.AddListener(OnBlockViewSelected);
            }
            
            return this;
        }


        private void OnBlockViewSelected(BlockView selectedView)
        {
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                // make swap pair
            }
        }
    }
}