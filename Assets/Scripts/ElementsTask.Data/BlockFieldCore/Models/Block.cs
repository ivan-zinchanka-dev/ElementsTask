using System;

namespace ElementsTask.Data.BlockFieldCore.Models
{
    [Serializable]
    public struct Block
    {
        public static Block Empty => new Block(BlockType.Empty);
        public BlockType Type { get; internal set; }
        
        public Block(BlockType type)
        {
            Type = type;
        }
    }
}