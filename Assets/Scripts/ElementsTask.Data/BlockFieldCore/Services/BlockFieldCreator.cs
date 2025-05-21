using System.Threading.Tasks;
using ElementsTask.Data.BlockFieldCore.Models;
using ElementsTask.Data.Levels.Services;
using ElementsTask.Data.PlayerProgression;
using ElementsTask.Data.PlayerProgression.Services;

namespace ElementsTask.Data.BlockFieldCore.Services
{
    public class BlockFieldCreator
    {
        private readonly IPlayerProgressService _playerProgressService;
        private readonly ILevelLoader _levelLoader;

        public BlockFieldCreator(IPlayerProgressService playerProgressService, ILevelLoader levelLoader)
        {
            _playerProgressService = playerProgressService;
            _levelLoader = levelLoader;
        }

        public async Task<BlockField> CreateFieldAsync()
        {
            BlockField field = await _levelLoader.LoadLevelAsync(_playerProgressService.CurrentLevelIndex);

            if (field != null)
            {
                return field;
            }
            else
            {
                _playerProgressService.CurrentLevelIndex = 0;
                return await _levelLoader.LoadLevelAsync(_playerProgressService.CurrentLevelIndex);
            }
        }
    }
}