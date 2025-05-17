using System.Collections.Generic;
using ElementsTask.Common.Extensions;
using ElementsTask.Core.Models;
using ElementsTask.Core.Services;
using ElementsTask.Presentation.Enums;
using ElementsTask.Presentation.Models;
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
        
        private void Awake()
        {
            BlockField fieldModel = _blockFieldCreator.Create();
            
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
            
            //GameObject.CreatePrimitive(PrimitiveType.Quad).transform.position = _selectedBlock.transform.position;
        }

        private void Update()
        {
            if (IsPointerDownReceived(out Vector3 pointerPosition) && _selectedBlock != null)
            {
                BlockView pair = GetSwapPair(_selectedBlock, 
                    GetMovingDirection(pointerPosition - _selectedBlock.transform.position));

                if (pair != null)
                {
                    Swap(_selectedBlock, pair);
                }

                //GameObject.CreatePrimitive(PrimitiveType.Quad).transform.position = pointerPosition;
            }
        }

        private static BlockMovingDirection GetMovingDirection(Vector3 inputDirection)
        {
            if (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y))
            {
                return inputDirection.x > 0f ? BlockMovingDirection.Right : BlockMovingDirection.Left;
            }
            else
            {
                return inputDirection.y > 0f ? BlockMovingDirection.Up : BlockMovingDirection.Down;
            }
        }

        private void Swap(BlockView first, BlockView second)
        {
            BlockSwapData firstData = first.GetSwapData();
            BlockSwapData secondData = second.GetSwapData();

            first.transform.position = secondData.WorldPosition;
            first
                .SetGridPosition(secondData.GridPosition)
                .SetSortingOrder(secondData.SortingOrder);
            
            second.transform.position = firstData.WorldPosition;
            second
                .SetGridPosition(firstData.GridPosition)
                .SetSortingOrder(firstData.SortingOrder);
        }


        private BlockView GetSwapPair(BlockView origin, BlockMovingDirection swapDirection)
        {
            if (swapDirection == BlockMovingDirection.Up)
            {
                if (origin.GridPosition.y < _size.y - 1)
                {
                    return _blocks.Find(block => 
                        block.GridPosition.x == origin.GridPosition.x && 
                        block.GridPosition.y == origin.GridPosition.y + 1);
                }

                return null;
            }
            else if (swapDirection == BlockMovingDirection.Down)
            {
                if (origin.GridPosition.y > 0)
                {
                    return _blocks.Find(block => 
                        block.GridPosition.x == origin.GridPosition.x && 
                        block.GridPosition.y == origin.GridPosition.y - 1);
                }

                return null;
            }
            else if (swapDirection == BlockMovingDirection.Right)
            {
                if (origin.GridPosition.x < _size.x - 1)
                {
                    return _blocks.Find(block => 
                        block.GridPosition.x == origin.GridPosition.x + 1 && 
                        block.GridPosition.y == origin.GridPosition.y);
                }

                return null;
            }
            else if (swapDirection == BlockMovingDirection.Left)
            {
                if (origin.GridPosition.x > 0)
                {
                    return _blocks.Find(block => 
                        block.GridPosition.x == origin.GridPosition.x - 1 && 
                        block.GridPosition.y == origin.GridPosition.y);
                }

                return null;
            }

            return null;
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