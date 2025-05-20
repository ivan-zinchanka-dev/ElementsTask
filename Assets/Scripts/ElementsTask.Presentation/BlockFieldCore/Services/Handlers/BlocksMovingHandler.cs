using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ElementsTask.Common.Extensions;
using ElementsTask.Presentation.BlockFieldCore.Enums;
using ElementsTask.Presentation.BlockFieldCore.Models;
using ElementsTask.Presentation.BlockFieldCore.Views;
using ElementsTask.Presentation.Components.Grid;
using UnityEngine;

namespace ElementsTask.Presentation.BlockFieldCore.Services.Handlers
{
    public class BlocksMovingHandler : IDisposable
    {
        private readonly Vector2Int _fieldSize;
        private readonly Grid<BlockView> _grid;
        private readonly float _movingDuration;
        
        private Tween _movingTween;
        
        public BlocksMovingHandler(Vector2Int fieldSize, Grid<BlockView> grid, float movingDuration = 0.15f)
        {
            _fieldSize = fieldSize;
            _grid = grid;
            _movingDuration = movingDuration;
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
        
        private GridCell<BlockView> GetSwapPair(GridCell<BlockView> origin, BlockMovingDirection swapDirection)
        {
            Vector2Int? targetPosition = null;
            
            switch (swapDirection)
            {
                case BlockMovingDirection.Up:
                    if (origin.Position.y < _fieldSize.y - 1)
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
                    if (origin.Position.x < _fieldSize.x - 1)
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

            return targetPosition.HasValue ? _grid.GetCell(targetPosition.Value) : null;
        }
        
        private Tween BeginSwap(GridCell<BlockView> firstCell, GridCell<BlockView> secondCell)
        {
            (firstCell.Content, secondCell.Content) = (secondCell.Content, firstCell.Content);
            return BeginSwap(firstCell.Content, secondCell.Content);
        }
        
        private Tween BeginSwap(BlockView first, BlockView second)
        {
            BlockSwapData firstData = first.GetSwapData();
            BlockSwapData secondData = second.GetSwapData();
            
            first.SetSortingOrder(secondData.SortingOrder);
            
            second.SetSortingOrder(firstData.SortingOrder);
            
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