using System.Threading.Tasks;
using ElementsTask.Core.Models;

namespace ElementsTask.Core.Services
{
    public interface ILevelLoader
    {
        public Task<BlockField> LoadLevelAsync(int levelIndex);
    }
}