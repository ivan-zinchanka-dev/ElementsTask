using ElementsTask.Core.Models;
using ElementsTask.Presentation.BlockFieldCore.Models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ElementsTask.Presentation.BlockFieldCore.Views
{
    public class BlockView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] 
        private SpriteRenderer _spriteRenderer;

        [ShowInInspector] 
        private Vector2Int _gridPosition;
        
        [field:SerializeField] 
        public UnityEvent<BlockView> OnSelected { get; private set; }
        
        private Block _block;
        
        public int SortingOrder
        {
            get =>_spriteRenderer.sortingOrder;
            set => _spriteRenderer.sortingOrder = value;
        }
        
        public BlockType Type => _block.Type;
        public bool IsEmpty => Type == BlockType.Empty;
        
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

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSelected?.Invoke(this);
        }

        public BlockSwapData GetSwapData()
        {
            return new BlockSwapData(transform.position, _spriteRenderer.sortingOrder);
        }
        
        /*public override bool Equals(object obj)
        {
            return Equals(obj as BlockView);
        }

        public bool Equals(BlockView other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return _block.Type == other._block.Type;
        }

        public override int GetHashCode()
        {
            return _block.Type.GetHashCode();
        }*/
    }
}