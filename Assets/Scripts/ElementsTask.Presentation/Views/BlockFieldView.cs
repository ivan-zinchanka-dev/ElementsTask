using ElementsTask.Common.Extensions;
using ElementsTask.Core.Models;
using ElementsTask.Core.Services;
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
        
        private BlockView[,] _blocks;
        private BlockView _selectedBlock;

        private void Awake()
        {
            BlockField fieldModel = _blockFieldCreator.Create();
            _blocks = new BlockView[fieldModel.Width, fieldModel.Height];
            int currentSortingOrder = 0;
            
            for (int i = 0; i < fieldModel.Width; i++)
            {
                for (int j = 0; j < fieldModel.Height; j++)
                {
                    Transform cell = _grid.Cells[i, j];
                    BlockView createdBlock = _blockViewsFactory
                        .CreateBlockView(fieldModel.Blocks[i, j].Type, cell)
                        .SetModel(fieldModel.Blocks[i, j])
                        .SetSortingOrder(currentSortingOrder);

                    _blocks[i, j] = createdBlock;
                    currentSortingOrder++;
                }
            }
        }

        private void OnEnable()
        {
            foreach (BlockView block in _blocks)
            {
                block.OnSelected.AddListener(OnBlockSelected);
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
            
            GameObject.CreatePrimitive(PrimitiveType.Quad).transform.position = _selectedBlock.transform.position;
        }

        private void Update()
        {
            if (IsPointerDownReceived(out Vector3 pointerPosition) && _selectedBlock != null)
            {
                GameObject.CreatePrimitive(PrimitiveType.Quad).transform.position = pointerPosition;
            }
        }
        
        private void OnDisable()
        {
            foreach (BlockView block in _blocks)
            {
                block.OnSelected.RemoveListener(OnBlockSelected);
            }
        }
    }
}