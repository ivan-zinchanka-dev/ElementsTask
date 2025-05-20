using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ElementsTask.Common.Extensions;
using ElementsTask.Presentation.BlockFieldCore.Views;
using ElementsTask.Presentation.Components.Grid;
using UnityEngine;

namespace ElementsTask.Presentation.BlockFieldCore.Services.Handlers
{
    public class BlocksFallingHandler : IDisposable
    {
        private readonly BlockFieldViewGrid _grid;
        private readonly float _fallingSpeed;
        
        private Sequence _fallingTween;
        
        public BlocksFallingHandler(BlockFieldViewGrid grid, 
            float fallingSpeed = 3f)
        {
            _grid = grid;
            _fallingSpeed = fallingSpeed;
        }
        

        public async UniTask StartFallingAsync()
        {
            if (_fallingTween.IsActive())
            {
                return;
            }
            
            _fallingTween = DOTween.Sequence();
            
            while (TrySimulateFalling(_fallingTween))
            {
                await _fallingTween.ToUniTask();
                _fallingTween = DOTween.Sequence();
            }
        }

        private bool TrySimulateFalling(Sequence fallingTween)
        {
            bool needSimulation = false;
            
            foreach (GridCell<BlockView> currentCell in _grid)
            {
                if ( currentCell.Content  != null && !currentCell.Content.IsEmpty && currentCell.Position.y > 0)
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
                        
                        fallingTween
                            .Join(Fall(bottom, column));
                        
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
            return bottom.Content.IsEmpty;
        }

        private Sequence Fall(GridCell<BlockView> emptyBottomCell, List<GridCell<BlockView>> blockColumn)
        {
            Vector2Int topPosition = new Vector2Int(emptyBottomCell.Position.x, emptyBottomCell.Position.y + blockColumn.Count);
            GridCell<BlockView> topCell = _grid.GetCell(topPosition);

            BlockView emptyBottom = emptyBottomCell.Content;
            emptyBottom.transform.position = topCell.Transform.position;
            
            Sequence fallingTween = DOTween.Sequence();
            
            foreach (GridCell<BlockView> currentCell in blockColumn)
            {
                Vector2Int targetPosition = new Vector2Int(currentCell.Position.x, currentCell.Position.y - 1);
                GridCell<BlockView> targetCell = _grid.GetCell(targetPosition);
                
                targetCell.Content = currentCell.Content;
                targetCell.Content.SortingOrder -= _grid.Width;
                
                fallingTween.Join(
                    targetCell.Content.transform.DOMove(targetCell.Transform.position, 0.15f)
                        .SetEase(Ease.Linear));
            }
            
            topCell.Content = emptyBottom;
            
            return fallingTween;
        }

        public void Dispose()
        {
            _fallingTween.Kill();
        }
    }
}