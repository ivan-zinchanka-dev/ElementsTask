using System.Threading.Tasks;
using ElementsTask.Data.BlockFieldCore.Models;

namespace ElementsTask.Data.Levels.Services
{
    public interface ISavedLevelLoader
    {
        public Task<BlockField> LoadCurrentLevelAsync();
    }
}