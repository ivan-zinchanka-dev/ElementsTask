using System;
using UnityEngine;

namespace ElementsTask.Presentation.BlockFieldCore.Models
{
    [Serializable]
    public class BlockFieldViewOptions
    {
        [field: SerializeField] 
        public float SwappingDuration { get; private set; } = 0.15f;

        [field: SerializeField] 
        public float FallingSpeed { get; private set; } = 0.15f;
    }
}