using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ElementsTask.Common.Components.Grid;
using ElementsTask.Common.Extensions;
using ElementsTask.Presentation.BlockFieldCore.Models;
using ElementsTask.Presentation.BlockFieldCore.Views;
using UnityEngine;

namespace ElementsTask.Presentation.BlockFieldCore.Services.Handlers
{
    public class BlocksFallingHandler : IDisposable
    {
        private const string BlocksFallingTweenGroupId = "blocks_falling";
        
        private readonly BlockFieldViewOptions _viewOptions;
        private readonly BlockFieldViewGrid _grid;
        private readonly BlockSortingOrdersDictionary _cachedSortingOrders;
        
        private Sequence _fallingTween;

        public BlocksFallingHandler(
            BlockFieldViewOptions viewOptions, 
            BlockFieldViewGrid grid, 
            BlockSortingOrdersDictionary cachedSortingOrders)
        {
            _viewOptions = viewOptions;
            _grid = grid;
            _cachedSortingOrders = cachedSortingOrders;
        }

        public async UniTask<bool> SimulateFallingIfNeedAsync()
        {
            if (_fallingTween.IsActive())
            {
                return false;
            }
            
            _fallingTween = DOTween.Sequence()
                .SetId(BlocksFallingTweenGroupId);
            bool simulateOnce = false;
            
            while (TrySimulateFalling())
            {
                await _fallingTween.ToUniTask();
                simulateOnce = true;
                _fallingTween = DOTween.Sequence()
                    .SetId(BlocksFallingTweenGroupId);
            }
            
            return simulateOnce;
        }

        private bool TrySimulateFalling()
        {
            bool needSimulation = false;
            
            foreach (GridCell<BlockView> currentCell in _grid)
            {
                if (currentCell.HasContent && currentCell.Position.y > 0)
                {
                    if (IsFloatingBlock(currentCell, out GridCell<BlockView> bottom))
                    {
                        List<GridCell<BlockView>> column = _grid
                            .Where(otherCell => 
                                otherCell.HasContent &&
                                otherCell.Position.x == currentCell.Position.x &&
                                otherCell.Position.y >= currentCell.Position.y)
                            .OrderBy(otherCell => otherCell.Position.y)
                            .ToList();
                        
                        _fallingTween.Join(Fall(bottom, column));
                        
                        needSimulation = true;
                    }
                }
            }
            
            return needSimulation;
        }
        
        private bool IsFloatingBlock(GridCell<BlockView> origin, out GridCell<BlockView> bottom)
        {
            Vector2Int targetPosition = origin.Position.WithY(origin.Position.y - 1);
            bottom = _grid.GetCell(targetPosition);
            return bottom.IsEmpty;
        }

        private Sequence Fall(GridCell<BlockView> emptyBottomCell, List<GridCell<BlockView>> blockColumn)
        {
            Vector2Int topPosition = new Vector2Int(emptyBottomCell.Position.x, emptyBottomCell.Position.y + blockColumn.Count);
            GridCell<BlockView> topCell = _grid.GetCell(topPosition);
            
            Sequence columnFallingTween = DOTween.Sequence();
            var blocks = new List<BlockView>();
            
            foreach (GridCell<BlockView> currentCell in blockColumn)
            {
                Vector2Int targetPosition = new Vector2Int(currentCell.Position.x, currentCell.Position.y - 1);
                GridCell<BlockView> targetCell = _grid.GetCell(targetPosition);
                
                targetCell.Content = currentCell.Content;
                targetCell.Content.SortingOrder = _cachedSortingOrders.GetSortingOrder(targetCell.Position);
                targetCell.Content.OnFall();
                
                columnFallingTween.Join(
                    targetCell.Content.transform
                        .DOMove(targetCell.Transform.position, _viewOptions.FallingSpeed)
                        .SetEase(Ease.Linear)
                        .SetId(BlocksFallingTweenGroupId));
                
                blocks.Add(targetCell.Content);
            }
            
            topCell.Content = null;

            columnFallingTween.AppendCallback(() =>
            {
                foreach (BlockView block in blocks)
                {
                    block.OnLand();
                }
            })
            .SetId(BlocksFallingTweenGroupId);
            
            return columnFallingTween;
        }

        public void Dispose()
        {
            Debug.Log("DISPOSE FALLING");
            _fallingTween.Kill();
            DOTween.Kill(BlocksFallingTweenGroupId);
        }
    }
}