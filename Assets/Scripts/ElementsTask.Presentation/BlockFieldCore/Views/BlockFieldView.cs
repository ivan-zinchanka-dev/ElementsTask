using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ElementsTask.Common.Extensions;
using ElementsTask.Core.Models;
using ElementsTask.Core.Services;
using ElementsTask.Presentation.BlockFieldCore.Models;
using ElementsTask.Presentation.BlockFieldCore.Services.Factories;
using ElementsTask.Presentation.BlockFieldCore.Services.Handlers;
using ElementsTask.Presentation.Components.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ElementsTask.Presentation.BlockFieldCore.Views
{
    public class BlockFieldView : MonoBehaviour
    {
        [SerializeField]
        private BlockFieldViewOptions _options;
        [SerializeField] 
        private BlockFieldViewGrid _grid;
     
        [Inject]
        private Camera _camera;
        [Inject]
        private BlockFieldCreator _blockFieldCreator;
        [Inject] 
        private BlockViewsFactory _blockViewsFactory;
        
        private GridCell<BlockView> _selectedCell;
        private BlockSortingOrdersDictionary _cachedSortingOrders;
        private IObjectResolver _selfDiContainer;
        
        private BlocksSwappingHandler _blocksSwappingHandler;
        private BlocksFallingHandler _blocksFallingHandler;
        private BlocksDestructionHandler _blocksDestructionHandler;
        
        public async Task InitializeAsync()
        {
            BlockField fieldModel = await _blockFieldCreator.CreateFieldAsync();

            if (fieldModel == null)
            {
                Debug.LogError("Field not loaded");
                return;
            }

            _cachedSortingOrders = new BlockSortingOrdersDictionary(fieldModel.Size);
            
            for (int y = 0; y < fieldModel.Height; y++)
            {
                for (int x = 0; x < fieldModel.Width; x++)
                {
                    GridCell<BlockView> cell = _grid.GetCell(x, y);
                    
                    Block block = fieldModel.GetBlock(x, y);
                    BlockType blockType = block.Type;

                    if (blockType != BlockType.Empty)
                    {
                        BlockView createdBlock = _blockViewsFactory
                            .CreateBlockView(blockType, cell.Transform)
                            .SetModel(block)
                            .SetSortingOrder(_cachedSortingOrders.GetSortingOrder(x, y));

                        cell.Content = createdBlock;
                    }
                    else
                    {
                        cell.Content = null;
                    }
                }
            }
            
            var diBuilder = new ContainerBuilder();
            diBuilder.RegisterComponent<BlockFieldViewGrid>(_grid);
            diBuilder.RegisterInstance<BlockFieldViewOptions>(_options);
            diBuilder.RegisterInstance<BlockSortingOrdersDictionary>(_cachedSortingOrders);
            diBuilder.Register<BlocksSwappingHandler>(Lifetime.Transient);
            diBuilder.Register<BlocksFallingHandler>(Lifetime.Transient);
            diBuilder.Register<BlocksDestructionHandler>(Lifetime.Transient);
            
            _selfDiContainer = diBuilder.Build();
            
            _blocksSwappingHandler = _selfDiContainer.Resolve<BlocksSwappingHandler>();
            _blocksFallingHandler = _selfDiContainer.Resolve<BlocksFallingHandler>();
            _blocksDestructionHandler = _selfDiContainer.Resolve<BlocksDestructionHandler>();
            
            foreach (GridCell<BlockView> cell in _grid)
            {
                cell.Content?.OnSelected.AddListener(OnBlockSelected);
            }
        }

        [Button]
        private void FindToDestroy()
        {
            _blocksDestructionHandler.SimulateDestructionAsync().Forget();
        }

        public async Task ReInitialize()
        {
            Cleanup();
            await InitializeAsync();
        }
        
        public void Cleanup()
        {
            _blocksSwappingHandler?.Dispose();
            _blocksFallingHandler?.Dispose();
            
            _selfDiContainer.Dispose();
            
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

        [Button]
        private void SimGravity()
        {
            _blocksFallingHandler.SimulateFallingAsync().Forget();
        }

        private void Update()
        {
            if (IsPointerDownReceived(out Vector3 pointerPosition) && _selectedCell != null)
            {
                Debug.Log("TryMoveBlockAsync");
                
                _blocksSwappingHandler.TryMoveBlockAsync(_selectedCell, pointerPosition).ContinueWith(moved =>
                {
                    if (moved)
                    {
                        _blocksFallingHandler.SimulateFallingAsync().Forget();
                        _blocksDestructionHandler.SimulateDestructionAsync().ContinueWith(() =>
                        {
                            _blocksFallingHandler.SimulateFallingAsync().Forget();
                        });
                    }
                });
                
                _selectedCell = null;
            }
        }
    }
}