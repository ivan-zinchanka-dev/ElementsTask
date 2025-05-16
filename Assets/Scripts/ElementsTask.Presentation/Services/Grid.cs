using UnityEngine;

namespace ElementsTask.Presentation.Services
{
    public class Grid : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _cellSize = new Vector2(2, 2);
        [SerializeField]
        private Vector2Int _cellsCount = new Vector2Int(6, 9);
        
        private Transform[,] _cells;
        
        [Sirenix.OdinInspector.Button]
        private void Generate()
        {
            Vector3 originPosition = transform.position;
            Vector3 currentPosition = originPosition;
            
            Debug.Log("Generate");
            
            for (int i = 0; i < _cellsCount.y; i++)
            {
                for (int j = 0; j < _cellsCount.x; j++)
                {
                    var go = new GameObject("w");
                    go.transform.SetParent(transform);
                    go.transform.position = currentPosition;

                    currentPosition.x += _cellSize.x;
                }

                currentPosition.x = originPosition.x;
                currentPosition.y += _cellSize.y;
            }

        }

        [Sirenix.OdinInspector.Button]
        private void Clear()
        {
            
        }

    }
}