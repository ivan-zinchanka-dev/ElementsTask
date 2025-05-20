using System;
using Cysharp.Threading.Tasks;
using ElementsTask.Core.Enums;
using ElementsTask.Core.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ElementsTask.Presentation.BlockFieldCore.Views
{
    public class BlockView : MonoBehaviour, IPointerDownHandler
    {
        private static readonly int StateParam = Animator.StringToHash("BlockState");

        [SerializeField] 
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private Animator _animator;
        [SerializeField] 
        private AnimationClip _destroyAnimClip;
        
        [field:SerializeField] 
        public UnityEvent<BlockView> OnSelected { get; private set; }
        
        private Block _block;
        
        public int SortingOrder
        {
            get =>_spriteRenderer.sortingOrder;
            set => _spriteRenderer.sortingOrder = value;
        }
        
        public BlockType Type => _block.Type;
        
        public BlockView SetModel(Block block)
        {
            _block = block;
            _block.State = BlockState.Idle;
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

        private void SwitchState(BlockState newState)
        {
            _block.State = newState;
            _animator.SetInteger(StateParam, (int)newState);
        }

        public async UniTask SelfDestroyAsync()
        {
            SwitchState(BlockState.Destroy);
            
            if (_destroyAnimClip != null)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_destroyAnimClip.length), DelayType.DeltaTime);
            }
            
            OnSelected.RemoveAllListeners();
            Destroy(gameObject);
            
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}