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
        public Transform[,] Cells => _cells ??= GenerateCells();
        
        private Transform[,] GenerateCells()
        {
            Vector3 originPosition = transform.position;
            Vector3 currentPosition = originPosition;
            
            var cells = new Transform[_cellsCount.y, _cellsCount.x];
            
            for (int i = 0; i < _cellsCount.y; i++)
            {
                for (int j = 0; j < _cellsCount.x; j++)
                {
                    var cell = new GameObject($"cell_{i}_{j}");
                    cell.transform.SetParent(transform);
                    cell.transform.position = currentPosition;
                    cells[i, j] = cell.transform; 
                    
                    currentPosition.x += _cellSize.x;
                }

                currentPosition.x = originPosition.x;
                currentPosition.y += _cellSize.y;
            }
            
            return cells;
        }
    }
}