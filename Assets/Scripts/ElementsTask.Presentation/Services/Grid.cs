using UnityEngine;

namespace ElementsTask.Presentation.Services
{
    public class Grid : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _cellSize = new Vector2(2, 2);
        [SerializeField]
        private Vector2Int _cellsCount = new Vector2Int(6, 9);

        [SerializeField] 
        public Transform[,] Cells { get; private set; }
        

        [Sirenix.OdinInspector.Button]
        public void Generate()
        {
            Vector3 originPosition = transform.position;
            Vector3 currentPosition = originPosition;
            
            Cells = new Transform[_cellsCount.y, _cellsCount.x];
            
            for (int i = 0; i < _cellsCount.y; i++)
            {
                for (int j = 0; j < _cellsCount.x; j++)
                {
                    var cell = new GameObject($"cell_{i}_{j}");
                    cell.transform.SetParent(transform);
                    cell.transform.position = currentPosition;
                    Cells[i, j] = cell.transform; 
                    
                    currentPosition.x += _cellSize.x;
                }

                currentPosition.x = originPosition.x;
                currentPosition.y += _cellSize.y;
            }

        }

        [Sirenix.OdinInspector.Button]
        private void Clear()
        {
            foreach (Transform cell in Cells)
            {
                DestroyImmediate(cell.gameObject);
            }
        }
    }
}