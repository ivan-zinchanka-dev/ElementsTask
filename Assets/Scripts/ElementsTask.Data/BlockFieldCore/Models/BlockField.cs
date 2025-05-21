using UnityEngine;

namespace ElementsTask.Data.BlockFieldCore.Models
{
    public class BlockField
    {
        private readonly Block[,] _blocks;

        public int Height => _blocks.GetLength(0);
        public int Width => _blocks.GetLength(1);
        public Vector2Int Size => new Vector2Int(Width, Height);
        
        public BlockField(Block[,] blocks)
        {
            _blocks = blocks;
        }

        public Block GetBlock(int x, int y)
        {
            return _blocks[y, x];
        }
        
        public Block GetBlock(Vector2Int position)
        {
            return GetBlock(position.y, position.x);
        }
        
        public Block GetBlock(in Vector2Int position)
        {
            return GetBlock(position.y, position.x);
        }
    }
}