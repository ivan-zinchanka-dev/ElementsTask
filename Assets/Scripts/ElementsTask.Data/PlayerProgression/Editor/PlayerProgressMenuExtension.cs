#if UNITY_EDITOR
using ElementsTask.Data.PlayerProgression.Services;
using UnityEditor;

namespace ElementsTask.Data.PlayerProgression.Editor
{
    public static class PlayerProgressMenuExtension
    {
        [MenuItem("Tools/Player Progress/Log Directory")]
        public static void LogTargetDirectory()
        {
            CreateProgressService();
        }
        
        [MenuItem("Tools/Player Progress/Clear")]
        public static void ClearProgress()
        {
            CreateProgressService().ClearProgress();
        }

        private static PlayerProgressService CreateProgressService()
        {
            return new PlayerProgressService(new FakeCryptoService());
        }
    }
}
#endif