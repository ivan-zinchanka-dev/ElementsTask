using System.Collections.Generic;
using ElementsTask.Presentation.Balloons.Enums;
using ElementsTask.Presentation.Balloons.Views;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ElementsTask.Presentation.Balloons.Containers
{
    [CreateAssetMenu(fileName = "balloon_views_container", menuName = "Scriptables/Containers/BalloonsViewsContainer", order = 0)]
    public class BalloonViewsContainer : SerializedScriptableObject
    {
        [OdinSerialize]
        private Dictionary<BalloonKind, BalloonView> _balloonViews = new ();
        
        public BalloonView GetViewByKind(BalloonKind balloonKind)
        {
            return _balloonViews.GetValueOrDefault(balloonKind);
        }
    }
}