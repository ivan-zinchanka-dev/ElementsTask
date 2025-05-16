using ElementsTask.Core.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ElementsTask.Presentation.Views
{
    public class BlockView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] 
        private SpriteRenderer _spriteRenderer;

        [field:SerializeField] 
        public UnityEvent<BlockView> OnSelected { get; private set; }

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

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnSelected");
            
            OnSelected?.Invoke(this);
        }
    }
}