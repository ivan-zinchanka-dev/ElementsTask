using System;
using System.Collections.Generic;
using System.Linq;
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
        private Dictionary<Vector2Int, Transform> _cells = new();
        
        private Sequence _fallingTween;
        
        public BlocksFallingHandler(Dictionary<Vector2Int, Transform> cells, Vector2Int fieldSize, List<BlockView> blocks, float fallingSpeed = 3f)
        {
            _cells = cells;
            _fieldSize = fieldSize;
            _blocks = blocks;
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
            }
            
        }

        private bool TrySimulateFalling(Sequence fallingTween)
        {
            bool needSimulation = false;
            
            foreach (BlockView block in _blocks)
            {
                if (!block.IsEmpty && block.GridPosition.y > 0)
                {
                    if (IsFloatingBlock(block, out BlockView bottom))
                    {
                        List<BlockView> column = _blocks
                            .Where(other => 
                                other.GridPosition.x == block.GridPosition.x &&
                                other.GridPosition.y >= block.GridPosition.y)
                            .OrderBy(other =>other.GridPosition.y)
                            .ToList();

                        /*foreach (var bv in column)
                        {
                            bv.transform.localScale *= 0.75f;
                        }*/
                        
                        fallingTween
                            .Join(Fall(bottom, column));
                        
                        needSimulation = true;
                    }
                }
            }

            return needSimulation;
        }
        
        private bool IsFloatingBlock(BlockView origin, out BlockView bottom)
        {
            Vector2Int targetPosition = origin.GridPosition.WithY(origin.GridPosition.y - 1);
            
            bottom = _blocks.Find(other => other.GridPosition == targetPosition);
            return bottom.IsEmpty;
        }

        private Sequence Fall(BlockView emptyBottom, List<BlockView> blockColumn)
        {
            Vector2Int top = new Vector2Int(emptyBottom.GridPosition.x, emptyBottom.GridPosition.y + blockColumn.Count);
            emptyBottom.SetGridPosition(top);
            emptyBottom.transform.position = _cells[top].transform.position;
            
            Sequence fallingTween = DOTween.Sequence();
            
            for (int i = 0; i < blockColumn.Count; i++)
            {
                BlockView block = blockColumn[i];
                Vector2Int targetPosition = new Vector2Int(block.GridPosition.x, block.GridPosition.y - 1);
                block.SetGridPosition(targetPosition);

                fallingTween.Join(
                    block.transform.DOMove(_cells[targetPosition].position, 0.15f)
                        .SetEase(Ease.Linear));
            }
            
            return fallingTween;
        }

        public void Dispose()
        {
            _fallingTween.Kill();
        }
    }
}