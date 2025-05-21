using System.Threading.Tasks;
using ElementsTask.Data.BlockFieldCore.Models;
using ElementsTask.Data.Levels.Services;
using ElementsTask.Data.PlayerProgression.Models;
using ElementsTask.Data.PlayerProgression.Services;

namespace ElementsTask.Data.BlockFieldCore.Services
{
    public class BlockFieldCreator
    {
        private readonly IPlayerProgressService _playerProgressService;
        private readonly IBuildInLevelLoader _buildInLevelLoader;
        private readonly ISavedLevelLoader _savedLevelLoader;

        public BlockFieldCreator(
            IPlayerProgressService playerProgressService, 
            IBuildInLevelLoader buildInLevelLoader, 
            ISavedLevelLoader savedLevelLoader)
        {
            _playerProgressService = playerProgressService;
            _buildInLevelLoader = buildInLevelLoader;
            _savedLevelLoader = savedLevelLoader;
        }

        public async Task<BlockField> CreateFieldAsync()
        {
            PlayerProgress playerProgress = await _playerProgressService.GetPlayerProgressAsync();
            
            if (playerProgress.HasSavedFieldState)
            {
                return await GetFieldFromSavedLevelAsync();
            }
            else
            {
                return await GetFieldFromBuiltInLevelAsync(playerProgress);
            }
        }

        private async Task<BlockField> GetFieldFromSavedLevelAsync()
        {
            return await _savedLevelLoader.LoadCurrentLevelAsync();
        }

        private async Task<BlockField> GetFieldFromBuiltInLevelAsync(PlayerProgress playerProgress)
        {
            BlockField field = await _buildInLevelLoader.LoadLevelAsync(playerProgress.CurrentLevelIndex);

            if (field != null)
            {
                return field;
            }
            else
            {
                playerProgress.CurrentLevelIndex = 0;
                return await _buildInLevelLoader.LoadLevelAsync(playerProgress.CurrentLevelIndex);
            }
        }
    }
}