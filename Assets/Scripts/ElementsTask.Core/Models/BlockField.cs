using UnityEngine;

namespace ElementsTask.Core.Models
{
    public class BlockField
    {
        private Vector2Int _size;
        private Block[,] _blocks; 
        
        public BlockField(Vector2Int size)
        {
            _size = size;
            _blocks = new Block[size.x, size.y];
        }
        
    }
}