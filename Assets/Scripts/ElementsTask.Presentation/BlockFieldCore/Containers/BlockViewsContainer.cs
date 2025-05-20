using System.Collections.Generic;
using ElementsTask.Core.Models;
using ElementsTask.Presentation.BlockFieldCore.Views;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ElementsTask.Presentation.BlockFieldCore.Containers
{
    [CreateAssetMenu(fileName = "block_views_container", menuName = "Scriptables/Containers/BlockViewsContainer", order = 0)]
    public class BlockViewsContainer : SerializedScriptableObject
    {
        [OdinSerialize]
        private Dictionary<BlockType, BlockView> _blockViews;

        public BlockView GetViewByType(BlockType type)
        {
            return _blockViews.GetValueOrDefault(type);
        }
    }
}