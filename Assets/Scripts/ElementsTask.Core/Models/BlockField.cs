namespace ElementsTask.Core.Models
{
    public class BlockField
    {
        public Block[,] Blocks { get; private set; }

        public int Width => Blocks.GetLength(0);
        public int Height => Blocks.GetLength(1);
        
        public BlockField(Block[,] blocks)
        {
            Blocks = blocks;
        }
    }
}