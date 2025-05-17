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
            return await _levelLoader.LoadLevelAsync(_playerProgressService.CurrentLevelIndex);
        }
    }
}