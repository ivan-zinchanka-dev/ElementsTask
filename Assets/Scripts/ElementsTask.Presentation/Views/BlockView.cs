using ElementsTask.Core.Models;
using UnityEngine;

namespace ElementsTask.Presentation.Views
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] 
        private SpriteRenderer _spriteRenderer;
        
        private Block _block;

        public BlockView SetModel(Block block)
        {
            _block = block;
            return this;
        }

        public BlockView SetSortingOrder(int sortingOrder)
        {
            _spriteRenderer.sortingOrder = sortingOrder;
            return this;
        }

    }
}