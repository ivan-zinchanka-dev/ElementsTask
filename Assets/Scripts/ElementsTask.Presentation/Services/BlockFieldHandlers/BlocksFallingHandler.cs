using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ElementsTask.Common.Extensions;
using ElementsTask.Presentation.Views;
using UnityEngine;

namespace ElementsTask.Presentation.Services.BlockFieldHandlers
{
    public class BlocksFallingHandler
    {
        private readonly Vector2Int _fieldSize;
        private readonly List<BlockView> _blocks;
        private readonly float _fallingSpeed;

        private Sequence _fallingTween;
        
        public BlocksFallingHandler(Vector2Int fieldSize, List<BlockView> blocks, float fallingSpeed = 1f)
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

            List<BlockView> relevantBlocks = GetRelevantBlocks();

            _fallingTween = DOTween.Sequence();

            foreach (BlockView block in relevantBlocks)
            {
                _fallingTween
                    .Join(block.transform.DOShakeScale(0.15f, new Vector3(1, 1, 0)));
            }

            await _fallingTween.ToUniTask();
        }

        private List<BlockView> GetRelevantBlocks()
        {
            List<BlockView> relevantBlocks = new List<BlockView>(_blocks.Capacity);
            
            foreach (BlockView block in _blocks)
            {
                if (!block.IsEmpty && block.GridPosition.y > 0)
                {
                    Vector2Int bottom = block.GridPosition.WithY(block.GridPosition.y - 1);
                    BlockView blockBottom = _blocks.Find(other => other.GridPosition == bottom);

                    if (blockBottom != null && blockBottom.IsEmpty)
                    {
                        relevantBlocks.Add(block);
                    }
                }
            }

            return relevantBlocks;
        }
    }
}