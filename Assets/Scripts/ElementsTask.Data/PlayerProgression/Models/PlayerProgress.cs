using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElementsTask.Data.BlockFieldCore.Models;
using ElementsTask.Data.PlayerProgression.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace ElementsTask.Data.PlayerProgression.Models
{
    public class PlayerProgress 
    {
        [JsonProperty] 
        public int CurrentLevelIndex { get; set; } = 0;
        
        [JsonProperty]
        [JsonConverter(typeof(BlockFieldStateConverter))]
        public Dictionary<Vector2Int, Block> BlockFieldState = new Dictionary<Vector2Int, Block>();
        
        [JsonIgnore]
        public bool HasSavedFieldState => BlockFieldState != null && BlockFieldState.Count > 0;
        
        internal event Func<Task> OnDemandSave;
        
        public async Task SaveAsync()
        {
            if (OnDemandSave != null)
            {
                await OnDemandSave();
            }
        }
    }
}