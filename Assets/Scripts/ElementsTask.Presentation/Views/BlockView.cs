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
        
        [SerializeField]
        private UnityEvent<BlockView> OnSelected;
        
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