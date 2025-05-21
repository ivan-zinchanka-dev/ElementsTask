using ElementsTask.Data.BlockFieldCore.Models;
using ElementsTask.Data.PlayerProgression.Models;
using ElementsTask.Presentation.Components.Grid;
using UnityEngine;

namespace ElementsTask.Presentation.BlockFieldCore.Views
{
    public class BlockFieldViewGrid : Grid<BlockView>
    {
        private PlayerProgress _playerProgress;
        
        public BlockFieldViewGrid WithChangesTracking(PlayerProgress playerProgress)
        {
            _playerProgress = playerProgress;
            return this;
        }

        public async void SaveBlockFieldState()
        {
            for (int y = 0; y < _cellsCount.y; y++)
            {
                for (int x = 0; x < _cellsCount.x; x++)
                {
                    _playerProgress.BlockFieldState[new Vector2Int(x, y)] = GetBlock(_cells[y, x].Content);
                }
            }
         
            await _playerProgress.SaveAsync();
        }

        private static Block GetBlock(BlockView blockView)
        {
            return blockView != null ? blockView.Model : Block.Empty;
        }
    }
}