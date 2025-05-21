using System.Collections.Generic;
using System.Threading.Tasks;
using ElementsTask.Common.Data.Csv;
using ElementsTask.Data.BlockFieldCore.Models;
using UnityEngine;

namespace ElementsTask.Data.Levels.Services
{
    public class BuildInLevelLoader : IBuildInLevelLoader
    {
        public async Task<BlockField> LoadLevelAsync(int levelIndex)
        {
            string levelName = GetLevelResourceName(levelIndex);
            TextAsset levelCsv = Resources.Load<TextAsset>(levelName);

            if (levelCsv != null)
            {
                Debug.Log($"File {levelName}.csv loaded");
            }
            else
            {
                Debug.LogWarning($"File {levelName}.csv not found");
                return null;
            }

            List<string[]> dataRows = await CsvReader.ReadDataRowsAsync(levelCsv.text);
            
            Resources.UnloadAsset(levelCsv);
            dataRows.Reverse();
            
            Block[,] blocks = new Block[dataRows.Count, BlockFieldConstraints.LevelSize.x];
            
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