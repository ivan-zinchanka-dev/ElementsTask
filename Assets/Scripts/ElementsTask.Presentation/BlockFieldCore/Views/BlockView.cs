using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ElementsTask.Data.BlockFieldCore.Enums;
using ElementsTask.Data.BlockFieldCore.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace ElementsTask.Presentation.BlockFieldCore.Views
{
    public class BlockView : MonoBehaviour, IPointerDownHandler
    {
        private static readonly int StateParam = Animator.StringToHash("BlockState");
        private static readonly int IdleStateHash = Animator.StringToHash("Idle");
        
        [SerializeField] 
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private Animator _animator;
        [SerializeField] 
        private AnimationClip _destroyAnimClip;
        
        [field:SerializeField] 
        public UnityEvent<BlockView> OnSelected { get; private set; }
        
        private Block _block;
        private readonly CancellationTokenSource _destructionStoppingTokenSource = new();
        
        public int SortingOrder
        {
            get =>_spriteRenderer.sortingOrder;
            set => _spriteRenderer.sortingOrder = value;
        }
        
        public BlockType Type => _block.Type;
        public Block Model => _block;
        public BlockState State { get; private set; } = BlockState.Idle;
        
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

        public BlockView RandomizeIdleAnimation()
        {
            if (State == BlockState.Idle)
            {
                _animator.Play(IdleStateHash, 0, Random.Range(0f, 1f));
            }
            
            return this;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (State == BlockState.Idle)
            {
                OnSelected?.Invoke(this);
            }
        }
        
        public void OnFall()
        {
            SwitchState(BlockState.Fall);
        }

        public void OnLand()
        {
            SwitchState(BlockState.Idle);
        }

        public void DestroyInstantly()
        {
            if (State == BlockState.Destroy)
            {
                _destructionStoppingTokenSource.Cancel();
            }
            
            Destroy(gameObject);
            _destructionStoppingTokenSource.Dispose();
        }

        public async UniTask DestroyAnimatedAsync()
        {
            SwitchState(BlockState.Destroy);
            
            if (_destroyAnimClip != null)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_destroyAnimClip.length), DelayType.DeltaTime, 
                    cancellationToken:_destructionStoppingTokenSource.Token);
            }

            if (_destructionStoppingTokenSource.IsCancellationRequested)
            {
                return;
            }

            OnSelected.RemoveAllListeners();
            Destroy(gameObject);
            
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        
        private void SwitchState(BlockState newState)
        {
            State = newState;
            _animator.SetInteger(StateParam, (int)newState);
        }
    }
}