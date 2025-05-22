using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ElementsTask.Common.Components.Grid;
using ElementsTask.Data.BlockFieldCore.Enums;
using ElementsTask.Data.BlockFieldCore.Models;
using ElementsTask.Presentation.BlockFieldCore.Views;
using UnityEngine;

namespace ElementsTask.Presentation.BlockFieldCore.Services.Handlers
{
    public class BlocksDestructionHandler : IDisposable
    {
        private const int TargetMatchCells = 3;
        private static readonly Vector2Int[] Directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        private readonly BlockFieldViewGrid _grid;

        private List<UniTask> _destructionTasks;
        
        public BlocksDestructionHandler(BlockFieldViewGrid grid)
        {
            _grid = grid;
        }
        
        public async UniTask<bool> SimulateDestructionIfNeedAsync()
        {
            if (_destructionTasks != null)
            {
                return false;
            }
            
            List<HashSet<Vector2Int>> regions = FindRegionsIncludingMatches(_grid, FindMatchLineCells(_grid));

            if (regions.Count == 0)
            {
                return false;
            }

            _destructionTasks = new List<UniTask>();
            
            foreach (HashSet<Vector2Int> region in regions)
            {
                foreach (Vector2Int cellPosition in region)
                { 
                    BlockView block = _grid.GetCell(cellPosition).Content;

                    if (block != null)
                    {
                        _destructionTasks.Add(block.SelfDestroyAsync());
                    }
                }
            }

            await UniTask.WhenAll(_destructionTasks);
            
            _destructionTasks.Clear();
            _destructionTasks = null;
            return true;
        }
        
        private HashSet<Vector2Int> FindMatchLineCells(BlockFieldViewGrid grid)
        {
            var result = new HashSet<Vector2Int>();

            int width = grid.Width;
            int height = grid.Height;
            
            for (int y = 0; y < height; y++)
            {
                AddMatchesInLine(result, width,
                    i => grid.GetCell(i, y)?.Content,
                    i => new Vector2Int(i, y));
            }
            
            for (int x = 0; x < width; x++)
            {
                AddMatchesInLine(result, height,
                    i => grid.GetCell(x, i)?.Content,
                    i => new Vector2Int(x, i));
            }

            return result;
        }
        
        
        private void AddMatchesInLine(
            HashSet<Vector2Int> result,
            int length,
            Func<int, BlockView> getBlock,
            Func<int, Vector2Int> getCoord)
        {
            int matchCells = 1;
            for (int i = 1; i < length; i++)
            {
                BlockView currentBlock = getBlock(i);
                BlockView previousBlock = getBlock(i - 1);

                if (IsRelevant(currentBlock) && IsRelevant(previousBlock) && currentBlock.Type == previousBlock.Type)
                {
                    matchCells++;
                }
                else
                {
                    if (matchCells >= TargetMatchCells)
                    {
                        for (int k = 0; k < matchCells; k++)
                        {
                            result.Add(getCoord(i - 1 - k));
                        }
                    }
                    
                    matchCells = 1;
                }
            }

            if (matchCells >= TargetMatchCells)
            {
                for (int k = 0; k < matchCells; k++)
                {
                    result.Add(getCoord(length - 1 - k));
                }
            }
        }
        
        private List<HashSet<Vector2Int>> FindRegionsIncludingMatches(
            BlockFieldViewGrid grid,
            HashSet<Vector2Int> matchPoints)
        {
            var visited = new bool[grid.Height, grid.Width];
            var result = new List<HashSet<Vector2Int>>();

            foreach (Vector2Int point in matchPoints)
            {
                int y = point.y;
                int x = point.x;

                if (visited[y, x])
                {
                    continue;
                }
                
                BlockView block = grid.GetCell(x, y).Content;
                var region = new HashSet<Vector2Int>();
                var queue = new Queue<Vector2Int>();
                queue.Enqueue(point);
                visited[y, x] = true;

                while (queue.Count > 0)
                {
                    Vector2Int current = queue.Dequeue();
                    region.Add(current);

                    foreach (Vector2Int direction in Directions)
                    {
                        int nx = current.x + direction.x;
                        int ny = current.y + direction.y;

                        if (nx >= 0 && nx < grid.Width &&
                            ny >= 0 && ny < grid.Height &&
                            !visited[ny, nx] &&
                            HasBlockType(grid.GetCell(nx, ny), block.Type))
                        {
                            queue.Enqueue(new Vector2Int(nx, ny));
                            visited[ny, nx] = true;
                        }
                    }
                }

                result.Add(region);
            }

            return result;
        }

        private static bool IsRelevant(BlockView block)
        {
            return block != null && block.State == BlockState.Idle;
        }

        private static bool HasBlockType(GridCell<BlockView> cell, BlockType blockType)
        {
            return cell != null && IsRelevant(cell.Content) && cell.Content.Type == blockType;
        }

        public void Dispose()
        {
            if (_destructionTasks != null)
            {
                _destructionTasks.Clear();
            }
        }
    }
}