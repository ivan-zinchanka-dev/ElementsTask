using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ElementsTask.Common.Animations
{
    public class HorizontalSinusAnimation : MonoBehaviour
    {
        [Serializable]
        public class Data
        {
            public float Duration = 15f;
            public float Distance = 10f;
            public float Frequency = 3f;
            public float Amplitude = 1f;
        }

        public async UniTask FlyToRightAsync(Data data)
        {
            float startX = transform.position.x;
            float endX = startX + data.Distance;

            await FlyAsync(data, endX);
        }

        public async UniTask FlyToLeftAsync(Data data)
        {
            float startX = transform.position.x;
            float endX = startX - data.Distance;

            await FlyAsync(data, endX);
        }
        
        private async UniTask FlyAsync(Data data, float endX)
        {
            float baseY = transform.position.y;

            await transform.DOMoveX(endX, data.Duration).SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    float x = transform.position.x;
                    float yOffset = Mathf.Sin(x * data.Frequency) * data.Amplitude;
                    transform.position = new Vector3(x, baseY + yOffset, transform.position.z);
                })
                .SetLink(gameObject)
                .ToUniTask();
        }
    }
}