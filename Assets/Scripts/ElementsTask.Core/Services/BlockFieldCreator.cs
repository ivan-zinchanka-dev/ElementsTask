using System.Threading.Tasks;
using ElementsTask.Core.Models;

namespace ElementsTask.Core.Services
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