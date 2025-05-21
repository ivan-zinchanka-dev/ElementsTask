using System;
using System.Collections.Generic;
using ElementsTask.Data.BlockFieldCore.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace ElementsTask.Data.PlayerProgression.Models
{
    public class PlayerProgress
    {
        [JsonProperty] 
        public int CurrentLevelIndex { get; set; } = 0;
        
        [JsonProperty]
        public Dictionary<Vector2Int, Block> BlockFieldState = new Dictionary<Vector2Int, Block>();
        
        public bool HasSavedFieldState => BlockFieldState != null && BlockFieldState.Count > 0;
        
        internal event Action OnDemandSave;
        
        public void Save()
        {
            OnDemandSave?.Invoke();
        }
    }
}