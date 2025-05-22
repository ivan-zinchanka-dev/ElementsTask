using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ElementsTask.Common.Components.Grid;
using ElementsTask.Common.Extensions;
using ElementsTask.Data.BlockFieldCore.Enums;
using ElementsTask.Presentation.BlockFieldCore.Enums;
using ElementsTask.Presentation.BlockFieldCore.Models;
using ElementsTask.Presentation.BlockFieldCore.Views;
using UnityEngine;

namespace ElementsTask.Presentation.BlockFieldCore.Services.Handlers
{
    public class BlocksSwappingHandler : IDisposable
    {
        private const string BlocksSwappingTweenGroupId = "blocks_swapping";
        
        private readonly BlockFieldViewOptions _viewOptions;
        private readonly BlockFieldViewGrid _grid;
        private readonly BlockSortingOrdersDictionary _cachedSortingOrders;
        
        private Tween _movingTween;

        public BlocksSwappingHandler(
            BlockFieldViewOptions viewOptions, 
            BlockFieldViewGrid grid, 
            BlockSortingOrdersDictionary cachedSortingOrders)
        {
            _viewOptions = viewOptions;
            _grid = grid;
            _cachedSortingOrders = cachedSortingOrders;
        }

        public async UniTask<bool> TryMoveBlockAsync(GridCell<BlockView> selectedCell, Vector3 targetPosition)
        {
            if (_movingTween.IsActive())
            {
                return false;
            }
            
            GridCell<BlockView> pair = GetSwapPair(selectedCell, 
                GetMovingDirection(targetPosition - selectedCell.Transform.position));

            if (pair != null)
            {
                _movingTween = BeginSwap(selectedCell, pair);
                _movingTween.SetId(BlocksSwappingTweenGroupId);
                await _movingTween.ToUniTask();
            }
            
            return true;
        }

        private static BlockMovingDirection GetMovingDirection(Vector3 inputOffset)
        {
            const float minOffsetMagnitude = 0.35f;

            if (inputOffset.magnitude < minOffsetMagnitude)
            {
                return BlockMovingDirection.None;
            }
            
            if (Mathf.Abs(inputOffset.x) > Mathf.Abs(inputOffset.y))
            {
                return inputOffset.x > 0f ? BlockMovingDirection.Right : BlockMovingDirection.Left;
            }
            else
            {
                return inputOffset.y > 0f ? BlockMovingDirection.Up : BlockMovingDirection.Down;
            }
        }

        private bool CanMoveUp(GridCell<BlockView> origin)
        {
            if (origin.Position.y < _grid.Height - 1)
            {
                GridCell<BlockView> upCell = _grid.GetCell(origin.Position.WithY(origin.Position.y + 1));
                return upCell.HasContent;
            }

            return false;
        }

        private GridCell<BlockView> GetSwapPair(GridCell<BlockView> origin, BlockMovingDirection swapDirection)
        {
            Vector2Int? targetPosition = null;
            
            switch (swapDirection)
            {
                case BlockMovingDirection.Up:
                    if (CanMoveUp(origin))
                    {
                        targetPosition = origin.Position.WithY(origin.Position.y + 1);
                    }
                    break;
                
                case BlockMovingDirection.Down:
                    if (origin.Position.y > 0)
                    {
                        targetPosition = origin.Position.WithY(origin.Position.y - 1);
                    }
                    break;
                
                case BlockMovingDirection.Right:
                    if (origin.Position.x < _grid.Width - 1)
                    {
                        targetPosition = origin.Position.WithX(origin.Position.x + 1);
                    }
                    break;
                
                case BlockMovingDirection.Left:
                    if (origin.Position.x > 0)
                    {
                        targetPosition = origin.Position.WithX(origin.Position.x - 1);
                    }
                    break;
            }

            if (targetPosition.HasValue)
            {
                GridCell<BlockView> pair = _grid.GetCell(targetPosition.Value);
                return pair.IsEmpty || pair.Content.State == BlockState.Idle ? pair : null; 
            }

            return null;
        }
        
        private Tween BeginSwap(GridCell<BlockView> firstCell, GridCell<BlockView> secondCell)
        {
            (firstCell.Content, secondCell.Content) = (secondCell.Content, firstCell.Content);

            Sequence swappingSequence = DOTween.Sequence();
            
            if (firstCell.HasContent)
            {
                firstCell.Content.SortingOrder = _cachedSortingOrders.GetSortingOrder(firstCell.Position);
                
                swappingSequence
                    .Join(firstCell.Content.transform
                    .DOMove(firstCell.Transform.position, _viewOptions.SwappingDuration)
                    .SetEase(Ease.Flash)
                    .SetId(BlocksSwappingTweenGroupId));
            }

            if (secondCell.HasContent)
            {
                secondCell.Content.SortingOrder = _cachedSortingOrders.GetSortingOrder(secondCell.Position);
                
                swappingSequence
                    .Join(secondCell.Content.transform
                        .DOMove(secondCell.Transform.position, _viewOptions.SwappingDuration)
                        .SetEase(Ease.Flash)
                        .SetId(BlocksSwappingTweenGroupId));
            }

            return swappingSequence;
        }

        public void Dispose()
        {
            _movingTween.Kill();
            DOTween.Kill(BlocksSwappingTweenGroupId);
        }
    }
}