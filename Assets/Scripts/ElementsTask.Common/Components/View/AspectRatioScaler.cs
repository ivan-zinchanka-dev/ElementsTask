using UnityEngine;

namespace ElementsTask.Common.Components.View
{
    public class AspectRatioScaler : MonoBehaviour
    {
        [SerializeField] 
        private Vector2 _referenceAspect = new Vector2(1080f, 2340f);
        
        [Header("Scale Limits")]
        [SerializeField]
        private float _minScale = 0.9f;
        [SerializeField]
        private float _maxScale = 1.3f;
        
        public void Scale()
        {
            ValidateScaleLimits();
            
            float currentAspect = (float)Screen.width / Screen.height;
            float referenceAspectRatio = _referenceAspect.x / _referenceAspect.y;
            
            float scaleFactor = Mathf.Clamp(currentAspect / referenceAspectRatio, _minScale, _maxScale);
            transform.localScale = Vector3.one * scaleFactor;
        }
        
        private void ValidateScaleLimits()
        {
            if (_minScale > _maxScale)
            {
                Debug.LogError("[AspectRatioScaler] Min scale should be less than or equal to max scale.");
            }
        }
    }
}