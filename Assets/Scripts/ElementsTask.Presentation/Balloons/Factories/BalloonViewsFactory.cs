using ElementsTask.Presentation.Balloons.Containers;
using ElementsTask.Presentation.Balloons.Enums;
using ElementsTask.Presentation.Balloons.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ElementsTask.Presentation.Balloons.Factories
{
    public class BalloonViewsFactory : MonoBehaviour
    {
        [SerializeField] 
        private BalloonViewsContainer _viewsContainer;
        
        [Inject] 
        private IObjectResolver _diContainer;
        
        public BalloonView CreateBalloonView(
            BalloonKind balloonKind, 
            Transform parent = null, 
            bool worldPositionStays = false)
        {
            BalloonView originalView = _viewsContainer.GetViewByKind(balloonKind);

            if (originalView != null)
            {
                return _diContainer.Instantiate(originalView, parent, worldPositionStays);
            }

            return null;
        }
        
        public BalloonView CreateBalloonView(
            BalloonKind balloonKind, 
            Vector3 position = default, 
            Quaternion rotation = default, 
            Transform parent = null)
        {
            BalloonView originalView = _viewsContainer.GetViewByKind(balloonKind);

            if (originalView != null)
            {
                return _diContainer.Instantiate(originalView, position, rotation, parent);
            }

            return null;
        }
    }
}