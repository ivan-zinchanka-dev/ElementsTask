using System;
using System.Threading.Tasks;
using ElementsTask.Data.PlayerProgression.Models;

namespace ElementsTask.Data.PlayerProgression.Services
{
    public interface IPlayerProgressService : IDisposable
    {
        public Task<PlayerProgress> GetPlayerProgressAsync();
    }
}