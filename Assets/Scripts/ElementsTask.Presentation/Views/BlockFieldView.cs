using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ElementsTask.Common.Extensions;
using ElementsTask.Core.Models;
using ElementsTask.Core.Services;
using ElementsTask.Presentation.BlockFieldCore.Components;
using ElementsTask.Presentation.Components.Grid;
using ElementsTask.Presentation.Services.BlockFieldHandlers;
using ElementsTask.Presentation.Services.Factories;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace ElementsTask.Presentation.Views
{
    public class BlockFieldView : MonoBehaviour
    {
        [SerializeField] 
        private BlockFieldViewGrid _grid;
     
        [Inject]
        private Camera _camera;
        [Inject]
        private BlockFieldCreator _blockFieldCreator;
        [Inject] 
        private BlockViewsFactory _blockViewsFactory;
        
        private Vector2Int _size;
        //private List<BlockView> _blocks;
        //private Dictionary<Vector2Int, Transform> _cells = new ();
        
        private GridCell<BlockView> _selectedCell;
        
        private BlocksMovingHandler _blocksMovingHandler;
        private BlocksFallingHandler _blocksFallingHandler;
        
        public async Task InitializeAsync()
        {
            BlockField fieldModel = await _blockFieldCreator.CreateFieldAsync();

            if (fieldModel == null)
            {
                Debug.LogError("Field not loaded");
                return;
            }

            _size = new Vector2Int(fieldModel.Width, fieldModel.Height);
            //_blocks = new List<BlockView>(_size.x * _size.y);
            int currentSortingOrder = 0;
            
            for (int y = 0; y < fieldModel.Height; y++)
            {
                for (int x = 0; x < fieldModel.Width; x++)
                {
                    GridCell<BlockView> cell = _grid.GetCell(x, y);
                    BlockView createdBlock = _blockViewsFactory
                        .CreateBlockView(fieldModel.GetBlock(x, y).Type, cell.Transform)
                        .SetModel(fieldModel.GetBlock(x, y))
                        .SetSortingOrder(currentSortingOrder);

                    cell.Content = createdBlock;
                    
                    //_blocks.Add(createdBlock);
                    currentSortingOrder++;
                }
            }
            
            _blocksMovingHandler = new BlocksMovingHandler(_size, _grid);
            _blocksFallingHandler = new BlocksFallingHandler(_size, _grid);
            
            foreach (GridCell<BlockView> cell in _grid)
            {
                cell.Content?.OnSelected.AddListener(OnBlockSelected);
            }
        }

        public async Task ReInitialize()
        {
            Cleanup();
            await InitializeAsync();
        }
        
        public void Cleanup()
        {
            _blocksMovingHandler?.Dispose();
            _blocksFallingHandler?.Dispose();
            
            foreach (GridCell<BlockView> cell in _grid)
            {
                if (cell.Content != null)
                {
                    cell.Content.OnSelected.RemoveListener(OnBlockSelected);
                    Destroy(cell.Content.gameObject);
                }
            }
        }

        private bool IsPointerDownReceived(out Vector3 pointerPosition)
        {
            if (Input.GetMouseButtonUp(0))
            {
                pointerPosition = _camera.ScreenToWorldPoint(Input.mousePosition).WithZ(0f);
                return true;
            }

            pointerPosition = default;
            return false;
        }

        private void OnBlockSelected(BlockView selectedBlock)
        {
            _selectedCell = _grid.FirstOrDefault(cell => cell.Content == selectedBlock);
        }

        private void Update()
        {
            if (IsPointerDownReceived(out Vector3 pointerPosition) && _selectedCell != null)
            {
                Debug.Log("TryMoveBlockAsync");
                
                _blocksMovingHandler.TryMoveBlockAsync(_selectedCell, pointerPosition).ContinueWith(moved =>
                {
                    if (moved)
                    {
                        //_blocksFallingHandler.StartFallingAsync().Forget();
                    }
                });
                
                _selectedCell = null;
            }
            /*else
            {
                _selectedBlock = null;
            }*/
        }
    }
}