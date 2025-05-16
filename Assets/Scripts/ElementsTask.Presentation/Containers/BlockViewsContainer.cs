using System.Collections.Generic;
using ElementsTask.Core.Models;
using ElementsTask.Presentation.Views;
using UnityEngine;

namespace ElementsTask.Presentation.Containers
{
    [CreateAssetMenu(fileName = "block_views_container", menuName = "Scriptables/Containers/BlockViewsContainer", order = 0)]
    public class BlockViewsContainer : ScriptableObject
    {
        [SerializeField]
        private List<Pair<BlockType, BlockView>> _blockViews;

        public BlockView GetViewByType(BlockType type)
        {
            return _blockViews.Find(pair => pair.First == type).Second;
        }
    }
}