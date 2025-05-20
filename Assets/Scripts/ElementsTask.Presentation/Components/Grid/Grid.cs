using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementsTask.Presentation.Components.Grid
{
    public class Grid<TContent> : MonoBehaviour, IEnumerable<GridCell<TContent>> where TContent : class
    {
        [SerializeField]
        private Vector2 _cellSize = new Vector2(0.78f, 0.78f);
        [SerializeField]
        private Vector2Int _cellsCount = new Vector2Int(6, 9);

        private GridCell<TContent>[,] _cells;

        public int Width => _cellsCount.x;
        public int Height => _cellsCount.y;
        
        public GridCell<TContent> GetCell(Vector2Int position) => GetCell(position.x, position.y);
        
        public GridCell<TContent> GetCell(int x, int y)
        {
            _cells ??= GenerateCells();

            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return _cells[y, x];
            }
            else
            {
                return null;
            }
        }

        public IEnumerator<GridCell<TContent>> GetEnumerator()
        {
            _cells ??= GenerateCells();

            for (int y = 0; y < _cellsCount.y; y++)
            {
                for (int x = 0; x < _cellsCount.x; x++)
                {
                    yield return _cells[y, x];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        private GridCell<TContent>[,] GenerateCells()
        {
            Vector3 originPosition = transform.position;
            Vector3 currentPosition = originPosition;
            
            var cells = new GridCell<TContent>[_cellsCount.y, _cellsCount.x];
            
            for (int y = 0; y < _cellsCount.y; y++)
            {
                for (int x = 0; x < _cellsCount.x; x++)
                {
                    var cell = new GameObject($"cell_({x},{y})");
                    cell.transform.SetParent(transform);
                    cell.transform.position = currentPosition;
                    cells[y, x] = new GridCell<TContent>(cell.transform, new Vector2Int(x, y)); 
                    
                    currentPosition.x += _cellSize.x;
                }

                currentPosition.x = originPosition.x;
                currentPosition.y += _cellSize.y;
            }
            
            return cells;
        }
    }
}