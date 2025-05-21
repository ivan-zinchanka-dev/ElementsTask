using ElementsTask.Common.Animations;
using UnityEngine;

namespace ElementsTask.Presentation.Balloons.Views
{
    public class BalloonView : MonoBehaviour
    {
        [field:SerializeField] 
        public HorizontalSinusAnimation Animation { get; private set; }
    }
}