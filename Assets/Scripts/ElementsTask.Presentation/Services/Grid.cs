using UnityEngine;

namespace ElementsTask.Presentation.Services
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] 
        private float _cellSize = 1.0f;
        
        private Transform[,] _cells;
        
        [Sirenix.OdinInspector.Button]
        private void Generate(int cellsCount)
        {
            Vector3 origin = transform.localPosition;

            Debug.Log("Generate");
            
            /*for (int i = 0; i < UPPER; i++)
            {
                
            }*/

        }

    }
}