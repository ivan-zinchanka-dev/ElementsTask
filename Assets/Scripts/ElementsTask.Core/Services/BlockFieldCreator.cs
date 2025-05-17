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
            new BlockField(new Block[,]
            {
                { Empty, Water, Fire, Water, Water, Empty },
                { Empty, Water, Fire, Water, Water, Empty },
                { Empty, Fire, Water, Fire, Fire, Empty },
                { Empty, Water, Fire, Water, Water, Empty },
                { Empty, Water, Water, Empty, Empty, Empty },
            }),
            new BlockField(new Block[,]
            {
                { Empty, Water, Fire, Water, Water, Empty },
                { Empty, Water, Fire, Water, Water, Empty },
                { Empty, Fire, Water, Fire, Fire, Empty },
                
                { Empty, Water, Water, Empty, Water, Empty },
                { Empty, Water, Fire, Empty, Empty, Empty },
                { Empty, Water, Empty, Empty, Empty, Empty },
            }),
            new BlockField(new Block[,]
            {
                { Fire, Water, Fire, Water, Water, Fire },
                { Fire, Water, Fire, Water, Water, Fire },
                { Fire, Fire, Water, Fire, Fire, Fire },
                
                { Fire, Water, Water, Empty, Water, Fire },
                { Fire, Water, Fire, Empty, Empty, Fire },
                { Fire, Water, Empty, Empty, Empty, Empty },
                
                { Fire, Water, Empty, Empty, Empty, Empty },
                { Fire, Water, Empty, Empty, Empty, Empty },
                { Fire, Water, Empty, Empty, Empty, Empty },
            }),
        };
        
        public BlockField Create()
        {
            return _levels[1];
        }
        
        private static Block Block(string typeId)
        {
            return new Block(BlockType.Parse(typeId));
        }
        
        private static Block Empty => Models.Block.Empty;
        private static Block Water => Block("Water");
        private static Block Fire => Block("Fire");
    }
}