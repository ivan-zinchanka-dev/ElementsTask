namespace ElementsTask.Core.Models
{
    public class BlockField
    {
        public Block[,] Blocks { get; private set; }

        public BlockField(Block[,] blocks)
        {
            Blocks = blocks;
        }
    }
}