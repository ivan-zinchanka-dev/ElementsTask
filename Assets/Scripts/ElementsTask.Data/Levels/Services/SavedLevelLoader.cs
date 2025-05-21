using System.Threading.Tasks;
using ElementsTask.Data.BlockFieldCore.Models;
using ElementsTask.Data.PlayerProgression.Models;
using ElementsTask.Data.PlayerProgression.Services;
using UnityEngine;

namespace ElementsTask.Data.Levels.Services
{
    public class SavedLevelLoader : ISavedLevelLoader
    {
        private readonly IPlayerProgressService _playerProgressService;

        public SavedLevelLoader(IPlayerProgressService playerProgressService)
        {
            _playerProgressService = playerProgressService;
        }

        public async Task<BlockField> LoadCurrentLevelAsync()
        {
            PlayerProgress playerProgress = await _playerProgressService.GetPlayerProgressAsync();

            if (!playerProgress.HasSavedFieldState)
            {
                return null;
            }
            
            Block[,] blocks = new Block[BlockFieldConstraints.LevelSize.y, BlockFieldConstraints.LevelSize.x];
            
            for (int y = 0; y < blocks.GetLength(0); y++)
            {
                for (int x = 0; x < blocks.GetLength(1); x++)
                {
                    blocks[y, x] = GetBlock(new Vector2Int(x, y), playerProgress);
                }
            }

            return new BlockField(blocks);
        }

        private Block GetBlock(Vector2Int position, PlayerProgress playerProgress)
        {
            return playerProgress.BlockFieldState.TryGetValue(position, out Block block) ? block : Block.Empty;
        }
    }
}