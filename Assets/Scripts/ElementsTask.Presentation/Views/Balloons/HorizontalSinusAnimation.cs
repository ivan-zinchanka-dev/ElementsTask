using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ElementsTask.Presentation.Views.Balloons
{
    public class HorizontalSinusAnimation : MonoBehaviour
    {
        public class Data
        {
            public float Duration = 15f;
            public float Distance = 10f;
            public float Frequency = 3f;
            public float Amplitude = 1f;
        }
        
        private void Start()
        {
            FlyToLeftAsync(15f, 10f, 5f, 1f).Forget();
        }

        public async UniTask FlyToRightAsync(float duration, float distance, float frequency, float amplitude)
        {
            float startX = transform.position.x;
            float endX = startX + distance;

            await FlyAsync(duration, distance, frequency, amplitude, endX);
        }

        public async UniTask FlyToLeftAsync(float duration, float distance, float frequency, float amplitude)
        {
            float startX = transform.position.x;
            float endX = startX - distance;

            await FlyAsync(duration, distance, frequency, amplitude, endX);
        }
        
        private async UniTask FlyAsync(float duration, float distance, float frequency, float amplitude, float endX)
        {
            await transform.DOMoveX(endX, duration).SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    float x = transform.position.x;
                    float y = Mathf.Sin(x * frequency) * amplitude;
                    transform.position = new Vector3(x, y, transform.position.z);
                })
                .SetLink(gameObject)
                .ToUniTask();
        }
    }
}