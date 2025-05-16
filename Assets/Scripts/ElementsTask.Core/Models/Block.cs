using ElementsTask.Core.Enums;

namespace ElementsTask.Core.Models
{
    public struct Block
    {
        public BlockType Type { get; private set; }
        public BlockState State { get; private set; }
    }
}