using System.Collections.Generic;
using System.Threading.Tasks;
using ElementsTask.Common.Services.Csv;
using ElementsTask.Core.Models;
using UnityEngine;

namespace ElementsTask.Core.Services
{
    public class CsvLevelLoader : ILevelLoader
    {
        private static readonly Vector2Int LevelSize = new Vector2Int(6, 9);
        
        public async Task<BlockField> LoadLevelAsync(int levelIndex)
        {
            TextAsset levelCsv = Resources.Load<TextAsset>(GetLevelResourceName(levelIndex));
            List<string[]> dataRows = await CsvReader.ReadDataRowsAsync(levelCsv.text);
            dataRows.Reverse();
            
            Block[,] blocks = new Block[dataRows.Count, LevelSize.x];
            
            for (int y = 0; y < blocks.GetLength(0); y++)
            {
                for (int x = 0; x < blocks.GetLength(1); x++)
                {
                    blocks[y, x] = new Block(BlockType.Parse(dataRows[y][x]));
                }
            }
            
            return new BlockField(blocks);
        }
        
        private string GetLevelResourceName(int levelIndex)
        {
            return $"Levels/lvl_{levelIndex}";
        }
    }
}