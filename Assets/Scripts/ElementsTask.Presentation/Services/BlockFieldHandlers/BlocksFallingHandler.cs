using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ElementsTask.Common.Extensions;
using ElementsTask.Presentation.BlockFieldCore.Components;
using ElementsTask.Presentation.Components.Grid;
using ElementsTask.Presentation.Views;
using UnityEngine;

namespace ElementsTask.Presentation.Services.BlockFieldHandlers
{
    public class BlocksFallingHandler : IDisposable
    {
        private readonly Vector2Int _fieldSize;
        private readonly BlockFieldViewGrid _grid;
        private readonly float _fallingSpeed;
        
        private Sequence _fallingTween;
        
        public BlocksFallingHandler(Vector2Int fieldSize, BlockFieldViewGrid grid, 
            float fallingSpeed = 3f)
        {
            _fieldSize = fieldSize;
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
            
            /*foreach (GridCell<BlockView> cell in _grid)
            {
                BlockView block = cell.Content;
                
                if (!block.IsEmpty && block.GridPosition.y > 0)
                {
                    if (IsFloatingBlock(block, out BlockView bottom))
                    {
                        List<BlockView> column = _grid
                            .Where(other => 
                                other.GridPosition.x == cell.GridPosition.x &&
                                other.GridPosition.y >= cell.GridPosition.y)
                            .OrderBy(other =>other.GridPosition.y)
                            .ToList();
                        
                        fallingTween
                            .Join(Fall(bottom, column));
                        
                        needSimulation = true;
                    }
                }
            }*/

            return needSimulation;
        }
        
        /*private bool IsFloatingBlock(BlockView origin, out BlockView bottom)
        {
            Vector2Int targetPosition = origin.GridPosition.WithY(origin.GridPosition.y - 1);
            
            bottom = _grid.GetCell(targetPosition).Content;
            return bottom.IsEmpty;
        }

        private Sequence Fall(BlockView emptyBottom, List<BlockView> blockColumn)
        {
            Vector2Int top = new Vector2Int(emptyBottom.GridPosition.x, emptyBottom.GridPosition.y + blockColumn.Count);
            emptyBottom.SetGridPosition(top);
            emptyBottom.transform.position = _grid.GetCell(top).Transform.position;
            
            Sequence fallingTween = DOTween.Sequence();
            
            for (int i = 0; i < blockColumn.Count; i++)
            {
                BlockView block = blockColumn[i];
                Vector2Int targetPosition = new Vector2Int(block.GridPosition.x, block.GridPosition.y - 1);
                block.SetGridPosition(targetPosition);

                fallingTween.Join(
                    block.transform.DOMove(_grid.GetCell(targetPosition).Transform.position, 0.15f)
                        .SetEase(Ease.Linear));
            }
            
            return fallingTween;
        }*/

        public void Dispose()
        {
            _fallingTween.Kill();
        }
    }
}