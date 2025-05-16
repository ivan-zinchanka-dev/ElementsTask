using System.Collections.Generic;
using ElementsTask.Core.Models;

namespace ElementsTask.Core.Services
{
    public class BlockFieldCreator
    {
        private readonly List<BlockField> _levels = new List<BlockField>()
        {
            new BlockField(new Block[,]
            {
                { Empty, Water, Empty, Water, Fire, Fire },
                { Empty, Water, Empty, Fire, Empty, Empty },
            }),
        };
        
        public BlockField Create()
        {
            return _levels[0];
        }
        
        private static Block Block(string typeId)
        {
            return new Block(new BlockType(typeId));
        }
        
        private static Block Empty => Models.Block.Empty;
        private static Block Water => Block("Water");
        private static Block Fire => Block("Fire");
    }
}