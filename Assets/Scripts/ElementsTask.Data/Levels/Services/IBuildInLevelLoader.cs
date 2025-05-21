using System.Threading.Tasks;
using ElementsTask.Data.BlockFieldCore.Models;

namespace ElementsTask.Data.Levels.Services
{
    public interface IBuildInLevelLoader
    {
        public Task<BlockField> LoadLevelAsync(int levelIndex);
    }
}