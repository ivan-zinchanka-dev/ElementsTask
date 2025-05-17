using ElementsTask.Core.Models;
using ElementsTask.Presentation.Models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ElementsTask.Presentation.Views
{
    public class BlockView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] 
        private SpriteRenderer _spriteRenderer;

        [ShowInInspector] 
        private Vector2Int _gridPosition;
        
        [field:SerializeField] 
        public UnityEvent<BlockView> OnSelected { get; private set; }

        public Vector2Int GridPosition => _gridPosition;
        
        private Block _block;
        
        public BlockView SetModel(Block block)
        {
            _block = block;
            return this;
        }

        public BlockView SetGridPosition(Vector2Int gridPosition)
        {
            _gridPosition = gridPosition;
            return this;
        }

        public BlockView SetSortingOrder(int sortingOrder)
        {
            _spriteRenderer.sortingOrder = sortingOrder;
            return this;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSelected?.Invoke(this);
        }

        public BlockSwapData GetSwapData()
        {
            return new BlockSwapData(transform.position, _gridPosition);
        }
    }
}