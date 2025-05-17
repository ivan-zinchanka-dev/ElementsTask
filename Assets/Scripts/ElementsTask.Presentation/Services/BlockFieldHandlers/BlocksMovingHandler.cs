using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ElementsTask.Presentation.Enums;
using ElementsTask.Presentation.Models;
using ElementsTask.Presentation.Views;
using UnityEngine;

namespace ElementsTask.Presentation.Services.BlockFieldHandlers
{
    public class BlocksMovingHandler : IDisposable
    {
        private readonly Vector2Int _fieldSize;
        private readonly List<BlockView> _blocks;
        private readonly float _movingDuration;
        
        private Tween _movingTween;
        
        public BlocksMovingHandler(Vector2Int fieldSize, List<BlockView> blocks, float movingDuration = 0.15f)
        {
            _fieldSize = fieldSize;
            _blocks = blocks;
            _movingDuration = movingDuration;
        }
        
        public async UniTask<bool> TryMoveBlockAsync(BlockView selectedBlock, Vector3 targetPosition)
        {
            if (_movingTween.IsActive())
            {
                return false;
            }
            
            BlockView pair = GetSwapPair(selectedBlock, 
                GetMovingDirection(targetPosition - selectedBlock.transform.position));

            if (pair != null)
            {
                _movingTween = BeginSwap(selectedBlock, pair);
                await _movingTween.ToUniTask();
            }
            
            return true;
        }

        private static BlockMovingDirection GetMovingDirection(Vector3 inputDirection)
        {
            if (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y))
            {
                return inputDirection.x > 0f ? BlockMovingDirection.Right : BlockMovingDirection.Left;
            }
            else
            {
                return inputDirection.y > 0f ? BlockMovingDirection.Up : BlockMovingDirection.Down;
            }
        }
        
        private BlockView GetSwapPair(BlockView origin, BlockMovingDirection swapDirection)
        {
            if (swapDirection == BlockMovingDirection.Up)
            {
                if (origin.GridPosition.y < _fieldSize.y - 1)
                {
                    return _blocks.Find(block => 
                        block.GridPosition.x == origin.GridPosition.x && 
                        block.GridPosition.y == origin.GridPosition.y + 1);
                }

                return null;
            }
            else if (swapDirection == BlockMovingDirection.Down)
            {
                if (origin.GridPosition.y > 0)
                {
                    return _blocks.Find(block => 
                        block.GridPosition.x == origin.GridPosition.x && 
                        block.GridPosition.y == origin.GridPosition.y - 1);
                }

                return null;
            }
            else if (swapDirection == BlockMovingDirection.Right)
            {
                if (origin.GridPosition.x < _fieldSize.x - 1)
                {
                    return _blocks.Find(block => 
                        block.GridPosition.x == origin.GridPosition.x + 1 && 
                        block.GridPosition.y == origin.GridPosition.y);
                }

                return null;
            }
            else if (swapDirection == BlockMovingDirection.Left)
            {
                if (origin.GridPosition.x > 0)
                {
                    return _blocks.Find(block => 
                        block.GridPosition.x == origin.GridPosition.x - 1 && 
                        block.GridPosition.y == origin.GridPosition.y);
                }

                return null;
            }

            return null;
        }
        
        private Tween BeginSwap(BlockView first, BlockView second)
        {
            BlockSwapData firstData = first.GetSwapData();
            BlockSwapData secondData = second.GetSwapData();
            
            first
                .SetGridPosition(secondData.GridPosition)
                .SetSortingOrder(secondData.SortingOrder);
            
            second
                .SetGridPosition(firstData.GridPosition)
                .SetSortingOrder(firstData.SortingOrder);
            
            return DOTween.Sequence()
                .Append(first.transform
                    .DOMove(secondData.WorldPosition, _movingDuration)
                    .SetEase(Ease.Flash))
                .Join(second.transform
                    .DOMove(firstData.WorldPosition, _movingDuration)
                    .SetEase(Ease.Flash));
        }

        public void Dispose()
        {
            _movingTween.Kill();
        }
    }
}