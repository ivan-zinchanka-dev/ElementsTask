using UnityEngine;

namespace ElementsTask.Presentation.BlockFieldCore.Models
{
    public struct BlockSwapData
    {
        public Vector3 WorldPosition { get; }
        public int SortingOrder { get; }
        
        public BlockSwapData(Vector3 worldPosition, int sortingOrder)
        {
            WorldPosition = worldPosition;
            SortingOrder = sortingOrder;
        }
    }
}