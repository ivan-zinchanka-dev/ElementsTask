using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ElementsTask.Common.Extensions;
using ElementsTask.Presentation.Models;
using ElementsTask.Presentation.Views;
using UnityEngine;

namespace ElementsTask.Presentation.Services.BlockFieldHandlers
{
    public class BlocksFallingHandler : IDisposable
    {
        private readonly Vector2Int _fieldSize;
        private readonly List<BlockView> _blocks;
        private readonly float _fallingSpeed;

        private Sequence _fallingTween;
        
        public BlocksFallingHandler(Vector2Int fieldSize, List<BlockView> blocks, float fallingSpeed = 3f)
        {
            _fieldSize = fieldSize;
            _blocks = blocks;
            _fallingSpeed = fallingSpeed;
        }

        public async UniTask SimulateFallingAsync()
        {
            if (_fallingTween.IsActive())
            {
                return;
            }

            Dictionary<BlockView, BlockView> swapPairs = GetSwapPairs();

            _fallingTween = DOTween.Sequence();

            foreach (var pair in swapPairs)
            {
                _fallingTween.Join(BeginSwap(pair.Key, pair.Value));
            }

            await _fallingTween.ToUniTask();
        }

        private Dictionary<BlockView, BlockView> GetSwapPairs()
        {
            var swapPairs = new Dictionary<BlockView, BlockView>();
            
            foreach (BlockView block in _blocks)
            {
                if (!block.IsEmpty && block.GridPosition.y > 0)
                {
                    BlockView pair = GetSwapPair(block);
                    
                    if (pair != null)
                    {
                        swapPairs.Add(block, pair);
                        
                        //relevantBlocks.Add(block);
                    }
                }
            }

            return swapPairs;
        }

        private BlockView GetSwapPair(BlockView origin)
        {
            Vector2Int targetPosition = origin.GridPosition.WithY(origin.GridPosition.y - 1);
            BlockView pair = null;

            while (targetPosition.y >= 0)
            {
                BlockView current = _blocks.Find(other => other.GridPosition == targetPosition);

                if (!current.IsEmpty)
                {
                    break;
                }

                pair = current;
                targetPosition.y--;
            }

            return pair;
        }
        
        //TODO Unify
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

            float fallingDistance = Vector3.Distance(first.transform.position, second.transform.position);
            float fallingDuration = fallingDistance / _fallingSpeed;

            return DOTween.Sequence()
                .Append(first.transform
                    .DOMove(secondData.WorldPosition, fallingDuration)
                    .SetEase(Ease.InQuart))
                .Join(second.transform
                    .DOMove(firstData.WorldPosition, fallingDuration)
                    .SetEase(Ease.InQuart));
        }

        public void Dispose()
        {
            _fallingTween.Kill();
        }
    }
}