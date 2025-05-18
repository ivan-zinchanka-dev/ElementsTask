using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ElementsTask.Presentation.Balloons
{
    public class BalloonViewsFactory : MonoBehaviour
    {
        [SerializeField] 
        private BalloonViewsContainer _viewsContainer;
        
        [Inject] 
        private IObjectResolver _diContainer;
        
        public BalloonView CreateBalloonView(BalloonKind balloonKind, Transform parent = null, bool worldPositionStays = false)
        {
            BalloonView originalView = _viewsContainer.GetViewByKind(balloonKind);

            if (originalView != null)
            {
                return _diContainer.Instantiate(originalView, parent, worldPositionStays);
            }

            return null;
        }
    }
}