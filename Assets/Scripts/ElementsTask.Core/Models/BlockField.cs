using UnityEngine;

namespace ElementsTask.Core.Models
{
    public class BlockField
    {
        private Block[,] _blocks;

        public BlockField(Block[,] blocks)
        {
            _blocks = blocks;
        }
    }
}