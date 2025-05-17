using ElementsTask.Core.Enums;

namespace ElementsTask.Core.Models
{
    public struct Block
    {
        public static readonly BlockType EmptyType = new BlockType("Empty");
        
        public BlockType Type { get; private set; }
        public BlockState State { get; set; }

        public Block(BlockType type)
        {
            Type = type;
            State = BlockState.Idle;
        }

        public static Block Empty => new Block(EmptyType);
    }
}