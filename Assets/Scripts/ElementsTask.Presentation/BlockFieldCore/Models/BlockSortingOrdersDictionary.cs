using UnityEngine;

namespace ElementsTask.Presentation.BlockFieldCore.Models
{
    public class BlockSortingOrdersDictionary
    {
        private readonly int[,] _sortingOrders;
        
        public BlockSortingOrdersDictionary(Vector2Int gridSize)
        {
            _sortingOrders = new int[gridSize.y, gridSize.x];
            
            int currentSortingOrder = 0;
            
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    _sortingOrders[y, x] = currentSortingOrder;
                    currentSortingOrder++;
                }
            }
        }

        public int GetSortingOrder(int x, int y)
        {
            return _sortingOrders[y, x];
        }
        
        public int GetSortingOrder(Vector2Int position)
        {
            return GetSortingOrder(position.x, position.y);
        }
    }
}