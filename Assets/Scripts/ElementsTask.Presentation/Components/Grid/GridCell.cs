using UnityEngine;

namespace ElementsTask.Presentation.Components.Grid
{
    public class GridCell<TContent> where TContent : class
    {
        public Transform Transform { get; private set; }
        public TContent Content { get; set; }
        public Vector2Int Position { get; set; }
        
        public bool HasContent => Content != null; 

        public GridCell(Transform transform, Vector2Int position, TContent content = null)
        {
            Transform = transform;
            Position = position;
            Content = content;
        }
    }
}