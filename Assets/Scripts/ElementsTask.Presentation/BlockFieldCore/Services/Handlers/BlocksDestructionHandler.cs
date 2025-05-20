using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ElementsTask.Presentation.BlockFieldCore.Views;
using ElementsTask.Presentation.Components.Grid;
using UnityEngine;

namespace ElementsTask.Presentation.BlockFieldCore.Services.Handlers
{
    public class BlocksDestructionHandler : IDisposable
    {
        private readonly BlockFieldViewGrid _grid;
        
        private Sequence _destructionTween;

        public BlocksDestructionHandler(BlockFieldViewGrid grid)
        {
            _grid = grid;
        }


        public async UniTask SimulateDestructionAsync()
        {
            if (_destructionTween.IsActive())
            {
                return;
            }
            
            _destructionTween = DOTween.Sequence();

            TrySimulateDestruction();
        }

        private bool TrySimulateDestruction()
        {
            bool needSimulation = false;

            var regions = FindRegionsIncludingMatches(_grid, FindMatchLineCells(_grid));

            foreach (var region in regions)
            {
                foreach (var position in region)
                { 
                    var c = _grid.GetCell(position).Content;

                    if (c != null)
                    {
                        c.transform.localScale *= 0.85f;
                    }
                }
            }

            return needSimulation;
        }
        
        
        private HashSet<Vector2Int> FindMatchLineCells(BlockFieldViewGrid grid)
        {
            var result = new HashSet<Vector2Int>();
            int height = grid.Height;
            int width = grid.Width;
            
            for (int y = 0; y < height; y++)
            {
                int count = 1;
                for (int x = 1; x < width; x++)
                {
                    if (grid.GetCell(x, y)?.Content != null && 
                        grid.GetCell(x, y).Content.Type == grid.GetCell(x - 1, y)?.Content.Type)
                    {
                        count++;
                    }
                    else
                    {
                        if (count >= 3)
                            for (int k = 0; k < count; k++)
                                result.Add(new Vector2Int(x - 1 - k, y));
                        count = 1;
                    }
                }
                if (count >= 3)
                    for (int k = 0; k < count; k++)
                        result.Add(new Vector2Int(width - 1 - k, y));
            }
            
            for (int x = 0; x < width; x++)
            {
                int count = 1;
                for (int y = 1; y < height; y++)
                {
                    if (grid.GetCell(x, y)?.Content != null &&
                        grid.GetCell(x, y).Content.Type == grid.GetCell(x, y - 1)?.Content.Type)
                    {
                        count++;
                    }
                    else
                    {
                        if (count >= 3)
                            for (int k = 0; k < count; k++)
                                result.Add(new Vector2Int(x, y - 1 - k));
                        count = 1;
                    }
                }
                if (count >= 3)
                    for (int k = 0; k < count; k++)
                        result.Add(new Vector2Int(x, height - 1 - k));
            }

            return result;
        }
        
        private List<HashSet<Vector2Int>> FindRegionsIncludingMatches(
            BlockFieldViewGrid grid,
            HashSet<Vector2Int> matchPoints)
        {
            var visited = new bool[grid.Height, grid.Width];
            var result = new List<HashSet<Vector2Int>>();

            foreach (var point in matchPoints)
            {
                int y = point.y;
                int x = point.x;

                if (visited[y, x]) continue;

                BlockView block = grid.GetCell(x, y).Content;
                var region = new HashSet<Vector2Int>();
                var queue = new Queue<Vector2Int>();
                queue.Enqueue(point);
                visited[y, x] = true;

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    region.Add(current);

                    foreach (var dir in new[]{ Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                    {
                        var nx = current.x + dir.x;
                        var ny = current.y + dir.y;

                        if (nx >= 0 && nx < grid.Width &&
                            ny >= 0 && ny < grid.Height &&
                            !visited[ny, nx] &&
                            grid.GetCell(nx, ny)?.Content != null &&
                            grid.GetCell(nx, ny).Content.Type == block.Type)
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
        
        public void Dispose()
        {
            _destructionTween.Kill();
        }
    }
}