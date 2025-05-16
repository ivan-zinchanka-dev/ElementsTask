using System;
using UnityEngine;

namespace ElementsTask.Core.Models
{
    [Serializable]
    public class BlockType
    {
        [field:SerializeField]
        public string Id { get; private set; }
    }
}