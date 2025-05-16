using ElementsTask.Core.Models;
using ElementsTask.Core.Services;
using ElementsTask.Presentation.Services.Factories;
using UnityEngine;
using VContainer;
using Grid = ElementsTask.Presentation.Services.Grid;

namespace ElementsTask.Presentation.Management
{
    public class GameSceneStartup : MonoBehaviour
    {
        [Inject]
        private BlockFieldCreator _blockFieldCreator;
        [Inject] 
        private BlockViewsFactory _blockViewsFactory;
        [Inject] 
        private Grid _grid;    
        
        private void Awake()
        {
            _grid.Generate();
            
            BlockField blockField = _blockFieldCreator.Create();

            int currentSortingOrder = 0;
            
            for (int i = 0; i < blockField.Blocks.GetLength(0); i++)
            {
                for (int j = 0; j < blockField.Blocks.GetLength(1); j++)
                {
                    // TODO Refactor Grid
                    Transform cell = _grid.Cells[i, j];
                    _blockViewsFactory
                        .CreateBlockView(blockField.Blocks[i, j].Type, cell)
                        .SetModel(blockField.Blocks[i, j])
                        .SetSortingOrder(currentSortingOrder);
                    
                    currentSortingOrder++;
                }
            }
        }
    }
}