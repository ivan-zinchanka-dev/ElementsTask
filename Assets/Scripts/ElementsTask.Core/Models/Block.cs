using ElementsTask.Core.Enums;

namespace ElementsTask.Core.Models
{
    public struct Block
    {
        public static Block Empty => new Block(BlockType.Empty);

        public BlockType Type { get; private set; }
        public BlockState State { get; set; }

        public Block(BlockType type)
        {
            Type = type;
            State = BlockState.Idle;
        }
    }
}