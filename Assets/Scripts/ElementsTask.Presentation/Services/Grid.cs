using UnityEngine;

namespace ElementsTask.Presentation.Services
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] 
        private float _cellSize = 1.0f;
        
        private Transform[,] _cells;
        
        private void Generate(int cellsCount)
        {
            Vector3 origin = transform.localPosition;

            /*for (int i = 0; i < UPPER; i++)
            {
                
            }*/

        }

    }
}