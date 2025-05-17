using UnityEngine;

namespace ElementsTask.Presentation.Models
{
    public struct BlockSwapData
    {
        public Vector3 WorldPosition { get; }
        public Vector2Int GridPosition { get; }

        public BlockSwapData(Vector3 worldPosition, Vector2Int gridPosition)
        {
            WorldPosition = worldPosition;
            GridPosition = gridPosition;
        }
    }
}