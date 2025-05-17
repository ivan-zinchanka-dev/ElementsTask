using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ElementsTask.Common.Extensions;
using ElementsTask.Core.Models;
using ElementsTask.Core.Services;
using ElementsTask.Presentation.Services.BlockFieldHandlers;
using ElementsTask.Presentation.Services.Factories;
using UnityEngine;
using VContainer;

namespace ElementsTask.Presentation.Views
{
    public class BlockFieldView : MonoBehaviour
    {
        [SerializeField] 
        private Services.Grid _grid;
     
        [Inject]
        private Camera _camera;
        [Inject]
        private BlockFieldCreator _blockFieldCreator;
        [Inject] 
        private BlockViewsFactory _blockViewsFactory;
        
        private Vector2Int _size;
        private List<BlockView> _blocks;
        private BlockView _selectedBlock;
        
        private BlocksMovingHandler _blocksMovingHandler;
        private BlocksFallingHandler _blocksFallingHandler;
        
        public async Task InitializeAsync()
        {
            BlockField fieldModel = await _blockFieldCreator.CreateFieldAsync();
            
            _size = new Vector2Int(fieldModel.Width, fieldModel.Height);
            _blocks = new List<BlockView>(_size.x * _size.y);
            int currentSortingOrder = 0;
            
            for (int y = 0; y < fieldModel.Height; y++)
            {
                for (int x = 0; x < fieldModel.Width; x++)
                {
                    Transform cell = _grid.Cells[y, x];
                    BlockView createdBlock = _blockViewsFactory
                        .CreateBlockView(fieldModel.GetBlock(x, y).Type, cell)
                        .SetModel(fieldModel.GetBlock(x, y))
                        .SetGridPosition(new Vector2Int(x, y))
                        .SetSortingOrder(currentSortingOrder);

                    _blocks.Add(createdBlock);
                    currentSortingOrder++;
                }
            }
            
            _blocksMovingHandler = new BlocksMovingHandler(_size, _blocks);
            _blocksFallingHandler = new BlocksFallingHandler(_size, _blocks);
            
            foreach (BlockView block in _blocks)
            {
                block.OnSelected.AddListener(OnBlockSelected);
            }
        }

        public void ReInitialize()
        {
            Cleanup();
            InitializeAsync();
        }
        
        public void Cleanup()
        {
            foreach (BlockView block in _blocks)
            {
                block.OnSelected.RemoveListener(OnBlockSelected);
            }
            
            _blocksMovingHandler?.Dispose();
            _blocksFallingHandler?.Dispose();

            foreach (BlockView block in _blocks)
            {
                if (block != null)
                {
                    Destroy(block.gameObject);
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
            _selectedBlock = selectedBlock;
        }

        private void Update()
        {
            if (IsPointerDownReceived(out Vector3 pointerPosition) && _selectedBlock != null)
            {
                _blocksMovingHandler.TryMoveBlockAsync(_selectedBlock, pointerPosition).ContinueWith(moved =>
                {
                    if (moved)
                    {
                        _blocksFallingHandler.SimulateFallingAsync().Forget();
                    }
                });
            }
        }
    }
}