using ElementsTask.Core.Services;
using UnityEngine;

namespace ElementsTask.Presentation.Management
{
    public class Startup : MonoBehaviour
    {
        private void Awake()
        {
            new BlockFieldCreator().Create();
        }
    }
}