using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace ElementsTask.Core.Models.Configs
{
    [CreateAssetMenu(fileName = "block_types_config", menuName = "Configs/BlockTypesConfig", order = 0)]
    public class BlockTypesConfig : ScriptableObject
    {
        [SerializeField]
        private List<BlockType> _blockTypes;

        [SerializeField] 
        private SpriteAtlas _blockSpriteAtlas;

        public IReadOnlyList<BlockType> BlockTypes => _blockTypes;
        
    }
}