using ElementsTask.Core.Models;
using UnityEngine;

namespace ElementsTask.Core.Services
{
    public class BlockFieldCreator
    {
        public BlockField Create()
        {
            return new BlockField(new Vector2Int(6, 10));
        }
    }
}