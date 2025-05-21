using System;
using UnityEngine;

namespace ElementsTask.Common.Data.Crypto.ROS
{
    [Serializable]
    public struct RosCryptoKey
    {
        private const int MinPartValue = 1;
        private const int MaxPartValue = 9;
        
        [field:SerializeField] [field:Range(MinPartValue, MaxPartValue)] 
        public int PartA { get; private set; }
        
        [field:SerializeField] [field:Range(MinPartValue, MaxPartValue)] 
        public int PartB { get; private set; }
        
        [field:SerializeField] [field:Range(MinPartValue, MaxPartValue)] 
        public int PartC { get; private set; }
        
        [field:SerializeField] [field:Range(MinPartValue, MaxPartValue)] 
        public int PartD { get; private set; }
    }
}